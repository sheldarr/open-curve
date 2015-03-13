namespace OpenCurve.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Board : IOpenCurveComponent
    {
        public ContentManager Content { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }

        public List<Player> Players { get; private set; }
        public BoardSize BoardSize;

        private bool[,] _boardField;
        private Texture2D _playerTexture;

        private SpriteBatch SpriteBatch { get; set; }
        private FpsCounter FpsCounter { get; set; }

        private Random Random { get; set; }

        public OnExit Exit;

        public Board(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Content = content;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            Players = new List<Player>();
        }

        public void Initialize()
        {
            FpsCounter = new FpsCounter(Content, SpriteBatch);
            Random = new Random();
        }

        public void LoadContent()
        {
            _playerTexture = Content.Load<Texture2D>("player");
            FpsCounter.LoadContent();
        }

        public void UnloadContent()
        {
            FpsCounter.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            FpsCounter.Update(gameTime);

            foreach (var player in Players)
            {
                if (Keyboard.GetState().IsKeyDown(player.MoveLeftKey))
                {
                    player.TurnLeft();
                }
                if (Keyboard.GetState().IsKeyDown(player.MoveRightKey))
                {
                    player.TurnRight();
                }

                player.ApplyBonuses();
            }

            foreach (var player in Players.Where(p => p.IsAlive))
            {
                FillBoard(player.Position, player.Size);

                player.MakeMove();

                if (player.Position.Y <= 0 || player.Position.Y >= BoardSize.Height
                    || player.Position.X <= 0 || player.Position.X >= BoardSize.Width)
                {
                    player.IsAlive = false;
                }

                CheckCollisions(player);
            }
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch.GraphicsDevice.Clear(Color.Black);

            FpsCounter.Draw(gameTime);

            SpriteBatch.Begin();

            foreach (var player in Players)
            {
                foreach (var previousPosition in player.PreviousPositions)
                {
                    SpriteBatch.Draw(_playerTexture, previousPosition - new Vector2(player.Size / 2, player.Size / 2), null, player.Color, 0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);
                }

                var rightCrossVector = Vector3.Cross(new Vector3(player.Direction, 0), Vector3.UnitZ);
                var leftCrossVector = Vector3.Cross(new Vector3(player.Direction, 0), -Vector3.UnitZ);

                var directionPosition = new Vector2(player.Position.X + player.Direction.X * player.Size, player.Position.Y + player.Direction.Y * player.Size);
                var leftPerpendicularDirection = new Vector2(player.Position.X + rightCrossVector.X * player.Size, player.Position.Y + rightCrossVector.Y * player.Size);
                var rightPerpendicularDirection = new Vector2(player.Position.X + leftCrossVector.X * player.Size, player.Position.Y + leftCrossVector.Y * player.Size);

                SpriteBatch.Draw(_playerTexture, directionPosition, null, Color.Purple, 0f, new Vector2(0, 0), 0.2f, SpriteEffects.None, 0);
                SpriteBatch.Draw(_playerTexture, leftPerpendicularDirection, null, Color.Purple, 0f, new Vector2(0, 0), 0.2f, SpriteEffects.None, 0);
                SpriteBatch.Draw(_playerTexture, rightPerpendicularDirection, null, Color.Purple, 0f, new Vector2(0, 0), 0.2f, SpriteEffects.None, 0);
            }
            SpriteBatch.End();
        }

        public void Reset(GameOptions gameOptions)
        {
            BoardSize = gameOptions.BoardSize;
            Players.Clear();

            _boardField = new bool[BoardSize.Width, BoardSize.Height];

            foreach (var playerOptions in gameOptions.PlayerOptions)
            {
                Players.Add(PlayerFactory.CreatePlayer(playerOptions));
            }

            RandomizePlayersPositions();
            RandomizePlayersDirection();
        }

        private void RandomizePlayersPositions()
        {
            foreach (var player in Players)
            {
                var randomPosition = new Vector2(Random.Next(0, BoardSize.Width), Random.Next(0, BoardSize.Height));
                player.Position = randomPosition;
            }
        }

        private void RandomizePlayersDirection()
        {
            foreach (var player in Players)
            {
                var randomDirection = new Vector2((float)(Random.NextDouble() * 2 - 1), (float)(Random.NextDouble() * 2 - 1));
                randomDirection.Normalize();
                player.Direction = randomDirection;
            }
        }

        private void FillBoard(Vector2 playerPosition, int playerSize)
        {
            var areaRadius = (int)Math.Round((double)playerSize / 2);

            for (var x = (int)playerPosition.X - areaRadius; x < (int)playerPosition.X + areaRadius; x++)
            {
                for (var y = (int)playerPosition.Y - areaRadius; y < (int)playerPosition.Y + areaRadius; y++)
                {
                    if (x > 0 && y > 0 && x < BoardSize.Width && y < BoardSize.Height)
                    {
                        _boardField[x, y] = true;
                    }
                }
            }
        }

        private void CheckCollisions(Player player)
        {
            var areaRadius = (int)Math.Round((double)player.Size / 2);
            var directionPosition = new Vector2(player.Position.X + player.Direction.X * player.Size * 2, player.Position.Y + player.Direction.Y * player.Size * 2);

            for (var x = (int)directionPosition.X - areaRadius; x < (int)directionPosition.X + areaRadius; x++)
            {
                for (var y = (int)directionPosition.Y - areaRadius; y < (int)directionPosition.Y + areaRadius; y++)
                {
                    if (x <= 0 || y <= 0 || x >= BoardSize.Width || y >= BoardSize.Height) continue;

                    if (_boardField[x, y])
                    {
                        player.IsAlive = false;
                    }
                }
            }
        }
    }
}
