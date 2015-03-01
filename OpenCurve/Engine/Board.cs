namespace OpenCurve.Engine
{
    using Microsoft.Xna.Framework;

    public class Board
    {
        private const int DefaultWidth = 640;
        private const int DefaultHeight = 480;

        public Vector2 Size { get; set; }

        public Board()
        {
            Size = new Vector2(DefaultWidth, DefaultHeight);
        }


    }
}
