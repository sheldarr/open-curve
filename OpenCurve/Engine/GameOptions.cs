namespace OpenCurve.Engine
{
    using System.Collections.Generic;

    public class GameOptions
    {
        public bool ExitGame { get; set; }
        public bool Fullscreen { get; set; }
        public int RoundLimit { get; set; }
        public BoardSize BoardSize { get; set; }
        public ICollection<PlayerOptions> PlayerOptions { get; set; }
    }
}
