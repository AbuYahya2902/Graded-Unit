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
        //constructor for the main menu
           public MainMenu() : base("Main Menu")
        {
            #region attributes
            //creates menu entries
            MenuEntry arcadeModeMenuEntry = new MenuEntry("Arcade Mode");
            MenuEntry endlessModeMenuEntry = new MenuEntry("Endless Mode");
            MenuEntry coopModeMenuEntry = new MenuEntry("Co-Op Mode");
            MenuEntry highscoreMenuEntry = new MenuEntry("High Scores");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
            #endregion

            #region event handlers
            //menu event handlers
            arcadeModeMenuEntry.Selected += ArcadeModeMenuEntrySel;
            endlessModeMenuEntry.Selected += EndlessModeMenuEntrySel;
            coopModeMenuEntry.Selected += CoopModeMenuEntrySel;  
            highscoreMenuEntry.Selected += HighscoreMenuEntrySel;
            optionsMenuEntry.Selected += OptionsMenuEntrySel;
            exitMenuEntry.Selected += OnCancel;
            #endregion

            #region menu entries
            //adds entries to menu
            MenuEntries.Add(arcadeModeMenuEntry);
            MenuEntries.Add(endlessModeMenuEntry);
            MenuEntries.Add(coopModeMenuEntry);
            MenuEntries.Add(highscoreMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
            #endregion
        }


           #region Initilization
           //event handler for when endless Mode entry is selected
        void EndlessModeMenuEntrySel(object sender, PlayerIndexEventArgs e)
        {
            Loading.Load(ScreenManager, true, e.PlayerIndex,
                               new EndlessGameplay());
        }

        //event handler for when Arcade Mode entry is selected
        void ArcadeModeMenuEntrySel(object sender, PlayerIndexEventArgs e)
        {
            Loading.Load(ScreenManager, true, e.PlayerIndex,
                               new ArcadeGameplay());
        }

        //event handler for when Co-op Mode entry is selected
        void CoopModeMenuEntrySel(object sender, PlayerIndexEventArgs e)
        {
            Loading.Load(ScreenManager, true, e.PlayerIndex,
                               new CoopGameplay());
        }

        //event handler for when options entry is selected
        void OptionsMenuEntrySel(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsScreen(), e.PlayerIndex);
        }

        void HighscoreMenuEntrySel(object sender, PlayerIndexEventArgs e)
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
           #endregion
}
