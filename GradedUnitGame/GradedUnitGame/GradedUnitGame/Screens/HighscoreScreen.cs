#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
   class HighscoreScreen : MenuScreen
   {
       #region attributes
       
       MenuEntry modeEntry;
       DatabaseInt dataInt;
       //holds the number of rows currently in the query
       int rowDraw = 0;
       //holds the maximum number of rows that will be drawn
       int maxRows = 10;
       //holds the currently selected game mode
       string gameMode;
       #endregion

       enum mode
       {
           Arcade,
           Endless,
           Cooperative,
       }

       static mode currentMode = mode.Arcade;

       //constructor
        public HighscoreScreen() 
            : base("High Scores") 
        {
            modeEntry = new MenuEntry(string.Empty);
            setMenuText();
            gameMode = "Arcade";

            MenuEntry back = new MenuEntry("Return");

            modeEntry.Selected += modeEntrySel;
            back.Selected += OnCancel;

            MenuEntries.Add(modeEntry);
            MenuEntries.Add(back);

            dataInt = new DatabaseInt();
            dataInt.ReadDatabase(gameMode);
        }
        void setMenuText()
        {
            modeEntry.Text = "Mode: " + currentMode;
        }

       void modeEntrySel(object sender, PlayerIndexEventArgs e)
        {
            currentMode++;
            if (currentMode > mode.Cooperative)
                currentMode = 0;
           if (currentMode == mode.Arcade)
           {
               gameMode = "Arcade";
               dataInt.ReadDatabase(gameMode);
           }
           else if (currentMode == mode.Cooperative)
           {
               gameMode = "CoOp";
               dataInt.ReadDatabase(gameMode);
           }
           else
           {
               gameMode = "Endless";
               dataInt.ReadDatabase(gameMode);
           }
           // setMenuText();
       }

       //draws the current gamemode and score
       public override void Draw(GameTime gameTime)
       {
           SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
           SpriteFont font = ScreenManager.Font;
           spriteBatch.Begin();
           spriteBatch.DrawString(font, "Mode: " + currentMode, new Vector2(300, 10), Color.White);
           int i = 1;
           int count = 0;
           rowDraw = dataInt.CheckRows();
           while (i <= rowDraw && i <= maxRows)
           {
               dataInt.Draw(font, spriteBatch, count);
               i++;

           }
           spriteBatch.End();
       }

       //loads content for this screen
       public override void LoadContent()
       {
           ContentManager content = ScreenManager.Game.Content;
       }

        }
    
}
