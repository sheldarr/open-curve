namespace OpenCurve
{
    using System;
    using System.Collections.Generic;
    using Engine;
    using Engine.Bonuses;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

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

            BasicMoveSpeed = 60.0f;
            BasicRotationSpeed = 8f;
            BasicSize = 4;

            GapDelay = TimeSpan.FromSeconds(3);
            GapTime = TimeSpan.FromSeconds(1.5);
            Gap = true;

            IsAlive = true;
        }

        public void Update(GameTime gameTime)
        {
            ApplyBonuses();
            HandleControls(gameTime);
            MakeMove(gameTime);
        }

        public void HandleControls(GameTime gameTime)
        {
            var elapsedSeconds = gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if (PlayerControls.PadController)
            {
                if (GamePad.GetState(PlayerControls.PlayerIndex).DPad.Left == ButtonState.Pressed)
                {
                    TurnLeft(elapsedSeconds);
                }
                if (GamePad.GetState(PlayerControls.PlayerIndex).DPad.Right == ButtonState.Pressed)
                {
                    TurnRight(elapsedSeconds);
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(PlayerControls.MoveLeftKey))
                {
                    TurnLeft(elapsedSeconds);
                }
                if (Keyboard.GetState().IsKeyDown(PlayerControls.MoveRightKey))
                {
                    TurnRight(elapsedSeconds);
                }
            }
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
            var elapsedSeconds = gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (Gap)
            {
                GapTime -= gameTime.ElapsedGameTime;
                if (GapTime < TimeSpan.FromSeconds(0))
                {
                    Gap = false;
                    GapTime = TimeSpan.FromSeconds(0.2);
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

            var positionOffsetX = Direction.X * MoveSpeed * elapsedSeconds;
            var positionOffsetY = Direction.Y * MoveSpeed * elapsedSeconds;

            var positionOffsetVector = new Vector2(positionOffsetX, positionOffsetY);

            if (!Gap)
            {
                PreviousPositions.Add(Position);
            }

            Position += positionOffsetVector;
        }

        public void TurnLeft(float elapsedSeconds)
        {
            Direction = Vector2.Normalize(CalculateNewDirection(-BasicRotationSpeed*elapsedSeconds));
        }

        public void TurnRight(float elapsedSeconds)
        {
            Direction = Vector2.Normalize(CalculateNewDirection(BasicRotationSpeed*elapsedSeconds));
        }

        private Vector2 CalculateNewDirection(float angle)
        {
            var newDirectionX = (float)((Direction.X * Math.Cos(angle)) - (Direction.Y * Math.Sin(angle)));
            var newDirectionY = (float)((Direction.Y * Math.Cos(angle)) + (Direction.X * Math.Sin(angle)));

            return new Vector2(newDirectionX, newDirectionY);
        }
    }
}
