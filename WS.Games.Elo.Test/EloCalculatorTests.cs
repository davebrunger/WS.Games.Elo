using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using WS.Games.Elo.Lib.Elo;

namespace WS.Games.Elo.Test
{
    [TestFixture]
    public class EloCalculatorTest
    {
        private class RatingHolder : IImmutableRatingHolder<RatingHolder>
        {
            public int Identifier { get; }
            public int Rating { get; }

            public RatingHolder(int identifier, int rating)
            {
                Identifier = identifier;
                Rating = rating;
            }

            public bool IdentifiesWith(IImmutableRatingHolder<RatingHolder> other)
            {
                return Identifier == other.UpdateRating(other.Rating).Identifier;
            }

            public RatingHolder UpdateRating(int newRating)
            {
                return new RatingHolder(Identifier, newRating);
            }
        }


        [Test]
        public void DoNothingTest()
        {
        }

        [Test]
        [TestCaseSource(nameof(TestGetWinProbabilityTestCaseSource))]
        public void TestGetWinProbability(int playerRating, int opponentRating, double expectedProbabilityToTwoDecimalPlaces)
        {
            var calculator = new EloCalculator(32);
            var actualProbability = calculator.GetWinProbability(playerRating, opponentRating);
            var actualProbabilityToTwoDecimalPlaces = Math.Round(actualProbability, 2);
            Assert.AreEqual(expectedProbabilityToTwoDecimalPlaces, actualProbabilityToTwoDecimalPlaces);
        }

        private static object[] TestGetWinProbabilityTestCaseSource => new object[] {
            new object[] {1000, 1000, 0.5},
            new object[] {2000, 2000, 0.5},
            new object[] {1100, 1000, 0.64},
            new object[] {2100, 2000, 0.64},
            new object[] {1200, 1000, 0.76},
            new object[] {2200, 2000, 0.76},
            new object[] {1000, 1100, 0.36},
            new object[] {2000, 2100, 0.36},
            new object[] {1000, 1200, 0.24},
            new object[] {2000, 2200, 0.24},
            new object[] {1613, 1609, 0.51},
            new object[] {1613, 1477, 0.69},
            new object[] {1613, 1388, 0.79},
            new object[] {1613, 1586, 0.54},
            new object[] {1613, 1720, 0.35},
            new object[] {984, 1000, 0.48},
            new object[] {984, 1016, 0.45},
            new object[] {1000, 1016, 0.48}
        };

        [Test]
        [TestCaseSource(nameof(TestGetNewRatingTestCaseSource))]
        public void TestGetNewRating(int originalRating, double winProbability, Result result, int expectedNewRating)
        {
            var calculator = new EloCalculator(32);
            var actualNewRating = calculator.GetNewRating(originalRating, winProbability, result);
            Assert.AreEqual(expectedNewRating, actualNewRating);
        }

        private static object[] TestGetNewRatingTestCaseSource => new object[] {
            new object[] {1000, 0.5, Result.Loss, 984},
            new object[] {1000, 0.5, Result.Draw, 1000},
            new object[] {1000, 0.5, Result.Win, 1016},
            new object[] {2000, 0.5, Result.Loss, 1984},
            new object[] {2000, 0.5, Result.Draw, 2000},
            new object[] {2000, 0.5, Result.Win, 2016},
            new object[] {1000, 0, Result.Loss, 1000},
            new object[] {1000, 0, Result.Draw, 1016},
            new object[] {1000, 0, Result.Win, 1032},
            new object[] {1000, 1, Result.Loss, 968},
            new object[] {1000, 1, Result.Draw, 984},
            new object[] {1000, 1, Result.Win, 1000},
        };

        [Test]
        [TestCaseSource(nameof(TestUpdateRatingTestCaseSource))]
        public void TestUpdateRating(int playerRating, int opponentRating, Result result, int expectedNewPlayerRating)
        {
            var calculator = new EloCalculator(32);
            var newRatingHolder = calculator.UpdateRating(new RatingHolder(0, playerRating), new RatingHolder(1, opponentRating), result);
            Assert.AreEqual(newRatingHolder.Rating, expectedNewPlayerRating);
        }

        private static object[] TestUpdateRatingTestCaseSource => new object[] {
            new object[] {1000, 1000, Result.Loss, 984},
            new object[] {1500, 1500, Result.Draw, 1500},
            new object[] {2000, 2000, Result.Win, 2016},
            new object[] {1613, 1609, Result.Loss, 1597},
            new object[] {1613, 1477, Result.Draw, 1607},
            new object[] {1613, 1388, Result.Win, 1620}
        };

        [Test]
        [TestCaseSource(nameof(TestUpdateRatingsTestCaseSource))]
        public void TestUpdateRatings(IReadOnlyCollection<IReadOnlyCollection<KeyValuePair<int, int>>> gameResult, Dictionary<int, int> expectedNewRatings)
        {
            var calculator = new EloCalculator(32);
            var gameResultRatingHolders = gameResult
                .Select(rs => rs.Select(r => new RatingHolder(r.Key, r.Value)).ToList())
                .ToList();
            var newRatingHolders = calculator.UpdateRatings(gameResultRatingHolders).ToList();
            Assert.AreEqual(newRatingHolders.Count, expectedNewRatings.Count);
            Assert.AreEqual(newRatingHolders.Count, newRatingHolders.Select(rh => rh.Identifier).Distinct().Count());
            foreach (var rh in newRatingHolders)
            {
                Assert.AreEqual(rh.Rating, expectedNewRatings[rh.Identifier]);
            }
        }

        private static object[] TestUpdateRatingsTestCaseSource => new object[] {
            new object[] {
                new List<List<KeyValuePair<int, int>>>{
                    new List<KeyValuePair<int, int>> {
                        KeyValuePair.Create(0, 1000)
                    },
                    new List<KeyValuePair<int, int>> {
                        KeyValuePair.Create(1, 1000)
                    },
                    new List<KeyValuePair<int, int>> {
                        KeyValuePair.Create(2, 1000)
                    }
                },
                new Dictionary<int, int> {
                    {0, 1016},
                    {1, 1000},
                    {2, 984},
                }
            },
            new object[] {
                new List<List<KeyValuePair<int, int>>>{
                    new List<KeyValuePair<int, int>> {
                        KeyValuePair.Create(3, 984)
                    },
                    new List<KeyValuePair<int, int>> {
                        KeyValuePair.Create(2, 1000)
                    },
                    new List<KeyValuePair<int, int>> {
                        KeyValuePair.Create(1, 1000)
                    },
                    new List<KeyValuePair<int, int>> {
                        KeyValuePair.Create(0, 1016)
                    }
                },
                new Dictionary<int, int> {
                    {0, 999},
                    {1, 995},
                    {2, 1005},
                    {3, 1001}
                }
            }
        };
    }
}
