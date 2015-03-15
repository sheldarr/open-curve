namespace OpenCurve.Engine
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Score : IOpenCurveComponent
    {
        private SpriteFont _scoreSpriteFont;

        private ContentManager Content { get; set; }
        private GraphicsDevice GraphicsDevice { get; set; }
        private SpriteBatch SpriteBatch { get; set; }

        public OnExit Exit;

        public IEnumerable<PlayerScore> PlayersScore { get; set; }

        public Score(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Content = content;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            _scoreSpriteFont = Content.Load<SpriteFont>("MainMenuFont");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                Exit();
            }
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();

            var position = new Vector2(GraphicsDevice.Viewport.Width / 2 - 160, 80);

            SpriteBatch.DrawString(_scoreSpriteFont, "Final Score!", position, Color.CornflowerBlue);

            var playerIndex = 1;
            foreach (var playerScore in PlayersScore)
            {
                position += new Vector2(0, 40);

                var score = String.Format("{0}. {1} points", playerIndex, playerScore.Points);
                SpriteBatch.DrawString(_scoreSpriteFont, score, position, playerScore.Color);

                playerIndex++;
            }

            SpriteBatch.End();
        }
    }
}
