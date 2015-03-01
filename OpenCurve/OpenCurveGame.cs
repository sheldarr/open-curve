using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OpenCurve
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class OpenCurveGame : Game
    {
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;

        private List<Player> Players { get; set; }

        public OpenCurveGame()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Players = new List<Player>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            var player = new Player
            {
                Color = Color.Yellow,
                Position = new Vector2(100, 100)
            };

            Players.Add(player);

            var player2 = new Player
            {
                Color = Color.Red,
                Position = new Vector2(400, 400)
            };

            Players.Add(player2);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Players.ForEach(p => p.MakeMove());

            foreach (var player in Players)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    player.TurnLeft();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    player.TurnRight();
                }
                //if (Players.SelectMany(p => p.PreviousPositions).Any(pp => pp == player.Position))
                //{
                //}
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin();

            foreach (var player in Players)
            {
                var circle = CreateCircleText(player.Size);

                foreach (var position in player.PreviousPositions)
                {
                    _spriteBatch.Draw(circle, position, player.Color);
                }
            }

            var font = Content.Load<SpriteFont>("GameFont");
            var pos = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            _spriteBatch.DrawString(font, "HEHESZKI", pos, Color.Red);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        Texture2D CreateCircleText(int radius)
        {
            var texture = new Texture2D(GraphicsDevice, radius, radius);
            var colorData = new Color[radius * radius];

            var diam = radius / 2f;
            var diamsq = diam * diam;

            for (var x = 0; x < radius; x++)
            {
                for (var y = 0; y < radius; y++)
                {
                    var index = x * radius + y;
                    var pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }
    }
}
