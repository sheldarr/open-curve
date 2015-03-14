namespace OpenCurve.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class MainMenu : IOpenCurveComponent
    {
        private const int MaxPlayers = 8;

        private KeyboardState LastKeyboardState { get; set; }
        
        private ContentManager Content { get; set; }
        private GraphicsDevice GraphicsDevice { get; set; }
        private SpriteBatch SpriteBatch { get; set; }

        public GameOptions GameOptions { get; private set; }

        public OnExit Exit;

        private SpriteFont _menuSpriteFont;

        public MainMenu(ContentManager content, GraphicsDevice graphicsDevice)
        {
            Content = content;
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            GameOptions = new GameOptions
            {
                BoardSize = new BoardSize
                {
                    Width = GraphicsDevice.PresentationParameters.BackBufferWidth,
                    Height = GraphicsDevice.PresentationParameters.BackBufferHeight
                },
                PlayerOptions = new List<PlayerOptions>()
            };

            AddGamePadPlayers();
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
            var actualKeyboardState = Keyboard.GetState();

            if (actualKeyboardState.IsKeyDown(Keys.Up) && LastKeyboardState.IsKeyUp(Keys.Up))
            {
                if (GameOptions.PlayerOptions.Count < MaxPlayers)
                {
                    var newPlayerOptions = new PlayerOptions
                    {
                        Color = NextColor(),
                        PlayerControls = KeyboardControls(GameOptions.PlayerOptions.Count)
                    };

                    GameOptions.PlayerOptions.Add(newPlayerOptions);
                }
            }
            if (actualKeyboardState.IsKeyDown(Keys.Down) && LastKeyboardState.IsKeyUp(Keys.Down))
            {
                if (GameOptions.PlayerOptions.Count > 0)
                {
                    var lastPlayerOptions = GameOptions.PlayerOptions.Last();
                    GameOptions.PlayerOptions.Remove(lastPlayerOptions);
                }
            }
            if (actualKeyboardState.IsKeyDown(Keys.Enter))
            {
                Exit();
            }

            LastKeyboardState = actualKeyboardState;
        }

        public void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();

            var position = new Vector2(GraphicsDevice.Viewport.Width / 2 - 160, 20);

            SpriteBatch.DrawString(_menuSpriteFont, "Welcome to OpenCurve!", position, Color.CornflowerBlue);

            position += new Vector2(0, 40);

            SpriteBatch.DrawString(_menuSpriteFont, "Press [ENTER] to start the game!", position, Color.CornflowerBlue);

            position += new Vector2(0, 40);

            var playerIndex = 1;
            foreach (var playerOptions in GameOptions.PlayerOptions)
            {
                position += new Vector2(0, 40);
                string playerInfo = String.Empty;

                if (playerOptions.PlayerControls.PadController)
                {
                    playerInfo = String.Format("{0}. Player controls gamepad[{1}]", playerIndex, playerOptions.PlayerControls.PlayerIndex);
                    
                }
                else
                {
                    playerInfo = String.Format("{0}. Player controls [{1}][{2}]", playerIndex, playerOptions.PlayerControls.MoveLeftKey, playerOptions.PlayerControls.MoveRightKey);
                }
                SpriteBatch.DrawString(_menuSpriteFont, playerInfo, position, playerOptions.Color);

                playerIndex++;
            }

            SpriteBatch.End();
        }

        private Color NextColor()
        {
            switch (GameOptions.PlayerOptions.Count)
            {
                case(0):
                    return Color.Red;
                case (1):
                    return Color.Green;
                case (2):
                    return Color.Blue;
                case (3):
                    return Color.Yellow;
                case (4):
                    return Color.Magenta;
                case (5):
                    return Color.DarkSlateGray;
                case (6):
                    return Color.Cyan;
                default:
                    return Color.White;
            }
        }

        private PlayerControls KeyboardControls(int playerIndex)
        {
            switch (playerIndex)
            {
                case (0):
                    return new PlayerControls { MoveLeftKey = Keys.Left, MoveRightKey = Keys.Right };
                case (1):
                    return new PlayerControls { MoveLeftKey = Keys.A, MoveRightKey = Keys.S };
                case (2):
                    return new PlayerControls { MoveLeftKey = Keys.O, MoveRightKey = Keys.P };
                default:
                    return new PlayerControls { MoveLeftKey = Keys.NumPad4, MoveRightKey = Keys.NumPad6 };
            }
        }

        private PlayerControls GamePadControls(PlayerIndex playerIndex)
        {
            switch (playerIndex)
            {
                case (PlayerIndex.One):
                    return new PlayerControls { PadController = true, PlayerIndex = PlayerIndex.One };
                case (PlayerIndex.Two):
                    return new PlayerControls { PadController = true, PlayerIndex = PlayerIndex.Two };
                case (PlayerIndex.Three):
                    return new PlayerControls { PadController = true, PlayerIndex = PlayerIndex.Three };
                default:
                    return new PlayerControls { PadController = true, PlayerIndex = PlayerIndex.Four };
            }
        }

        private void AddGamePadPlayers()
        {
            AddGamePadPlayer(PlayerIndex.One);
            AddGamePadPlayer(PlayerIndex.Two);
            AddGamePadPlayer(PlayerIndex.Three);
            AddGamePadPlayer(PlayerIndex.Four);
        }

        private void AddGamePadPlayer(PlayerIndex playerIndex)
        {
            if (!GamePad.GetState(playerIndex).IsConnected)
            {
                return;
            }
            
            var newPlayerOptions = new PlayerOptions
            {
                Color = NextColor(),
                PlayerControls = GamePadControls(playerIndex)
            };

            GameOptions.PlayerOptions.Add(newPlayerOptions);
        }
    }
}
