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
    class OptionsScreen : MenuScreen
    {
        #region attributes
        //initialises menu entries as MenuEntry class
        public MenuEntry laserColourEntry;
        public MenuEntry playerShipEntry;
        public MenuEntry musicEntry;
        #endregion

        #region menu enums
        //lasercolour options
      public enum laserColour
        {
            Red,
            Green,
            Blue,
            Yellow,
            DrkSalmon,
        }

        //playership options
        public enum playerShip
        {
            One,
            Two,
            Three,
        }

        //sound options
        public enum music
        {
            On,
            Off,
        }
        #endregion

        #region Initilization
        //initialises the menu option
        public static laserColour currentColour = laserColour.Red;
        public static playerShip currentShip = playerShip.One;
        public static music currentMusic = music.On;

        //constructor
        public OptionsScreen()
            : base("Options")
        {
            laserColourEntry = new MenuEntry(string.Empty);
            playerShipEntry = new MenuEntry(string.Empty);
            musicEntry = new MenuEntry(string.Empty);

            SetMenuText();

            MenuEntry back = new MenuEntry("Return");

            laserColourEntry.Selected += LaserColourEntrySel;
            playerShipEntry.Selected += PlayerShipEntrySel;
            musicEntry.Selected += MusicEntrySel;
            back.Selected += OnCancel;

            MenuEntries.Add(laserColourEntry);
            MenuEntries.Add(playerShipEntry);
            MenuEntries.Add(musicEntry);
            MenuEntries.Add(back);
            
        }

        //sets the menu text
            void SetMenuText()
            {
                laserColourEntry.Text = "Laser Colour: " + currentColour;
                playerShipEntry.Text = "Ship Design: " + currentShip;
                musicEntry.Text = "Music: " + currentMusic;
            }

        //toggles through lasercolour options when selected
        void LaserColourEntrySel(object sender, PlayerIndexEventArgs e)
            {
                currentColour++;
                if (currentColour > laserColour.DrkSalmon)
                    currentColour = 0;
                SetMenuText();
            }

        //toggles through the playership option when selected
        void PlayerShipEntrySel(object sender, PlayerIndexEventArgs e)
        {
            currentShip++;
            if (currentShip > playerShip.Three)
                currentShip = 0;
            SetMenuText();
        }

        //if player selects the sound option, it will toggle between on and off
        void MusicEntrySel(object sender, PlayerIndexEventArgs e)
        {
            currentMusic++;
            if (currentMusic > music.Off)
                currentMusic = 0;
            SetMenuText();
        }

        //loads content, this is called once per screen
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
        }

        //todo change playersprite draw to the playership option
        //todo change lasersprite color to lasercolour option
    }
        #endregion
}
