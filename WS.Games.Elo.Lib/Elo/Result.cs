using System;

namespace WS.Games.Elo.Lib.Elo
{
    public enum Result
    {
        Loss = 0,
        Draw = 1,
        Win = 2
    }

    public static class ResultExtensionMethods
    {
        public static double ToScore(this Result result)
        {
            return (int)result / 2.0;
        }
    }
}
