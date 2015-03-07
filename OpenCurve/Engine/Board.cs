namespace OpenCurve.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using OpenPrimitives;

    public class Board : IGameComponent
    {
        private readonly SpriteBatch _spriteBatch;
        private Vector2 _size;

        private bool[,] _boardField;
        private IEnumerable<Texture2D> _playerTextures; 

        public List<Player> Players { get; set; }

        public Board(SpriteBatch spriteBatch, Vector2 size)
        {
            _spriteBatch = spriteBatch;
            _size = size;

            _boardField = new bool[(int)size.X, (int)size.Y];
            _playerTextures = new List<Texture2D>();
            //var texture2d = new Texture2D(spriteBatch.GraphicsDevice, (int)size.X, (int)size.Y);
            //texture2d.SetData(new [] { Color.White });
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
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

                if (player.Position.Y <= 0 || player.Position.Y >= _size.Y
                    || player.Position.X <= 0|| player.Position.X  >= _size.X)
                {
                    player.IsAlive = false;
                }

                CheckCollisions(player);

            }
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            var boardRectangle = new Rectangle(100, 40, (int) _size.X, (int) _size.Y);
            _spriteBatch.DrawRectangle(boardRectangle, Color.DarkBlue, 4);

            foreach (var player in Players)
            {
                _spriteBatch.DrawCircle(player.Position.X + 100, player.Position.Y + 40, player.Size, 8, player.Color, 4);

                var directionPosition = new Vector2(player.Position.X + player.Direction.X * player.Size*2, player.Position.Y + player.Direction.Y * player.Size*2);
                _spriteBatch.DrawCircle(directionPosition.X + 100, directionPosition.Y + 40, 1, 8, Color.Purple, 2);
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
                    if (x > 0 && y > 0 && x < _size.X && y < _size.Y)
                    {
                        _boardField[x, y] = true;
                    }
                } 
            }
        }

        private void CheckCollisions(Player player)
        {
            var areaRadius = (int)Math.Round((double)player.Size/ 2);
            var directionPosition = new Vector2(player.Position.X + player.Direction.X * player.Size*2, player.Position.Y + player.Direction.Y * player.Size*2);

            for (var x = (int)directionPosition.X - areaRadius; x < (int)directionPosition.X + areaRadius; x++)
            {
                for (var y = (int)directionPosition.Y - areaRadius; y < (int)directionPosition.Y + areaRadius; y++)
                {
                    if (x > 0 && y > 0 && x < _size.X && y < _size.Y)
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
