#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GradedUnitGame
{

    /// <summary>
    /// This is the main class for the game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        SpriteBatch sBatch;
       // Boolean sound = true;
       // Boolean gameOver = false;
        

        

        static readonly string[] preloadAssets =
        {
            "gradient",
        };


        



        public Game()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 853;
            graphics.PreferredBackBufferHeight = 480;

            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            screenManager.AddScreen(new MainMenu(), null);
           
        }

        /// <summary>
        /// Allows the game to perform initialization before starting
        /// This is where it can query for required services and load non-graphic
        /// related content.  
        /// </summary>
        protected void Initialise()
        {
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "World Defense";
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected void LoadContent(Texture2D playerSprite)
        {
            foreach (string asset in preloadAssets)
            {
                Content.Load<object>(asset);
            }

            // Create a new SpriteBatch, which can be used to draw textures.
            sBatch = new SpriteBatch(GraphicsDevice);

            
            //Contents Manager Loads images
            ContentManager aLoader = new ContentManager(this.Services);
            
           // playerSprite = aLoader.Load<Texture2D>("player1") as Texture2D;
           // playerSprite.Load(this.Content);
            

        }

        /// <summary>
        /// UnloadContent will be called once per game and is where all content ill be unloaded
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        //    UpdateLasers();
        }

        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        //protected override 
        protected override void Draw(GameTime gameTime) 
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
    
        }

    
        #region gameEntry
        static class Program
        {
            /// <summary>
            /// The main entry point for the application.
            /// </summary>
            static void Main()
            {
                using (Game game = new Game())
                {
                    game.Run();
                }
            }
        }
        #endregion
    }
}
