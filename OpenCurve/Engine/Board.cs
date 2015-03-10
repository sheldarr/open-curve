namespace OpenCurve.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using OpenPrimitives;

    public class Board : IOpenCurveComponent
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly ContentManager _content;
        private readonly BoardSize _boardSize;

        private readonly bool[,] _boardField;
        private Texture2D _playerTexture;

        public List<Player> Players { get; set; }

        public Board(SpriteBatch spriteBatch, ContentManager content, BoardSize boardSize)
        {
            _spriteBatch = spriteBatch;
            _content = content;
            _boardSize = boardSize;

            _boardField = new bool[_boardSize.Width, boardSize.Height];
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            _playerTexture = _content.Load<Texture2D>("player");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            foreach (var player in Players.Where(p => p.IsAlive))
            {
                FillBoard(player.Position, player.Size);

                player.MakeMove();

                if (player.Position.Y <= 0 || player.Position.Y >= _boardSize.Height
                    || player.Position.X <= 0|| player.Position.X  >= _boardSize.Width)
                {
                    player.IsAlive = false;
                }

                CheckCollisions(player);
            }
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            var boardRectangle = new Rectangle(0, 0, _boardSize.Width, _boardSize.Height);
            _spriteBatch.DrawRectangle(boardRectangle, Color.DarkBlue, 4);

            foreach (var player in Players)
            {
                foreach (var previousPosition in player.PreviousPositions)
                {
                    _spriteBatch.Draw(_playerTexture, previousPosition, null, player.Color, 0f, new Vector2(0, 0), 0.5f, SpriteEffects.None, 0);  
                }

                var directionPosition = new Vector2(player.Position.X + player.Direction.X * player.Size * 2, player.Position.Y + player.Direction.Y * player.Size * 2);
                _spriteBatch.Draw(_playerTexture, directionPosition, null, player.Color, 0f, new Vector2(0, 0), 0.2f, SpriteEffects.None, 0);  
            }
            _spriteBatch.End();
        }

        private void FillBoard(Vector2 playerPosition, int playerSize)
        {
            var areaRadius = (int)Math.Round((double)playerSize/2);

            for (var x = (int)playerPosition.X - areaRadius; x < (int)playerPosition.X + areaRadius; x++)
            {
                for (var y = (int)playerPosition.Y - areaRadius; y < (int)playerPosition.Y + areaRadius; y++)
                {
                    if (x > 0 && y > 0 && x < _boardSize.Width && y < _boardSize.Height)
                    {
                        _boardField[x, y] = true;
                    }
                } 
            }
        }

        private void CheckCollisions(Player player)
        {
            var areaRadius = (int)Math.Round((double)player.Size/ 2);
            var directionPosition = new Vector2(player.Position.X + player.Direction.X * player.Size * 2, player.Position.Y + player.Direction.Y * player.Size * 2);

            for (var x = (int)directionPosition.X - areaRadius; x < (int)directionPosition.X + areaRadius; x++)
            {
                for (var y = (int)directionPosition.Y - areaRadius; y < (int)directionPosition.Y + areaRadius; y++)
                {
                    if (x > 0 && y > 0 && x < _boardSize.Width && y < _boardSize.Height)
                    {
                        if (_boardField[x, y])
                        {
                            player.IsAlive = false;
                        }
                    }
                } 
            }
        }
    }
}
