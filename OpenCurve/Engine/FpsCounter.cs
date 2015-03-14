namespace OpenCurve.Engine
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class FpsCounter : IOpenCurveComponent
    {
        private readonly ContentManager _content;
        private readonly SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        private int _frameRate;
        private int _frameCounter;
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        public FpsCounter(ContentManager content, SpriteBatch spriteBatch)
        {
            _content = content;
            _spriteBatch = spriteBatch;
        }

        public void Initialize()
        {
        }

        public void LoadContent()
        {
            _spriteFont = _content.Load<SpriteFont>("GameFont");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime > TimeSpan.FromSeconds(1))
            {
                _elapsedTime -= TimeSpan.FromSeconds(1);
                _frameRate = _frameCounter;
                _frameCounter = 0;
            }
        }

        public void Draw(GameTime gameTime)
        {
            _frameCounter++;

            var fps = String.Format("FPS: {0}", _frameRate);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(_spriteFont, fps, new Vector2(4, 0), Color.White);

            _spriteBatch.End();
        }
    }
}