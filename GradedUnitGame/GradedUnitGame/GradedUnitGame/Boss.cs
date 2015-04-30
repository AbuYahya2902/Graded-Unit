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

namespace GradedUnitGame
{
    class Boss:Enemies
    {
        int health;
        public Boss(Texture2D sprite, Vector2 enemyPos, int scoreValue, Rectangle screenBoundary,int health)
            : base(sprite,enemyPos,scoreValue,screenBoundary)
    {
        this.health = health; 
 
    }
        public int getHealth()
        {
            return health;
        }
        public void setHealth(int health)
        {
            this.health = health;
        }   
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
        public int getScore()
        {
            return this.scoreValue;
        }
        }
    }

