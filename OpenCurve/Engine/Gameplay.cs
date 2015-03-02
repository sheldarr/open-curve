namespace OpenCurve.Engine
{
    using System;
    using System.Collections.Generic;
    using Bonuses;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Gameplay : IGameModule
    {
        public ContentManager Content { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }

        public List<Player> Players { get; private set; }

        private SpriteBatch SpriteBatch { get; set; }

        public OnExit Exit;

        public Gameplay(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Content = content;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            Players = new List<Player>();
        }

        public void Initialize()
        {
            var player = new Player
            {
                Color = Color.Yellow,
                Position = new Vector2(100, 100),
                PlayerBonuses = new List<IPlayerBonus>
                {
                    new SpeedPlayerBonus()
                }
            };

            Players.Add(player);

            var player2 = new Player
            {
                Color = Color.Red,
                Position = new Vector2(400, 400),
                PlayerBonuses = new List<IPlayerBonus>()
            };

            Players.Add(player2);
        }

        public void LoadContent()
        {
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            Players.ForEach(p => p.MakeMove());

            foreach (var player in Players)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    player.TurnLeft();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    player.TurnRight();
                }

                player.ApplyBonuses();
            }
        }

        public void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            foreach (var player in Players)
            {
                var playerTexture = TextureFactory.CreateCircleTexture(GraphicsDevice, player.BasicSize);

                foreach (var position in player.PreviousPositions)
                {
                    SpriteBatch.Draw(playerTexture, position, player.Color);
                }
            }

            SpriteBatch.End();

        }
    }
}
