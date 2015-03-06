namespace OpenCurve
{
    using System.Collections.Generic;
    using Engine;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public delegate void OnExit();

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class OpenCurveGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;

        private MainMenu _mainMenu;
        private Gameplay _gameplay;

        private IGameModule _activeGameModule;

        private List<Player> Players { get; set; }

        public OpenCurveGame()
        {
            Content.RootDirectory = "Content";

            _graphicsDeviceManager = new GraphicsDeviceManager(this) {PreferMultiSampling = true};
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _mainMenu = new MainMenu(Content, GraphicsDevice);
            _gameplay = new Gameplay(Content, GraphicsDevice);

            _mainMenu.Initialize();
            _gameplay.Initialize();

            _mainMenu.Exit = MainMenuExit;
            _gameplay.Exit = GameplayExit;

            _activeGameModule = _mainMenu;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _mainMenu.LoadContent();
            _gameplay.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            _mainMenu.UnloadContent();
            _gameplay.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _activeGameModule.Update(gameTime);
           
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _activeGameModule.Draw(gameTime);

            base.Draw(gameTime);
        }

        public void MainMenuExit()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _activeGameModule = _gameplay;
        }

        public void GameplayExit()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _activeGameModule = _mainMenu;
        }
    }
}
