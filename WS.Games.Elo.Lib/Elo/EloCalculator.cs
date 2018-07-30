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
            return GetNewRating(originalRating, KFactor, winProbability, result);
        }

        private int PointsExchanged(double winProbability, Result result)
        {
            return PointsExchanged(KFactor, winProbability, result);
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
            var multiplayerKFactor = (double)KFactor / (ratingHoldersFinishingPosition.Sum(rhl => rhl.Count()) - 1);

            return ratingHoldersFinishingPosition.SelectMany((rhl, p) => rhl.Select(rh =>
            {
                var originalRating = rh.Rating;
                
                var winProbabilitiesAgainstWinners = ratingHoldersFinishingPosition
                    .Take(p)
                    .SelectMany(ol => ol.Select(o => new {WinProbability = GetWinProbability(originalRating, o.Rating), Result = Result.Loss}));
                var winProbabilitiesAgainstTies = ratingHoldersFinishingPosition
                    .Skip(p)
                    .First()
                    .Where(o => !rh.IdentifiesWith(o))
                    .Select(o => new {WinProbability = GetWinProbability(originalRating, o.Rating), Result = Result.Draw});
                var winProbabilitiesAgainstLosers = ratingHoldersFinishingPosition
                    .Skip(p + 1)
                    .SelectMany(ol => ol.Select(o => new {WinProbability = GetWinProbability(originalRating, o.Rating), Result = Result.Win}));
                
                return winProbabilitiesAgainstWinners
                    .Concat(winProbabilitiesAgainstTies)
                    .Concat(winProbabilitiesAgainstLosers)
                    .Aggregate(rh.UpdateRating(originalRating), (rha, wp) => rha.UpdateRating(GetNewRating(rha.Rating, multiplayerKFactor, wp.WinProbability, wp.Result)));
            }));
        }

        private static int PointsExchanged(double kFactor, double winProbability, Result result)
        {
            return (int)Math.Round(kFactor * (result.ToScore() - winProbability));
        }

        private static int GetNewRating(int originalRating, double kFactor, double winProbability, Result result)
        {
            return originalRating + PointsExchanged(kFactor, winProbability, result);
        }
    }
}
