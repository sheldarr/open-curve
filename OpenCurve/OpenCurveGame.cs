namespace OpenCurve
{
    using System.Collections.Generic;
    using Engine;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using OpenTK.Input;

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
            var gameOptions = new GameOptions
            {
                BoardSize = new BoardSize
                {
                    Width = GraphicsDevice.PresentationParameters.BackBufferWidth,
                    Height = GraphicsDevice.PresentationParameters.BackBufferHeight
                },
                PlayerOptions = new List<PlayerOptions>
                {
                    new PlayerOptions
                    {
                        Color = Color.Red,
                        MoveLeftKey = Keys.A,
                        MoveRightKey = Keys.D,
                        PadController = false,
                        PlayerIndex = PlayerIndex.One
                    },
                    new PlayerOptions
                    {
                        Color = Color.Blue,
                        MoveLeftKey = Keys.Left,
                        MoveRightKey = Keys.Right,
                        PadController = false,
                        PlayerIndex = PlayerIndex.Two
                    }
                }
            };

            GraphicsDevice.Clear(Color.CornflowerBlue);
            _board.Reset(gameOptions);
            _activeOpenCurveComponent = _board;
        }

        public void GameplayExit()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _activeOpenCurveComponent = _mainMenu;
        }
    }
}
