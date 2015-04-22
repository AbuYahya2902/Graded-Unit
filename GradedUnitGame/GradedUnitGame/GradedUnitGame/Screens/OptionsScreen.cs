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
        MenuEntry laserColourEntry;
        MenuEntry playerShipEntry;
        MenuEntry soundEntry;
        Texture2D playerShipTex1;
        Texture2D playerShipTex2;
        Texture2D playerShipTex3;
        Texture2D gradientTexture;
       
        
      public enum laserColour
        {
            Red,
            Green,
            Blue,
            Yellow,
            DrkSalmon,
        }

        enum playerShip
        {
            One,
            Two,
            Three,
        }

        enum sound
        {
            On,
            Off,
        }

        public static laserColour currentColour = laserColour.DrkSalmon;
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

            laserColourEntry.Selected += turretColourEntrySel;
            playerShipEntry.Selected += playerShipEntrySel;
            soundEntry.Selected += soundEntrySel;
            back.Selected += OnCancel;

            MenuEntries.Add(laserColourEntry);
            MenuEntries.Add(playerShipEntry);
            MenuEntries.Add(soundEntry);
            MenuEntries.Add(back);
            
        }

     //   private bool toggleSound(bool sound)
      //  {
       //     if (sound == true)
        //    {
       //         sound = false;
                //set sound to OFF
         //   }
         //   else if (sound == false)
         //   {
           //     sound = true;
                //set sound to ON
          //  }
          //  return sound;

      //  }


            void setMenuText()
            {
                laserColourEntry.Text = "Turret Colour: " + currentColour;
                playerShipEntry.Text = "Ship Design: " + currentShip;
                soundEntry.Text = "Sound: " + currentSound;
            }

        void turretColourEntrySel(object sender, PlayerIndexEventArgs e)
            {
                currentColour++;
                if (currentColour > laserColour.Yellow)
                    currentColour = 0;
                setMenuText();
            }

        void playerShipEntrySel(object sender, PlayerIndexEventArgs e)
        {
            currentShip++;
            if (currentShip > playerShip.Three)
                currentShip = 0;
            setMenuText();
        }

        void soundEntrySel(object sender, PlayerIndexEventArgs e)
        {
            currentSound++;
            if (currentSound > sound.Off)
                currentSound = 0;
            setMenuText();
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            playerShipTex1 = content.Load<Texture2D>("./Players/Player1");
            playerShipTex2 = content.Load<Texture2D>("./Players/Player2");
            playerShipTex3 = content.Load<Texture2D>("./Players/Player3");
            gradientTexture = content.Load<Texture2D>("./Ui Misc/gradient");
        }

        void DrawPreview(GameTime gametime)
        {
            SpriteBatch sBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 spritePos = (viewportSize / 2);
            const int hPad = 32;
            const int vPad = 16;
            Rectangle previewRect = new Rectangle((int)spritePos.X - hPad,
                                                           (int)spritePos.Y - vPad,
                                                           (int)spritePos.X + hPad * 2,
                                                           (int)spritePos.Y + vPad * 2);
            

            sBatch.Begin();

            // Draw the background rectangle.
            //sBatch.Draw(gradientTexture, previewRect, Color.AntiqueWhite);
            //sBatch.Draw(playerShipTex1, new Vector2(100,100),Color.White);
            sBatch.DrawString(font, "test", new Vector2(100, 100), Color.Purple);
            // Draw the message box text.
            //if (currentShip.Equals(playerShip.One))
            //{
            //    sBatch.Draw(playerShipTex1, spritePos, Color.AntiqueWhite);
            //}
            //else if (currentShip.Equals(playerShip.Two))
            //{
            //    sBatch.Draw(playerShipTex2, spritePos, Color.AntiqueWhite);
            //}
            //else
            //{
            //    sBatch.Draw(playerShipTex3, spritePos, Color.AntiqueWhite);
            //}
            
            sBatch.End();
        }

        }
    
}
