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
       MenuEntry modeEntry;
       DatabaseInt dataInt;
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

            MenuEntry back = new MenuEntry("Return");

            modeEntry.Selected += modeEntrySel;
            back.Selected += OnCancel;

            MenuEntries.Add(modeEntry);
            MenuEntries.Add(back);
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
            setMenuText();
       }

       public override void LoadContent()
       {
           ContentManager content = ScreenManager.Game.Content;
       }

       //todo get highscores from database
     //  public void returnHighScores()
    //   {
     //      if (currentMode == mode.Arcade)
       //       {
                //call arcade mode highscores
         //       dataInt.ReadDatabase();
        //    }
       //   else if (currentMode == mode.Endless)
      //     {
                //call endless mode highscores
        //        dataInt.ReadDatabase();
      //      }
      //      else
                //call cooperative mode highscores
       //         dataInt.ReadDatabase();
     //  }

        }
    
}
