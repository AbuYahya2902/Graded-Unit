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
        #region attributes
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        SpriteBatch sBatch;
        #endregion 

        #region initilization
        //preloads asset to reduce strain
        static readonly string[] preloadAssets =
        {
            "gradient",
        };

        //constructor
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

        //allows game to initilise
        protected void Initialise()
        {
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "World Defense";
            base.Initialize();
        }

        //loads content for game, called once per game
        protected void LoadContent(Texture2D playerSprite)
        {
            foreach (string asset in preloadAssets)
            {
                Content.Load<object>(asset);
            }

            //creates new spritebatch to draw textures
            sBatch = new SpriteBatch(GraphicsDevice);

            
            //Content Manager loads images
            ContentManager aLoader = new ContentManager(this.Services);
        }

       //unloads game content, called once per game
        protected override void UnloadContent()
        {
            
        }

        #endregion

        #region update & draw
        //allows game to run logic
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //allows game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        
        }

        

        //draws game
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) 
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
    
        }

        static class Program
        {
            //main entry point
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
