namespace OpenCurve.Engine
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using OpenPrimitives;

    public class Board : IBoard
    {
        public SpriteBatch SpriteBatch { get; set; }
        public Vector2 Size { get; set; }
        public List<Player> Players { get; set; }

        public void Draw()
        {
            SpriteBatch.Begin();

            var boardRectangle = new Rectangle(100, 40, (int) Size.X, (int) Size.Y);
            SpriteBatch.DrawRectangle(boardRectangle, Color.DarkBlue, 4);

            foreach (var player in Players)
            {
                SpriteBatch.DrawCircle(player.Position.X, player.Position.Y, player.Size, 8, player.Color, 2);

                var directionPosition = new Vector2(player.Position.X + player.Direction.X * player.Size / 2, player.Position.Y + player.Direction.Y * player.Size / 2);
                SpriteBatch.DrawCircle(directionPosition.X, directionPosition.Y, 3, 8, Color.Purple, 2);
            }

            SpriteBatch.End();
        }
    }
}
