namespace OpenCurve.Engine
{
    using System.Collections.Generic;
    using Bonuses;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Gameplay : IGameComponent
    {
        public ContentManager Content { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }

        public List<Player> Players { get; private set; }
        public Board Board { get; set; }

        private SpriteBatch SpriteBatch { get; set; }
        private FpsCounter FpsCounter { get; set; }

        public OnExit Exit;

        public Gameplay(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Content = content;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            Players = new List<Player>();
            Board = new Board(SpriteBatch, new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth - 160, GraphicsDevice.PresentationParameters.BackBufferHeight - 80))
            {
                Players = Players
            };
        }

        public void Initialize()
        {
            FpsCounter = new FpsCounter(Content, SpriteBatch);

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
                Position = new Vector2(500, 300),
                PlayerBonuses = new List<IPlayerBonus>()
            };

            Players.Add(player2);
        }

        public void LoadContent()
        {
            FpsCounter.LoadContent();
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

            Board.Update(gameTime);
            FpsCounter.Update(gameTime);

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

                //var distance = Math.Sqrt(Math.Pow(p.X - player.Position.X, 2) + Math.Pow(p.Y - player.Position.Y, 2));

                player.ApplyBonuses();
            }
        }

        public void Draw(GameTime gameTime)
        {
            Board.Draw(gameTime);
            FpsCounter.Draw(gameTime);
        }
    }
}
