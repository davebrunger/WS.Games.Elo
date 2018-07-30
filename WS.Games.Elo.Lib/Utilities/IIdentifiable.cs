using System;

namespace WS.Games.Elo.Lib.Utilities
{
    public interface IIdentifiable<T>
    {
        bool IdentifiesWith(T other);
    }
}