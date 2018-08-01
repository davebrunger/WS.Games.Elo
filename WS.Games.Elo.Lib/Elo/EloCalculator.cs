using System;
using System.Collections.Generic;
using System.Linq;

namespace WS.Games.Elo.Lib.Elo
{
    public class EloCalculator
    {
        public int KFactor { get; }

        public EloCalculator(int kFactor)
        {
            KFactor = kFactor;
        }

        public double GetWinProbability(int rating, int opponentRating)
        {
            return 1 / (1 + Math.Pow(10, (opponentRating - rating) / 400.0));
        }

        public int GetNewRating(int originalRating, double winProbability, Result result)
        {
            return GetNewRating(originalRating, winProbability, result.ToScore());
        }

        public T UpdateRating<T>(IImmutableRatingHolder<T> ratingHolder, IRatingHolder opponent, Result result)
            where T : IImmutableRatingHolder<T>
        {
            var winProbability = GetWinProbability(ratingHolder.Rating, opponent.Rating);
            var newRating = GetNewRating(ratingHolder.Rating, winProbability, result);
            return ratingHolder.UpdateRating(newRating);
        }

        public IEnumerable<T> UpdateRatings<T>(IReadOnlyCollection<IReadOnlyCollection<IImmutableRatingHolder<T>>> ratingHoldersFinishingPosition)
            where T : IImmutableRatingHolder<T>
        {
            var players = ratingHoldersFinishingPosition.Sum(rhs => rhs.Count());
            if (players < 2)
            {
                throw new ArgumentException($"There must be at least 2 players involved in a ratings swap. Number of players specified: {players}", nameof(ratingHoldersFinishingPosition));
            }
            var playerCountAdjustment = 1.0 / (players - 1);

            return ratingHoldersFinishingPosition.SelectMany((rhl, p) => rhl.Select(rh =>
            {
                var originalRating = rh.Rating;

                var winProbabilitiesAgainstWinners = ratingHoldersFinishingPosition
                    .Take(p)
                    .SelectMany(ol => ol.Select(o => new { WinProbability = GetWinProbability(originalRating, o.Rating), Result = Result.Loss }));
                var winProbabilitiesAgainstTies = ratingHoldersFinishingPosition
                    .Skip(p)
                    .First()
                    .Where(o => !rh.IdentifiesWith(o))
                    .Select(o => new { WinProbability = GetWinProbability(originalRating, o.Rating), Result = Result.Draw });
                var winProbabilitiesAgainstLosers = ratingHoldersFinishingPosition
                    .Skip(p + 1)
                    .SelectMany(ol => ol.Select(o => new { WinProbability = GetWinProbability(originalRating, o.Rating), Result = Result.Win }));

                var totalWinProbability = winProbabilitiesAgainstWinners
                    .Concat(winProbabilitiesAgainstTies)
                    .Concat(winProbabilitiesAgainstLosers)
                    .Aggregate(new { WinProbability = 0.0, Result = 0.0 }, (rha, wp) => new { WinProbability = rha.WinProbability + wp.WinProbability, Result = rha.Result + wp.Result.ToScore() });

                return rh.UpdateRating(GetNewRating(originalRating,
                    totalWinProbability.WinProbability * playerCountAdjustment,
                    totalWinProbability.Result * playerCountAdjustment));
            }));
        }

        private int PointsExchanged(double winProbability, Result result)
        {
            return PointsExchanged(winProbability, result.ToScore());
        }

        private int PointsExchanged(double winProbability, double result)
        {
            return (int)Math.Round(KFactor * (result - winProbability));
        }

        private int GetNewRating(int originalRating, double winProbability, double result)
        {
            return originalRating + PointsExchanged(winProbability, result);
        }
    }
}
