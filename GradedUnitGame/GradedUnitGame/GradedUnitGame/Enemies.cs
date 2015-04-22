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
        //current position of the enemies
        public Rectangle enemyPos;

        //sprite for the enemy
        Texture2D enemySprite;

        //current state of enemy
        public bool isAlive;

        //how many scorepoints the enemy is worth
        public int scoreValue;

        //sets initial values for all variables
        public Enemies(Texture2D sprite, Rectangle position, int scoreValue)
        {
            enemySprite = sprite;
            this.scoreValue = scoreValue;
            enemyPos = position;
            isAlive = true;
        }

        public void draw(SpriteBatch sBatch)
        {
            if (isAlive)
            {
                sBatch.Draw(enemySprite, enemyPos, Color.White);
            }
        }
    }
}
