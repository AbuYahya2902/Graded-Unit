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
using System.Diagnostics;
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

        public void SetMotion(Vector2 enemyMotion)
        {
            this.enemyMotion = enemyMotion;
        }

        public void SetEnemiesKilled(int enemiesKilled)
        {
            this.enemiesKilled = enemiesKilled;
        }

        #endregion

        #region draw&update
        public void Draw(SpriteBatch sBatch)
        {
            if (isAlive)
            {
                sBatch.Draw(enemySprite, enemyPos, Color.White);
            }
        }
        public bool GetIsAlive()
        {
            return isAlive;
        }
        public int CollisionCheck(Lasers laser, Player player)
        {
            if(isAlive && laser.Boundary.Intersects(boundary) && laser.IfsActive() )
            {
                isAlive = false;
                laser.SetIsActive(false);
                player.AddScore(this.scoreValue);
                return 1;
            }
            return 0; 
        }

        public bool CollisionCheckWall()
        {
            if ((enemyPos.X) <= 0 || enemyPos.X + enemySprite.Width >= screenBoundary.Width)
            {
                return true;
            }
            return false;
        }

        public void MoveEnemies()
        {
            enemyPos += (enemyMotion * enemySpeed);
            Debug.WriteLine(enemyMotion + enemyPos);
            //Debug.WriteLine(enemyPos);
            if ((enemyPos.X) <= 0 || enemyPos.X + enemySprite.Width >= screenBoundary.Width)
            {
                enemyPos.Y += enemySpeed * 10;
                enemyMotion.X *= -1;

            }
            boundary = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, enemySprite.Width, enemySprite.Height);
        }

        public void MoveEnemiesdl()
        {
            enemyPos.Y += enemySpeed;
            enemyMotion.X *= -1;
            boundary = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, enemySprite.Width, enemySprite.Height);
        }

        public void SetPosition(Vector2 enemyPos)
        {
            this.enemyPos = enemyPos;
            boundary = boundary = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, enemySprite.Width, enemySprite.Height);
            Debug.WriteLine(this.enemyPos.ToString() + this.isAlive.ToString()
            +this.enemyMotion);

        }
        public void SetIsAlive(bool IsAlive)
        {
            this.isAlive = IsAlive;
        }
    
    }
#endregion
}
