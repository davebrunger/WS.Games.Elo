using System;
using System.Collections.Generic;
using System.Linq;
using WS.Games.Elo.Lib.Models;
using WS.Games.Elo.Lib.Repositories;

namespace WS.Games.Elo.Lib.Services
{
    public class PlayerService
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IPlayerServiceConfiguration configuration;

        public PlayerService(IRepositoryFactory repositoryFactory, IPlayerServiceConfiguration configuration)
        {
            this.repositoryFactory = repositoryFactory;
            this.configuration = configuration;
        }

        public void AddNewPlayer(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (name != name.Trim())
            {
                throw new ArgumentException("Player name cannot begin or end with whitspace", nameof(name));
            }

            using (var playerRepository = repositoryFactory.GetRepository<Player>())
            {
                var newPlayer = new Player
                {
                    Name = name,
                    Rating = configuration.NewPlayerRating
                };
                var existingPlayer = playerRepository.Get(newPlayer);
                if (existingPlayer != null)
                {
                    throw new ArgumentException($"A player with the name {name} already exists", nameof(name));
                }
                playerRepository.Put(newPlayer);
            }
        }

        public void RenamePlayer(string oldName, string newName)
        {
            if (string.IsNullOrWhiteSpace(oldName))
            {
                throw new ArgumentNullException(nameof(oldName));
            }
            if (oldName != oldName.Trim())
            {
                throw new ArgumentException("Player name cannot begin or end with whitspace", nameof(oldName));
            }
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentNullException(nameof(newName));
            }
            if (newName != newName.Trim())
            {
                throw new ArgumentException("Player name cannot begin or end with whitspace", nameof(newName));
            }

            using (var playerRepository = repositoryFactory.GetRepository<Player>())
            using (var gameResultRepository = repositoryFactory.GetRepository<GameResult>())
            {
                var player = playerRepository.Get(new Player { Name = oldName });
                if (player == null)
                {
                    throw new ArgumentException($"Unrecogised player name: {oldName}", nameof(oldName));
                }
                if (newName == oldName)
                {
                    return;
                }
                var existingPlayer = playerRepository.Get(new Player { Name = newName });
                if (existingPlayer != null)
                {
                    throw new ArgumentException($"A player with the name {newName} already exists", nameof(newName));
                }
                playerRepository.Delete(player);
                player.Name = newName;
                playerRepository.Put(player);

                var allGameResults = gameResultRepository.Get();
                foreach (var gameResult in allGameResults)
                {
                    if (!gameResult.PlayerResultsByPosition.Any(prs => prs.Any(pr => pr.PlayerName == oldName)))
                    {
                        continue;
                    }
                    gameResult.PlayerResultsByPosition = gameResult.PlayerResultsByPosition
                        .Select(prs => prs.Select(pr => pr.PlayerName == oldName
                            ? new PlayerResult
                            {
                                PlayerName = newName,
                                RatingBefore = pr.RatingBefore,
                                RatingAfter = pr.RatingAfter
                            }
                            : pr).ToList())
                        .ToList();
                    gameResultRepository.Put(gameResult);
                }
            }
        }

        public IEnumerable<Player> Get(int? minimumNumberOfGames = null)
        {
            if (minimumNumberOfGames.HasValue && minimumNumberOfGames < 0)
            {
                throw new ArgumentException("Minimum number of games must be 0 or greater", nameof(minimumNumberOfGames));
            }

            using(var playerRepository = repositoryFactory.GetRepository<Player>())
            using(var gameResultRepository = repositoryFactory.GetRepository<GameResult>())
            {
                var players = playerRepository.Get();

                if (minimumNumberOfGames.HasValue && minimumNumberOfGames > 0)
                {
                    var gamesPlayed = gameResultRepository.Get()
                        .SelectMany(gr => gr.PlayerResultsByPosition.SelectMany(prs => prs))
                        .GroupBy(pr => pr.PlayerName)
                        .ToDictionary(g => g.Key, g => g.Count());

                    players = players.Where(p => gamesPlayed.ContainsKey(p.Name) && gamesPlayed[p.Name] >= minimumNumberOfGames);
                }

                return players;
            }
        }
    }
}