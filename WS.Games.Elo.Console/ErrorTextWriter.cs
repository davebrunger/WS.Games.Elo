using System.IO;
using System.Text;
using NDesk.Options;
using WS.Utilities.Console;

namespace WS.Games.Elo.Console
{
    public class ErrorTextWriter : TextWriter
    {
        private string line = "";

        private readonly IOutputWriter outputWriter;

        public override Encoding Encoding => Encoding.Default;

        public ErrorTextWriter(IOutputWriter outputWriter)
        {
            this.outputWriter = outputWriter;
        }

        public override void Write(char value)
        {
            line = line + value;
        }

        public override void WriteLine()
        {
            outputWriter.WriteErrorLine(line);
            line = "";
        }

        public override void WriteLine(string line)
        {
            outputWriter.WriteErrorLine(this.line + line);
            this.line = "";
        }

        public void WriteUsage(string commandName, OptionSet optionSet, string extraParameters = null)
        {
            outputWriter.WriteErrorLine("Usage:");
            outputWriter.WriteErrorLine($"dotnet run -- {commandName} <options> {extraParameters}");
            optionSet.WriteOptionDescriptions(new ErrorTextWriter(outputWriter));
        }
    }
}