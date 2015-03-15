namespace OpenCurve
{
    using System;
    using System.Collections.Generic;
    using Engine;
    using Engine.Bonuses;
    using Microsoft.Xna.Framework;

    public class Player
    {
        public Color Color { get; set; }

        public IList<Vector2> PreviousPositions { get; set; } 
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }

        public float BasicMoveSpeed { get; set; }
        public float BasicRotationSpeed { get; set; }
        public int BasicSize { get; set; }

        public float MoveSpeed { get; set; }
        public float RotationSpeed { get; set; }
        public int Size { get; set; }

        public int Points { get; set; }

        public bool IsAlive { get; set; }

        public List<IPlayerBonus> PlayerBonuses { get; set; }

        public PlayerControls PlayerControls { get; set; }

        public TimeSpan GapDelay { get; set; }
        public TimeSpan GapTime { get; set; }
        public Boolean Gap { get; set; }

        public Player()
        {
            Color = Color.Red;

            PlayerBonuses = new List<IPlayerBonus>();
            PreviousPositions = new List<Vector2>();
            Position = new Vector2(0, 0);
            Direction = new Vector2(1, 1);

            BasicMoveSpeed = 2.0f;
            BasicRotationSpeed = 0.1f;
            BasicSize = 4;

            GapDelay = TimeSpan.FromSeconds(3);
            GapTime = TimeSpan.FromSeconds(1.5);
            Gap = true;

            IsAlive = true;
        }

        public void ApplyBonuses()
        {
            MoveSpeed = BasicMoveSpeed;
            RotationSpeed = BasicRotationSpeed;
            Size = BasicSize;

            PlayerBonuses.ForEach(pb => pb.Apply(this));
        }

        public void MakeMove(GameTime gameTime)
        {
            if (Gap)
            {
                GapTime -= gameTime.ElapsedGameTime;
                if (GapTime < TimeSpan.FromSeconds(0))
                {
                    Gap = false;
                    GapTime = TimeSpan.FromSeconds(0.35);
                }
            }
            else
            {
                GapDelay -= gameTime.ElapsedGameTime;
                if (GapDelay < TimeSpan.FromSeconds(0))
                {
                    Gap = true;
                    GapDelay = TimeSpan.FromSeconds(3);
                }
            }

            var positionOffsetX = Direction.X*MoveSpeed;
            var positionOffsetY = Direction.Y*MoveSpeed;

            var positionOffsetVector = new Vector2(positionOffsetX, positionOffsetY);

            if (!Gap)
            {
                PreviousPositions.Add(Position);
            }

            Position += positionOffsetVector;
        }

        public void TurnLeft()
        {
            Direction = Vector2.Normalize(CalculateNewDirection(-BasicRotationSpeed));
        }

        public void TurnRight()
        {
            Direction = Vector2.Normalize(CalculateNewDirection(BasicRotationSpeed));
        }

        private Vector2 CalculateNewDirection(float angle)
        {
            var newDirectionX = (float)((Direction.X * Math.Cos(angle)) - (Direction.Y * Math.Sin(angle)));
            var newDirectionY = (float)((Direction.Y * Math.Cos(angle)) + (Direction.X * Math.Sin(angle)));

            return new Vector2(newDirectionX, newDirectionY);
        }
    }
}
