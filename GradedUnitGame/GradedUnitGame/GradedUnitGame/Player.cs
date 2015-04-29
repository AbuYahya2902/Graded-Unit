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
    /// this is the class for the player and its variables
    /// </summary>
    class Player
    {
        #region attributes
        //current position of the player
        public Vector2 playerCoords;

        //movement of the player
        Vector2 movement;

        //screen boundary to detect collision
        Rectangle screenBoundary;

        //boolean storing if player is currently alive; used to determine gameover
        public bool isAlive;

        //current level of the player's shield between 3 and 0, if this reaches 0, the player shield is down
        public int playerShield;

        //tracks the players current score
        public int playerScore;

        //texture for the player
        Texture2D playerSprite;

        //variable holding player speed
        float playerSpeed;
        #endregion

        #region getters
        //gets players current position
        public Vector2 GetPCoords()
        {
            return this.playerCoords;
        }

        //sets players score
        public void AddScore(int playerScore)
        {
            this.playerScore += playerScore;
        }

        //gets players boundary for collision detection
        public Rectangle GetBoundary()
        {
            return new Rectangle((int)playerCoords.X, (int)playerCoords.Y, playerSprite.Width, playerSprite.Height);
        }
        #endregion

        #region initilization
        //contructor, sets initial values for all variables
        public Player(Vector2 position, Texture2D playerSprite, Rectangle screenBoundary)
        {
            this.screenBoundary = screenBoundary;
            this.playerSprite = playerSprite;
            playerSpeed = 1.55f;
            playerCoords = position;
            isAlive = true;
            playerShield = 2;
            playerScore = 0;
        }
        #endregion

        #region movement
        //handles moving the player left
        public void MovePlayerLeft()
        {
            movement = Vector2.Zero;
            movement.X = -1;
            movement.X *= playerSpeed;
            playerCoords += movement;
            ScrBoundaryCheck();
        }

        //handles moving the player right
        public void MovePlayerRight()
        {
            movement = Vector2.Zero;
            movement.X = +1;
            movement.X *= playerSpeed;
            playerCoords += movement;
            ScrBoundaryCheck();
        }
        #endregion

        #region update & draw
        //checks if the player has moved offscreen or not
        public void ScrBoundaryCheck()
        {
            //stops player from going off the lefthand side of the screen
            if (playerCoords.X < 0)
            {
                playerCoords.X = 0;
            }
            //stops player from going off the righthand side of the screen
            if (playerCoords.X + playerSprite.Width > screenBoundary.Width)
            {
                playerCoords.X = screenBoundary.Width - playerSprite.Width;
            }
        }

        //draws the player on screen
        public void Draw(SpriteBatch sBatch)
        {
            sBatch.Draw(playerSprite, playerCoords, Color.White);
        }
        #endregion
    }
}
