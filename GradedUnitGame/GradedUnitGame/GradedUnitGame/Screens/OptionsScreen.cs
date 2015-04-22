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
        //initialises menu entries as MenuEntry class
        MenuEntry laserColourEntry;
        MenuEntry playerShipEntry;
        MenuEntry soundEntry;
        
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
        enum playerShip
        {
            One,
            Two,
            Three,
        }

        //sound options
        enum sound
        {
            On,
            Off,
        }

        //initialises the menu option
        public static laserColour currentColour = laserColour.Red;
        static playerShip currentShip = playerShip.One;
        static sound currentSound = sound.On;

        public OptionsScreen()
            : base("Options")
        {
            laserColourEntry = new MenuEntry(string.Empty);
            playerShipEntry = new MenuEntry(string.Empty);
            soundEntry = new MenuEntry(string.Empty);

            setMenuText();

            MenuEntry back = new MenuEntry("Return");

            laserColourEntry.Selected += laserColourEntrySel;
            playerShipEntry.Selected += playerShipEntrySel;
            soundEntry.Selected += soundEntrySel;
            back.Selected += OnCancel;

            MenuEntries.Add(laserColourEntry);
            MenuEntries.Add(playerShipEntry);
            MenuEntries.Add(soundEntry);
            MenuEntries.Add(back);
            
        }

        //todo, toggle sound mutes all sounds
      // private void toggleSound()
       // {
          //   if (currentSound = sound.Off)
         //   {
          //      playMusic = false;
                //set sound to OFF
         //   }
         //  else if (sound currentSound = sound.On)
          //  {
           //     playMusic = true;
                //set sound to ON
         //   }
         //   return playMusic;
         // }


        //sets the menu text
            void setMenuText()
            {
                laserColourEntry.Text = "Laser Colour: " + currentColour;
                playerShipEntry.Text = "Ship Design: " + currentShip;
                soundEntry.Text = "Sound: " + currentSound;
            }

        //toggles through lasercolour options when selected
        void laserColourEntrySel(object sender, PlayerIndexEventArgs e)
            {
                currentColour++;
                if (currentColour > laserColour.DrkSalmon)
                    currentColour = 0;
                setMenuText();
            }

        //toggles through the playership option when selected
        void playerShipEntrySel(object sender, PlayerIndexEventArgs e)
        {
            currentShip++;
            if (currentShip > playerShip.Three)
                currentShip = 0;
            setMenuText();
        }

        //if player selects the sound option, it will toggle between on and off
        void soundEntrySel(object sender, PlayerIndexEventArgs e)
        {
            currentSound++;
            if (currentSound > sound.Off)
                currentSound = 0;
            setMenuText();
        }

        //loads content, this is called once per screen
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
        }

        //todo change playersprite draw to the playership option
        //todo change lasersprite color to lasercolour option
       
        

        }
    
}
