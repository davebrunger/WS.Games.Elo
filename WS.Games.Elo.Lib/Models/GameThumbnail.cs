using System;
using WS.Games.Elo.Lib.Utilities;

namespace WS.Games.Elo.Lib.Models
{
    public class GameThumbnail: IIdentifiable<GameThumbnail>
    {
        public string GameName { get; set; }

        public string ThumbnailBase64Data { get; set; }

        public void SetThumbnailBytes(byte[] data)
        {
            ThumbnailBase64Data = Convert.ToBase64String(data);
        }

        public byte[] GetThumbnailBytes()
        {
            if (string.IsNullOrEmpty(ThumbnailBase64Data))
            {
                return null;
            }
            return Convert.FromBase64String(ThumbnailBase64Data);
        }

        public bool IdentifiesWith(GameThumbnail other)
        {
            return GameName == other.GameName;
        }
    }
}