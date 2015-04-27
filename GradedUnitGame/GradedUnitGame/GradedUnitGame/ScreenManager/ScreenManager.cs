#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GradedUnitGame
{
    public class ScreenManager : DrawableGameComponent
    {
        #region attributes
        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate = new List<GameScreen>();

        InputState input = new InputState();

        SpriteBatch sBatch;
        SpriteFont font;
        Texture2D blankTexture;

        bool isInitialized;
        #endregion

        #region getters 
        //returns the spritebatch
        public SpriteBatch SpriteBatch
        {
            get { return sBatch; }
        }
      
        //returns the game font
        public SpriteFont Font
        {
            get { return font; }
        }

        //returns array of screens
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        #endregion

        //constructor
        public ScreenManager(Game game):base(game)
        {
        }

        //initialises screen manager
        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }


        protected override void LoadContent()
        {
            //loads content belonging to screen maanger
            ContentManager content = Game.Content;

            sBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("./UI Misc/MainFont");
            blankTexture = content.Load<Texture2D>("./UI Misc/blank");

            //tells screens to load content
            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
            //tells screens to unload
            foreach (GameScreen screen in screens)
            {
                screen.UnloadContent();
            }
        }

        #region update & draw
        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            input.Update();

            //makes copy of master 
            screensToUpdate.Clear();

            foreach (GameScreen screen in screens)
                screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            //loop whilst screens are waiting to update
            while (screensToUpdate.Count > 0)
            {
                //pops top screen from waiting list
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                //updates screen
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    //lets screen accept input
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(input);

                        otherScreenHasFocus = true;
                    }

                    //tells screen if active popup
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }


        //tells screen to draw itself
        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }

        //adds screen to screen manager
        public void AddScreen(GameScreen screen, PlayerIndex? conPlayer)
        {
            screen.ConPlayer = conPlayer;
            screen.ScreenManager = this;
            screen.IsExiting = false;

            //if graphics are initilized, tell screen load 
            if (isInitialized)
            {
                screen.LoadContent();
            }

            screens.Add(screen);
        }


       //removes screen from screen manager
        public void RemoveScreen(GameScreen screen)
        {
            //if graphics are initilized, tell screen to unload
            if (isInitialized)
            {
                screen.UnloadContent();
            }

            screens.Remove(screen);
            screensToUpdate.Remove(screen);
        }


        

       //draws fullscreen translucent black background, for darkening and fading screens
        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            sBatch.Begin();

            sBatch.Draw(blankTexture,
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             Color.Black * alpha);

            sBatch.End();
        }
        #endregion
    }
}
