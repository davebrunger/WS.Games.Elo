namespace WS.Games.Elo.Lib.Utilities
{
    public interface IFileWrapper
    {
        string FileName { get; }
        bool FileExists { get; }
        void WriteAllText(string data);
        string ReadAllText();
    }
}