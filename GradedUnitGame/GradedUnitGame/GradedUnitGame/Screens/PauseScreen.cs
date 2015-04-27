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
            //creates menu entries
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");
            
            //hook event handlers
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySel;

            //adds menu entries 
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

      //handles event for when users selects quit game
        void QuitGameMenuEntrySel(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit?";

            MsgboxScreen confirmMsgBox = new MsgboxScreen(message);

            confirmMsgBox.Accepted += ConfirmMsgBoxAccepted;
            ScreenManager.AddScreen(confirmMsgBox, ConPlayer);
        }


     //handles event for user accepts messagebox, then uses loading screen to transition back to main menu
        void ConfirmMsgBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            Loading.Load(ScreenManager, false, null,
                                               new MainMenu());
        }
    
    }
}
