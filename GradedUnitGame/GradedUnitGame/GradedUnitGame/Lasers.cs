#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace GradedUnitGame
{
    /// <summary>
    /// this is the main class for the lasers and all variables for projectile motion
    /// </summary>
    class Lasers
    {
        #region attributes
        //texture for the laser
        Texture2D playerLaserTex;

        //current position of the laser
        public Vector2 laserPos;

        //movement of the laser
         Vector2 movement;

        //screen boundary to detect collision w/ screen
        Rectangle screenBoundary;

        //collision detection for other objects
        Rectangle boundary;

        //current state of laser
        public bool isActive;

        //speed of the laser when fired
        float laserSpeed = 1.05f;

        //returns if laser is currently active
        public bool IfIsActive()
        {
            return this.isActive;
        }

        public void SetIsActive(bool isActive)
        {
            this.isActive = isActive;
        }
        #endregion

        #region initilization
        //sets initial values for all variables
        public Lasers(Texture2D texture, Rectangle screenBoundary)
        {
            boundary = new Rectangle(0, 0, texture.Width, texture.Height);
            playerLaserTex = texture;
            this.screenBoundary = screenBoundary;
            isActive = false;
        }

       public void SetLaserColour(Color colour)
        {
           // this.colour = OptionsScreen.currentColour;

        }
        #endregion

       #region draw&update
       //updates the lasers current position
        public void UpdatePosition()
        {
            laserPos += movement *= laserSpeed;
            HasExited();
        }

        //fires the laser
        public void Fire(Rectangle playerPos)
        {
            isActive = true;
            movement = new Vector2(0,-1);
            laserPos.Y = playerPos.Y - playerLaserTex.Height;
            laserPos.X = playerPos.X + (playerPos.Width - playerLaserTex.Width) / 2;
        }

        //gets the boundary for collision detection
        public Rectangle Boundary
        {
            get
            {
                boundary.X = (int)laserPos.X;
                boundary.Y = (int)laserPos.Y;
                return boundary;
            }
        }

        //detects if laser has left the top of the screen
        public void HasExited()
        {
            if (laserPos.Y < 0)
                isActive = false;
        }

        //updates the lasers
        public void Update()
        {
            laserPos.Y -= laserSpeed;
            HasExited();
        }

        //draws the lasers
        public void Draw(SpriteBatch sBatch)
        {
            if (isActive)
                sBatch.Draw(playerLaserTex, laserPos, Color.DarkSalmon);
        }
    }
       #endregion
}
