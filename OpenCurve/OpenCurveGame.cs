namespace OpenCurve
{
    using Engine;
    using Microsoft.Xna.Framework;

    public delegate void OnExit();

    public class OpenCurveGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        private MainMenu _mainMenu;
        private Board _board;

        private IOpenCurveComponent _activeOpenCurveComponent;

        public OpenCurveGame()
        {
            Content.RootDirectory = "Content";

            _graphicsDeviceManager = new GraphicsDeviceManager(this) {PreferMultiSampling = true};
        }

        protected override void Initialize()
        {
            _mainMenu = new MainMenu(Content, GraphicsDevice);
            _board = new Board(Content, GraphicsDevice);

            _mainMenu.Initialize();
            _board.Initialize();

            _mainMenu.Exit = MainMenuExit;
            _board.Exit = GameplayExit;

            _activeOpenCurveComponent = _mainMenu;

            _graphicsDeviceManager.GraphicsDevice.PresentationParameters.MultiSampleCount = 16;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _mainMenu.LoadContent();
            _board.LoadContent();
        }

        protected override void UnloadContent()
        {
            _mainMenu.UnloadContent();
            _board.UnloadContent();
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
            _board.Reset(_mainMenu.GameOptions);
            _activeOpenCurveComponent = _board;
        }

        public void GameplayExit()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _activeOpenCurveComponent = _mainMenu;
        }
    }
}
