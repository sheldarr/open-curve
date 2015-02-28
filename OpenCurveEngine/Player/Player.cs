namespace OpenCurveEngine.Player
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public class Player
    {
        public Vector2 Position { get; set; }

        public List<Vector2> HistoryOfPositions { get; set; }
            
    }
}
