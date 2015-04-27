#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#endregion

namespace GradedUnitGame
{
    class MainMenu : MenuScreen
    {
           public MainMenu() : base("Main Menu")
        {
            #region attributes
            //creates menu entries
            MenuEntry endlessModeMenuEntry = new MenuEntry("Endless Mode");
            MenuEntry arcadeModeMenuEntry = new MenuEntry("Arcade Mode");
            MenuEntry coopModeMenuEntry = new MenuEntry("Co-Op Mode");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry highscoreMenuEntry = new MenuEntry("High Scores");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
            #endregion


            //menu event handlers
            endlessModeMenuEntry.Selected += endlessModeMenuEntrySelected;
            arcadeModeMenuEntry.Selected += arcadeModeMenuEntrySelected;
            coopModeMenuEntry.Selected += coopModeMenuEntrySelected;  
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            highscoreMenuEntry.Selected += highscoreMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            //adds entries to menu
            MenuEntries.Add(endlessModeMenuEntry);
            MenuEntries.Add(arcadeModeMenuEntry);
            MenuEntries.Add(coopModeMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(highscoreMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

   
     
        //event handler for when endless Mode entry is selected
        void endlessModeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Loading.Load(ScreenManager, true, e.PlayerIndex,
                               new EndlessGameplay());
        }

        //event handler for when Arcade Mode entry is selected
        void arcadeModeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Loading.Load(ScreenManager, true, e.PlayerIndex,
                               new Gameplay());
        }

        //event handler for when Co-op Mode entry is selected
        void coopModeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Loading.Load(ScreenManager, true, e.PlayerIndex,
                               new CoopGameplay());
        }

        //event handler for when options entry is selected
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsScreen(), e.PlayerIndex);
        }

        void highscoreMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HighscoreScreen(), e.PlayerIndex);
        }

       //when user wants to exit, asks if they're sure
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit the game?";

            MsgboxScreen confirmExitMessageBox = new MsgboxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        
        //event handler for when user selects ok on messagebox
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


    }

}
