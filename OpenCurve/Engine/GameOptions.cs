namespace OpenCurve.Engine
{
    using System.Collections.Generic;

    public class GameOptions
    {
        public BoardSize BoardSize { get; set; }
        public ICollection<PlayerOptions> PlayerOptions { get; set; }
    }
}
