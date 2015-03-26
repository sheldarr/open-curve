namespace OpenCurve.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Board : IOpenCurveComponent
    {
        public  List<Player> Players { get; set; }

        private BoardSize _boardSize;

        private bool[,] _boardField;

        private readonly SpriteBatch _spriteBatch;
        private readonly FpsCounter _fpsCounter;

        public OnExit Exit;
        private readonly SpriteFont _gameSpriteFont;

        private int ActualRound { get; set; }
        private int RoundLimit { get; set; }

        public Board()
        {
            _spriteBatch = GameServices.GetService<SpriteBatch>();
            
            Players = new List<Player>();
            _fpsCounter = new FpsCounter();

            _gameSpriteFont = GameServices.GetService<ContentManager>().Load<SpriteFont>("MainMenuFont");
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                Exit();
            }

            _fpsCounter.Update(gameTime);
            Players.ForEach(p => p.Update(gameTime));

            foreach (var player in Players.Where(p => p.IsAlive))
            {
                if (!player.Gap)
                {
                    FillBoard(player.Position, player.Size);
                }

                player.MakeMove(gameTime);

                CheckCollisions(player);
            }

            if (Players.Count(p => p.IsAlive) < 2)
            {
                NextRound();
            }
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.GraphicsDevice.Clear(Color.Black);

            Players.ForEach(p => p.Draw(gameTime));

            DrawPlayerPoints();
            DrawRoundCounter();
            _fpsCounter.Draw(gameTime);
        }

        public void DrawPlayerPoints()
        {
            _spriteBatch.Begin();

            var pointsPosition = new Vector2(160, 0);
            var orderedPlayers = Players.OrderByDescending(p => p.Points);

            foreach (var player in orderedPlayers)
            {
                _spriteBatch.DrawString(_gameSpriteFont, player.Points.ToString(), pointsPosition, player.Color);
                pointsPosition += new Vector2(30, 0);
            }

            _spriteBatch.End();
        }

        public void DrawRoundCounter()
        {
            _spriteBatch.Begin();

            var roundPosition = new Vector2(4, 0);
            var round = String.Format("Round {0} / {1}", ActualRound, RoundLimit);

            _spriteBatch.DrawString(_gameSpriteFont, round, roundPosition, Color.White);

            _spriteBatch.End();
        }


        public void Reset(GameOptions gameOptions)
        {
            ActualRound = 1;
            Players.Clear();

            RoundLimit = gameOptions.RoundLimit;
            _boardSize = gameOptions.BoardSize;

            _boardField = new bool[_boardSize.Width, _boardSize.Height];

            foreach (var playerOptions in gameOptions.PlayerOptions)
            {
                Players.Add(PlayerFactory.CreatePlayer(playerOptions));
            }

            Players.ForEach(player => player.ResetToDefaultValues());
            Players.ForEach(player => player.RandomizePosition(_boardSize));
            Players.ForEach(player => player.RandomizeDirection());
        }

        private void FillBoard(Vector2 playerPosition, int _playersize)
        {
            var halfOf_playersize = _playersize/2f;
            var areaRadius = (int)Math.Ceiling(halfOf_playersize);

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
            if (CheckCollisionsWithBoard(player) || CheckCollisionWithOther_players(player))
            {
                player.IsAlive = false;

                foreach (var otherAlive_players in Players.Where(p => p != player && p.IsAlive))
                {
                    otherAlive_players.Points++;
                }
            }
        }

        private bool CheckCollisionsWithBoard(Player player)
        {
            return player.Position.Y <= 0 
                || player.Position.X <= 0 
                || player.Position.Y >= _boardSize.Height
                || player.Position.X >= _boardSize.Width;
        }

        private bool CheckCollisionWithOther_players(Player player)
        {
            var areaRadius = (int)Math.Round((double)player.Size / 2);
            var directionPosition = new Vector2(player.Position.X + player.Direction.X * player.Size * 2,
                player.Position.Y + player.Direction.Y * player.Size * 2);

            for (var x = (int)directionPosition.X - areaRadius; x < (int)directionPosition.X + areaRadius; x++)
            {
                for (var y = (int)directionPosition.Y - areaRadius; y < (int)directionPosition.Y + areaRadius; y++)
                {
                    if (x <= 0 || y <= 0 || x >= _boardSize.Width || y >= _boardSize.Height) continue;

                    if (_boardField[x, y])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void NextRound()
        {
            ActualRound++;

            if (ActualRound > RoundLimit)
            {
                Exit();
            }

            Players.ForEach(player => player.ResetToDefaultValues());
            Players.ForEach(player => player.RandomizePosition(_boardSize));
            Players.ForEach(player => player.RandomizeDirection());

            _boardField = new bool[_boardSize.Width, _boardSize.Height];
        }
    }
}
