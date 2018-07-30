using System;
using WS.Games.Elo.Lib.Models;
using WS.Games.Elo.Lib.Utilities;

namespace WS.Games.Elo.Lib.Elo
{
    public interface IImmutableRatingHolder<T> : IRatingHolder, IIdentifiable<IImmutableRatingHolder<T>>
        where T : IImmutableRatingHolder<T>
    {
         T UpdateRating(int newRating);
    }
}