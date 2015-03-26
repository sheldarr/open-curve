namespace OpenCurve
{
    using System;
    using System.Collections.Generic;
    using Engine;
    using Engine.Bonuses;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Player : IOpenCurveComponent
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _texture;

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
            _spriteBatch = GameServices.GetService<SpriteBatch>();
            _texture = GameServices.GetService<ContentManager>().Load<Texture2D>("player.png");

            PlayerBonuses = new List<IPlayerBonus>();
            PreviousPositions = new List<Vector2>();

            ResetToDefaultValues();
        }

        public void ResetToDefaultValues()
        {
            PreviousPositions.Clear();

            GapDelay = TimeSpan.FromSeconds(3);
            GapTime = TimeSpan.FromSeconds(1.5);
            Gap = true;

            IsAlive = true;

            BasicMoveSpeed = 60.0f;
            BasicRotationSpeed = 8f;
            BasicSize = 4;
        }

        public void Update(GameTime gameTime)
        {
            ApplyBonuses();
            HandleControls(gameTime);
            MakeMove(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            foreach (var previousPosition in PreviousPositions)
            {
                _spriteBatch.Draw(_texture, previousPosition - new Vector2(Size / 2, Size / 2), null, Color, 0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);
            }

            _spriteBatch.Draw(_texture, Position - new Vector2(Size / 2, Size / 2), null, Color, 0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);

            var rightCrossVector = Vector3.Cross(new Vector3(Direction, 0), Vector3.UnitZ);
            var leftCrossVector = Vector3.Cross(new Vector3(Direction, 0), -Vector3.UnitZ);

            var directionPosition = new Vector2(Position.X + Direction.X * Size, Position.Y + Direction.Y * Size);
            var leftPerpendicularDirection = new Vector2(Position.X + rightCrossVector.X * Size, Position.Y + rightCrossVector.Y * Size);
            var rightPerpendicularDirection = new Vector2(Position.X + leftCrossVector.X * Size, Position.Y + leftCrossVector.Y * Size);

            _spriteBatch.Draw(_texture, directionPosition, null, Color.Purple, 0f, new Vector2(0, 0), 0.2f, SpriteEffects.None, 0);
            _spriteBatch.Draw(_texture, leftPerpendicularDirection, null, Color.Purple, 0f, new Vector2(0, 0), 0.2f, SpriteEffects.None, 0);
            _spriteBatch.Draw(_texture, rightPerpendicularDirection, null, Color.Purple, 0f, new Vector2(0, 0), 0.2f, SpriteEffects.None, 0);

            _spriteBatch.End();
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
            if (!IsAlive)
            {
                return;
            }

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
