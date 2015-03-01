namespace OpenCurve
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public class Player
    {
        public Color Color { get; set; }

        public Vector2 Position { get; set; }
        public List<Vector2> PreviousPositions { get; set; }
        public Vector2 Direction { get; set; }

        public float MoveSpeed { get; set; }
        public float RotationSpeed { get; set; }
        public int Size { get; set; }

        public Player()
        {
            Color = Color.Red;

            Position = new Vector2(0, 0);
            PreviousPositions = new List<Vector2>();
            Direction = new Vector2(1, 1);

            MoveSpeed = 1.0f;
            RotationSpeed = 0.1f;
            Size = 5;
        }

        public void MakeMove()
        {
            var positionOffsetX = Direction.X*MoveSpeed;
            var positionOffsetY = Direction.Y*MoveSpeed;

            var positionOffsetVector = new Vector2(positionOffsetX, positionOffsetY);

            PreviousPositions.Add(Position);
            Position += positionOffsetVector;
        }

        public void TurnLeft()
        {
            Direction = CalculateNewDirection(-RotationSpeed);
        }

        public void TurnRight()
        {
            Direction = CalculateNewDirection(RotationSpeed);
        }

        private Vector2 CalculateNewDirection(float angle)
        {
            var newDirectionX = (float)((Direction.X * Math.Cos(angle)) - (Direction.Y * Math.Sin(angle)));
            var newDirectionY = (float)((Direction.Y * Math.Cos(angle)) + (Direction.X * Math.Sin(angle)));

            return new Vector2(newDirectionX, newDirectionY);
        }
    }
}
