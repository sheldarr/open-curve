namespace OpenCurve.Engine
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class FpsCounter : IOpenCurveComponent
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly SpriteFont _spriteFont;

        private int _frameRate;
        private int _frameCounter;
        private TimeSpan _elapsedTime = TimeSpan.Zero;

        public FpsCounter()
        {
            _spriteBatch = GameServices.GetService<SpriteBatch>();
            _spriteFont = GameServices.GetService<ContentManager>().Load<SpriteFont>("GameFont");
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

            _spriteBatch.DrawString(_spriteFont, fps, new Vector2(4, 30), Color.White);

            _spriteBatch.End();
        }
    }
}