using System.IO;

namespace WS.Games.Elo.Lib.Utilities
{
    public class FileWrapper : IFileWrapper
    {
        public string FileName { get; }

        public bool FileExists => File.Exists(FileName);

        public FileWrapper(string fileName)
        {
            this.FileName = fileName;

        }
        public string ReadAllText()
        {
            return File.ReadAllText(FileName);
        }

        public void WriteAllText(string data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FileName));
            File.WriteAllText(FileName, data);
        }
    }
}