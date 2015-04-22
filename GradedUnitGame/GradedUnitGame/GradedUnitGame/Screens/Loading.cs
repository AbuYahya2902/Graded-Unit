#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GradedUnitGame
{
    class Loading : GameScreen
    {
            #region Fields

        bool loadingIsSlow;
        bool otherScreensGone;

        GameScreen[] screensToLoad;

        #endregion

        #region Initialization


        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        private Loading(ScreenManager screenManager, bool loadingIsSlow,
                              GameScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            TransOnTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow, PlayerIndex? conPlayer, params GameScreen[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.Exit();

            // Create and activate the loading screen.
            Loading loadingScreen = new Loading(screenManager, loadingIsSlow,screensToLoad);
            screenManager.AddScreen(loadingScreen, conPlayer);
        }

        #endregion


        #region Update and Draw

        /// <summary>
        /// Updates the loading screen.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenFocus, bool coveredByOther)
        {
            base.Update(gameTime, otherScreenFocus, coveredByOther);

            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load.
            if (otherScreensGone)      
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen, ConPlayer);
                    }
                }

                // ResetElapsedTime tells game timing to not try to catch up 
                ScreenManager.Game.ResetElapsedTime();
            }
        }


        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. 
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensGone = true;
            }


            // The gameplay screen can take a while, so a loading message is displayed.
            // Parameter indicates how long loading will take, in order to determine if the message should be shown

            if (loadingIsSlow)
            {
                SpriteBatch sBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                const string message = "Loading...";

                // Center the text in the viewport.
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPos = (viewportSize - textSize) / 2;

                Color color = Color.White * TransAlpha;

                // Draw the text.
                sBatch.Begin();
                sBatch.DrawString(font, message, textPos, color);
                sBatch.End();
            }
        }
    }
}
        #endregion