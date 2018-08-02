using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WS.Games.Elo.Console.Commands;
using WS.Games.Elo.Lib.Elo;
using WS.Games.Elo.Lib.Repositories;
using WS.Games.Elo.Lib.Services;
using WS.Utilities.Console;
using WS.Utilities.Injection;
using static System.Console;

namespace WS.Games.Elo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var container = ConfigureContainer();
                var comamnds = container.Resolve<IReadOnlyCollection<ICommand>>();
                var action = GetAction(comamnds, args);
                if (action == null)
                {
                    return;
                }
                action();
            }
            catch (Exception e)
            {
                PrintError(e.ToString());
            }
            finally
            {
                if (Debugger.IsAttached)
                {
                    WriteLine();
                    WriteLine("Done");
                    ReadLine();
                }
            }
        }

        private static BasicInjectionContainer ConfigureContainer()
        {
            var container = new BasicInjectionContainer();

            // Infrastructure Configuration
            container.RegisterType<IOutputWriter, ConsoleOutputWriter>(); 

            // WS.Games Configuration
            var baseDirectory = "/Users/davidbrunger/Documents/Visual Studio Code Projects/WS.Games.Elo/Data";

            container.RegisterType<PlayerService>();
            container.RegisterInstance<IRepositoryFactory>(new JsonRepositoryFactory(baseDirectory));
            container.RegisterType<IPlayerServiceConfiguration, Configuration>();
            container.RegisterType<GameService>();
            container.RegisterInstance(new EloCalculator(32));
            container.RegisterType<IGameServiceConfiguration, Configuration>();
            container.RegisterType<ImageService>();

            // Command Line Commands
            var commands = new List<ICommand>();
            var commandTypes = typeof(Program).GetTypeInfo().Assembly.DefinedTypes
                .Where(t => (typeof(ICommand)).GetTypeInfo().IsAssignableFrom(t))
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => t.AsType())
                .ToList();
            foreach (var commandType in commandTypes)
            {
                container.RegisterType(commandType, commandType);
                var command = container.Resolve(commandType) as ICommand;
                commands.Add(command);
            }

            container.RegisterInstance<IReadOnlyCollection<ICommand>>(commands.AsReadOnly());

            return container;
        }

        private static Action GetAction(IReadOnlyCollection<ICommand> commands, string[] args)
        {
            var commandLookup = commands.ToDictionary(c => c.Name, c => c, StringComparer.OrdinalIgnoreCase);
            if (args.Length < 1 || !commandLookup.ContainsKey(args[0]))
            {
                PrintUsage(commands);
                return null;
            }
            return commandLookup[args[0]].GetAction(args.Skip(1));
        }

        private static void PrintError(string message)
        {
            var foregroundColour = ForegroundColor;
            ForegroundColor = ConsoleColor.Red;
            WriteLine(message);
            ForegroundColor = foregroundColour;
        }

        private static void PrintUsage(IReadOnlyCollection<ICommand> commands)
        {
            PrintError("Usage:");
            PrintError("dotnet run -- <command> [<command-options>]");
            PrintError("Available Commands:");
            foreach (var command in commands.OrderBy(c => c.Name))
            {
                PrintError(command.Name);
            }
        }
    }
}
