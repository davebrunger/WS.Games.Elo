using System;
using System.Collections.Generic;
using System.Linq;
using WS.Games.Elo.Lib.Elo;
using WS.Games.Elo.Lib.Models;
using WS.Games.Elo.Lib.Repositories;

namespace WS.Games.Elo.Lib.Services
{
    public class GameService
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly EloCalculator eloCalculator;
        private readonly IGameServiceConfiguration configuration;

        public GameService(IRepositoryFactory repositoryFactory, EloCalculator eloCalculator, IGameServiceConfiguration configuration)
        {
            this.repositoryFactory = repositoryFactory;
            this.eloCalculator = eloCalculator;
            this.configuration = configuration;
        }

        public void AddNewGame(string name, string gameType, int minimumPlayerCount, int maximumPlayerCount)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (name != name.Trim())
            {
                throw new ArgumentException("Game name cannot begin or end with whitspace", nameof(name));
            }
            if (gameType != null && gameType != gameType.Trim())
            {
                throw new ArgumentException("Game type cannot begin or end with whitspace", nameof(gameType));
            }
            if (minimumPlayerCount < 1)
            {
                throw new ArgumentException("Game must be for at least 1 player", nameof(minimumPlayerCount));
            }
            if (maximumPlayerCount < minimumPlayerCount)
            {
                throw new ArgumentException("Maximum player count must not be less than minimum player count", nameof(maximumPlayerCount));
            }

            using (var gameRepository = repositoryFactory.GetRepository<Game>())
            {
                var newGame = new Game
                {
                    Name = name,
                    GameType = gameType != "" ? gameType : null,
                    MinimumPlayerCount = minimumPlayerCount,
                    MaximumPlayerCount = maximumPlayerCount,
                };
                var existingGame = gameRepository.Get(newGame);
                if (existingGame != null)
                {
                    throw new ArgumentException($"A game with the name {name} already exists", nameof(name));
                }
                gameRepository.Put(newGame);
            }
        }

        public void RecalculateRatings()
        {
            List<GameResult> gameResults;

            using (var playerRepository = repositoryFactory.GetRepository<Player>())
            using (var gameResultRepository = repositoryFactory.GetRepository<GameResult>())
            {
                var players = playerRepository.Get();
                foreach (var player in players)
                {
                    player.Rating = configuration.NewPlayerRating;
                    playerRepository.Put(player);
                }

                gameResults = gameResultRepository.Get()
                    .OrderBy(r => r.StartTime)
                    .ThenBy(r => r.Game)
                    .ToList();

                gameResultRepository.Clear();
            }

            foreach (var gameResult in gameResults)
            {
                var playerNamesByPosition = gameResult.PlayerResultsByPosition
                    .Select(prs => prs.Select(pr => pr.PlayerName).ToList())
                    .ToList();
                RegisterCompletedGame(gameResult.Game, gameResult.StartTime, gameResult.Location, playerNamesByPosition);
            }
        }

        public void RegisterCompletedGame(string gameName, DateTime startTime, string location, IReadOnlyCollection<IReadOnlyCollection<string>> playerNamesByPosition)
        {
            using (var gameRepository = repositoryFactory.GetRepository<Game>())
            using (var playerRepository = repositoryFactory.GetRepository<Player>())
            using (var gameResultRepository = repositoryFactory.GetRepository<GameResult>())
            {

                var game = gameRepository.Get(new Game { Name = gameName });
                if (game == null)
                {
                    throw new ArgumentException($"Unrecogised game name: {gameName}", nameof(gameName));
                }
                var playerCount = playerNamesByPosition.Sum(pns => pns.Count);
                if (playerCount < game.MinimumPlayerCount || playerCount > game.MaximumPlayerCount)
                {
                    throw new ArgumentException($"Invalid number of players for {gameName}: {playerCount}", nameof(playerNamesByPosition));
                }

                var recognisedPlayerNames = new HashSet<string>();
                var duplicatePlayerNames = new HashSet<string>();
                var unrecognisedPlayerNames = new HashSet<string>();

                var playersByPosition = playerNamesByPosition
                    .Select(pns => pns.Select(pn =>
                    {
                        if (recognisedPlayerNames.Contains(pn))
                        {
                            duplicatePlayerNames.Add(pn);
                            return null;
                        }
                        var player = playerRepository.Get(new Player { Name = pn });
                        if (player == null)
                        {
                            unrecognisedPlayerNames.Add(pn);
                            return null;
                        }
                        recognisedPlayerNames.Add(pn);
                        return player;
                    }).ToList()).ToList();

                if (duplicatePlayerNames.Count > 0)
                {
                    throw new ArgumentException($"Duplicate player name(s): {string.Join(", ", duplicatePlayerNames)}",
                        nameof(playerNamesByPosition));
                }

                if (unrecognisedPlayerNames.Count > 0)
                {
                    throw new ArgumentException($"Duplicate player name(s): {string.Join(", ", duplicatePlayerNames)}",
                        nameof(playerNamesByPosition));
                }

                var playersWithRatingsAfter = eloCalculator.UpdateRatings(playersByPosition);

                var playerResultsByPosition = playersByPosition
                    .Select((ps, psi) => ps.Select((p, pi) => new PlayerResult
                    {
                        PlayerName = p.Name,
                        RatingBefore = p.Rating,
                        RatingAfter = playersWithRatingsAfter.Single(a => p.IdentifiesWith(a)).Rating
                    }).ToList())
                    .ToList();

                var gameResult = new GameResult
                {
                    Game = game.Name,
                    StartTime = startTime,
                    Location = location,
                    PlayerResultsByPosition = playerResultsByPosition
                };

                gameResultRepository.Put(gameResult);

                foreach (var player in playersWithRatingsAfter)
                {
                    playerRepository.Put(player);
                }
            }
        }
    }
}