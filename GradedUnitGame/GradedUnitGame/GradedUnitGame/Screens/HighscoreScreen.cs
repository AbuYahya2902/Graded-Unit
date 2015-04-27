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
       int rowDraw = 0;
       int maxRows = 20;
       string gameMode;
       #endregion

       enum mode
       {
           Arcade,
           Endless,
           Cooperative,
       }

       static mode currentMode = mode.Arcade;

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
            setMenuText();
       }

       public override void LoadContent()
       {
           ContentManager content = ScreenManager.Game.Content;
       }

        }
    
}
