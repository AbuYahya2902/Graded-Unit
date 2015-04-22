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
            // Create menu entries.
            MenuEntry endlessModeMenuEntry = new MenuEntry("Endless Mode");
            MenuEntry arcadeModeMenuEntry = new MenuEntry("Arcade Mode");
            MenuEntry coopModeMenuEntry = new MenuEntry("Co-Op Mode");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry highscoreMenuEntry = new MenuEntry("High Scores");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
            

            // menu event handlers.
            endlessModeMenuEntry.Selected += endlessModeMenuEntrySelected;
            arcadeModeMenuEntry.Selected += arcadeModeMenuEntrySelected;
            coopModeMenuEntry.Selected += coopModeMenuEntrySelected;  
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            highscoreMenuEntry.Selected += highscoreMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to menu.
            MenuEntries.Add(endlessModeMenuEntry);
            MenuEntries.Add(arcadeModeMenuEntry);
            MenuEntries.Add(coopModeMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(highscoreMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

   
       /// <summary>
        /// Event handler for when the endless Mode menu entry is selected.
        /// </summary>
        void endlessModeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Loading.Load(ScreenManager, true, e.PlayerIndex,
                               new EndlessGameplay());
        }

        /// <summary>
        /// Event handler for when the Arace Mode menu entry is selected.
        /// </summary>
        void arcadeModeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Loading.Load(ScreenManager, true, e.PlayerIndex,
                               new Gameplay());
        }

        /// <summary>
        /// Event handler for when the Co-op Mode menu entry is selected.
        /// </summary>
        void coopModeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Loading.Load(ScreenManager, true, e.PlayerIndex,
                               new CoopGameplay());
        }

        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsScreen(), e.PlayerIndex);
        }

        void highscoreMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HighscoreScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// When user cancels the main menu, ask if they want to exit
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit the game?";

            MsgboxScreen confirmExitMessageBox = new MsgboxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


    }

}
