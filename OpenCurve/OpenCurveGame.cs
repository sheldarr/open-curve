using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace OpenCurve
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class OpenCurveGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private List<Player> Players { get; set; }

        public OpenCurveGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
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
                Position = new Vector2(100, 100)
            };

            Players.Add(player);

            var player2 = new Player
            {
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
            spriteBatch = new SpriteBatch(GraphicsDevice);

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

            foreach (var player in Players)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    player.Position += new Vector2(0, -1);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    player.Position += new Vector2(0, 1);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    player.Position += new Vector2(-1, 0);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    player.Position += new Vector2(1, 0);
                }
                player.HistoryOfPosition.Add(player.Position);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            var circle = createCircleText(4);

            spriteBatch.Begin();

            foreach (var player in Players)
            {
                foreach (var position in player.HistoryOfPosition)
                {
                    spriteBatch.Draw(circle, position, new Color(255, 0, 0));
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        Texture2D createCircleText(int radius)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, radius, radius);
            Color[] colorData = new Color[radius * radius];

            float diam = radius / 2f;
            float diamsq = diam * diam;

            for (int x = 0; x < radius; x++)
            {
                for (int y = 0; y < radius; y++)
                {
                    int index = x * radius + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
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
