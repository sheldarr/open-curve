namespace OpenCurve.Engine
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class MainMenu : IOpenCurveComponent
    {
        private ContentManager Content { get; set; }
        private GraphicsDevice GraphicsDevice { get; set; }
        private SpriteBatch SpriteBatch { get; set; }

        public OnExit Exit;

        private SpriteFont _menuSpriteFont;

        public MainMenu(ContentManager content, GraphicsDevice graphicsDevice)
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
            _menuSpriteFont = Content.Load<SpriteFont>("MainMenuFont");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Exit();
            }
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();

            var position = new Vector2(GraphicsDevice.Viewport.Width / 2 - 160, GraphicsDevice.Viewport.Height / 2 - 40);

            SpriteBatch.DrawString(_menuSpriteFont, "Welcome to OpenCurve!", position, Color.Green);

            position += new Vector2(0, 40);

            SpriteBatch.DrawString(_menuSpriteFont, "Press ENTER to start the game!", position, Color.Green);

            SpriteBatch.End();
        }
    }
}
