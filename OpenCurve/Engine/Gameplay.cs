namespace OpenCurve.Engine
{
    using System.Collections.Generic;
    using Bonuses;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Gameplay : IOpenCurveComponent
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

            var boardSize = new BoardSize
            {
                Width = GraphicsDevice.PresentationParameters.BackBufferWidth,
                Height = GraphicsDevice.PresentationParameters.BackBufferHeight
            };

            Board = new Board(SpriteBatch, content, boardSize)
            {
                Players = Players
            };
        }

        public void Initialize()
        {
            Board.Initialize();
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
            Board.LoadContent();
            FpsCounter.LoadContent();
        }

        public void UnloadContent()
        {
            Board.UnloadContent();
            FpsCounter.UnloadContent();
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
