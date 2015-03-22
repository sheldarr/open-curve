namespace OpenCurve.Engine.Extensions
{
    using System;
    using Microsoft.Xna.Framework;

    public static class PlayerExtensions
    {
        private static readonly Random Random = new Random();

        public static void RandomizePosition(this Player player, BoardSize boardSize)
        {
            var newRandomPosition = new Vector2(Random.Next(0, boardSize.Width), Random.Next(0, boardSize.Height));
            
            player.Position = newRandomPosition;
        }

        public static void RandomizeDirection(this Player player)
        {
            var randomDirection = new Vector2((float)(Random.NextDouble() * 2 - 1), (float)(Random.NextDouble() * 2 - 1));
            randomDirection.Normalize();
            
            player.Direction = randomDirection;
        }
    }
}