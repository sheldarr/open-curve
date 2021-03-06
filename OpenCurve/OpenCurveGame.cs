﻿namespace OpenCurve
{
    using System.Linq;
    using Engine;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public delegate void OnExit();

    public class OpenCurveGame : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;

        private MainMenu _mainMenu;
        private Board _board;
        private Score _score;

        private IOpenCurveComponent _activeOpenCurveComponent;

        public OpenCurveGame()
        {
            Content.RootDirectory = "Content";

            _graphicsDeviceManager = new GraphicsDeviceManager(this) {PreferMultiSampling = true};
        }

        protected override void Initialize()
        {
            var spriteBatch = new SpriteBatch(GraphicsDevice);

            GameServices.AddService(Content);
            GameServices.AddService(GraphicsDevice);
            GameServices.AddService(_graphicsDeviceManager);
            GameServices.AddService(Window);
            GameServices.AddService(spriteBatch);

            _mainMenu = new MainMenu();
            _board = new Board();
            _score = new Score();

            _mainMenu.Exit = MainMenuExit;
            _board.Exit = GameplayExit;
            _score.Exit = ScoreExit;

            _activeOpenCurveComponent = _mainMenu;

            _graphicsDeviceManager.GraphicsDevice.PresentationParameters.MultiSampleCount = 16;

            _graphicsDeviceManager.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            _graphicsDeviceManager.IsFullScreen = true;
            _graphicsDeviceManager.ApplyChanges();

            base.Initialize();

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
            if (_mainMenu.GameOptions.ExitGame)
            {
                Exit();
            }

            _graphicsDeviceManager.PreferredBackBufferWidth = _mainMenu.GameOptions.BoardSize.Width;
            _graphicsDeviceManager.PreferredBackBufferHeight = _mainMenu.GameOptions.BoardSize.Height;
            _graphicsDeviceManager.IsFullScreen = _mainMenu.GameOptions.Fullscreen;
            _graphicsDeviceManager.ApplyChanges();

            _board.Reset(_mainMenu.GameOptions);
            _activeOpenCurveComponent = _board;
        }

        public void GameplayExit()
        {
            _score.PlayersScore = _board.Players.Select(p => new PlayerScore { Color = p.Color, Points = p.Points }).OrderByDescending(p => p.Points);
            _activeOpenCurveComponent = _score;
        }

        public void ScoreExit()
        {
            _activeOpenCurveComponent = _mainMenu;
        }
    }
}
