namespace OpenCurve
{
    using Engine;
    using Microsoft.Xna.Framework;

    public delegate void OnExit();

    public class OpenCurveGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        private MainMenu _mainMenu;
        private Gameplay _gameplay;

        private IOpenCurveComponent _activeOpenCurveComponent;

        public OpenCurveGame()
        {
            Content.RootDirectory = "Content";

            _graphicsDeviceManager = new GraphicsDeviceManager(this) {PreferMultiSampling = true};
        }

        protected override void Initialize()
        {
            _mainMenu = new MainMenu(Content, GraphicsDevice);
            _gameplay = new Gameplay(Content, GraphicsDevice);

            _mainMenu.Initialize();
            _gameplay.Initialize();

            _mainMenu.Exit = MainMenuExit;
            _gameplay.Exit = GameplayExit;

            _activeOpenCurveComponent = _mainMenu;

            _graphicsDeviceManager.GraphicsDevice.PresentationParameters.MultiSampleCount = 16;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _mainMenu.LoadContent();
            _gameplay.LoadContent();
        }

        protected override void UnloadContent()
        {
            _mainMenu.UnloadContent();
            _gameplay.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            _activeOpenCurveComponent.Update(gameTime);
           
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _activeOpenCurveComponent.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void MainMenuExit()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _activeOpenCurveComponent = _gameplay;
        }

        public void GameplayExit()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _activeOpenCurveComponent = _mainMenu;
        }
    }
}
