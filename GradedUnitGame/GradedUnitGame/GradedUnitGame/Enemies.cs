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
    /// this is the main class for the enemies and their variables
    /// </summary>
    class Enemies
    {
        #region attributes
        //current position of the enemies
        public Vector2 enemyPos;

        //sprite for the enemy
        Texture2D enemySprite;

        //current state of enemy
        public bool isAlive;

        //how many scorepoints the enemy is worth
        public int scoreValue;

        public Vector2 enemyMotion;

        public float enemySpeed = 2f;
        public Rectangle boundary;

        Rectangle screenBoundary;

        

        int enemiesKilled;

        #endregion

        #region initilization
        //sets initial values for all variables
        public Enemies(Texture2D sprite, Vector2 enemyPos, int scoreValue, Rectangle screenBoundary)
        {
            enemySprite = sprite;
            this.scoreValue = scoreValue;
            this.enemyPos = enemyPos;
            isAlive = true;
            boundary.X = (int)enemyPos.X;
            boundary.Y = (int)enemyPos.Y;
            this.screenBoundary = screenBoundary;
            enemyMotion = new Vector2(1, 0);
        }

        public void setMotion(Vector2 enemyMotion)
        {
            this.enemyMotion = enemyMotion;
        }

        public void setEnemiesKilled(int enemiesKilled)
        {
            this.enemiesKilled = enemiesKilled;
        }

        #endregion

        #region draw&update
        public void draw(SpriteBatch sBatch)
        {
            if (isAlive)
            {
                sBatch.Draw(enemySprite, enemyPos, Color.White);
            }
        }
        public bool getIsAlive()
        {
            return isAlive;
        }
        public void CollisionCheck(Lasers laser, Player player)
        {
            if(isAlive && laser.Boundary.Intersects(boundary) && laser.ifisActive() )
            {
                isAlive = false;
                laser.setisActive(false);
                player.AddScore(this.scoreValue);
                enemiesKilled += 1;
            }
        }

        public bool CollisionCheckWall()
        {
            if ((enemyPos.X) <= 0 || enemyPos.X + enemySprite.Width >= screenBoundary.Width)
            {
                return true;
            }
            return false;
        }

        public void moveEnemies()
        {
            enemyPos += (enemyMotion * enemySpeed);
            //Debug.WriteLine(enemyPos);
            if ((enemyPos.X) <= 0 || enemyPos.X + enemySprite.Width >= screenBoundary.Width)
            {
                enemyPos.Y += enemySpeed * 10;
                enemyMotion.X *= -1;

            }
            boundary = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, enemySprite.Width, enemySprite.Height);
        }

        public void moveEnemiesdl()
        {
            enemyPos.Y += enemySpeed;
            enemyMotion.X *= -1;
            boundary = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, enemySprite.Width, enemySprite.Height);
        }

        public void setPosition(Vector2 enemyPos)
        {
            this.enemyPos = enemyPos;
            boundary = boundary = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, enemySprite.Width, enemySprite.Height);

        }
    }
#endregion
}
