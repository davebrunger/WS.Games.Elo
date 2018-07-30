using System;
using System.Collections.Generic;

namespace WS.Games.Elo.Console.Commands
{
    public interface ICommand
    {
        string Name { get; }

        Action GetAction(IEnumerable<string> options);
    }
}