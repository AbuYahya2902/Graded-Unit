#region using statements
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
    class Boss:Enemies
    {
        //holds the boss hp
        int health;

        //constructor for boss
        public Boss(Texture2D sprite, Vector2 enemyPos, int scoreValue, Rectangle screenBoundary,int health)
            : base(sprite,enemyPos,scoreValue,screenBoundary)
         {
        this.health = health; 
        }

        #region getters&setters
        //gets boss hp
        public int GetHealth()
        {
            return health;
        }

        //sets boss hp
        public void SetHealth(int health)
        {
            this.health = health;
        }

        //gets score
        public int GetScore()
        {
            return this.scoreValue;
        }
        #endregion

        //checks for boss collision
        public int CollisionCheckBoss(Lasers laser, Player player)
        {
            if(isAlive && laser.Boundary.Intersects(boundary) && laser.IfIsActive() )
            {
                isAlive = false;
                laser.SetIsActive(false);
                return 1;
            }
            return 0; 
        }
        }
    }

