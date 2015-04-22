#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace GradedUnitGame
{
    class PauseScreen : MenuScreen
    {
        public PauseScreen() : base("Paused")
        {
            // Create menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");
            
            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySel;

            // Add entries to menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySel(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit?";

            MsgboxScreen confirmMsgBox = new MsgboxScreen(message);

            confirmMsgBox.Accepted += ConfirmMsgBoxAccepted;
            ScreenManager.AddScreen(confirmMsgBox, ConPlayer);
        }


        /// <summary>
        /// Event handler for when the user selects ok on message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmMsgBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            Loading.Load(ScreenManager, false, null, new BgScreen(), new MainMenu());
        }

    }
}
