namespace OpenCurve.Engine
{
    using System.Collections.Generic;

    public class GameOptions
    {
        public GameType GameType { get; set; }
        public int MaxPlayers { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
