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
            #region attributes

        //tells the game if loading is slow, used to determine if message should be shown
        bool loadingIsSlow;
        //tells the game if the other screens have transitioned off
        bool otherScreensGone;

        //tells the game which screens to load
        GameScreen[] screensToLoad;

        #endregion

        #region Initialization


        //constructor
        private Loading(ScreenManager screenManager, bool loadingIsSlow,
                              GameScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            TransOnTime = TimeSpan.FromSeconds(0.5);
        }


        //activates loading screen
        public static void Load(ScreenManager screenManager, bool loadingIsSlow, PlayerIndex? conPlayer, params GameScreen[] screensToLoad)
        {
            //tells the current screens to transition off
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.Exit();

            //creates and activate loading screen
            Loading loadingScreen = new Loading(screenManager, loadingIsSlow,screensToLoad);
            screenManager.AddScreen(loadingScreen, conPlayer);
        }

        #endregion


        #region Update and Draw

        //updates loading screen
        public override void Update(GameTime gameTime, bool otherScreenFocus, bool coveredByOther)
        {
            base.Update(gameTime, otherScreenFocus, coveredByOther);

           //perform load if all screens have transitioned off
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

                //ResetElapsedTime tells game timing to not try to catch up 
                ScreenManager.Game.ResetElapsedTime();
            }
        }


        //draws loading screen
        public override void Draw(GameTime gameTime)
        {
            //if this is the only active screen, all others have transitioned off
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensGone = true;
            }


            //indicates how long loading will take, and if the loading message should be shown
            if (loadingIsSlow)
            {
                SpriteBatch sBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                const string message = "Loading...";

                //centers text
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPos = (viewportSize - textSize) / 2;

                Color color = Color.White * TransAlpha;

                //draws text
                sBatch.Begin();
                sBatch.DrawString(font, message, textPos, color);
                sBatch.End();
            }
        }
    }
}
        #endregion