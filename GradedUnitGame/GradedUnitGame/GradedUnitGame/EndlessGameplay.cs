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
    /// this is the class for the endless mode gameplay
    /// </summary>
    class EndlessGameplay : GameScreen
    {
        ContentManager content;
        SpriteFont gameFont;
        
        //boss textures
        Texture2D bossSprite;
        Texture2D bossLaserTex;
        //boss current position
        Vector2 bossPos = new Vector2(100, 390);
        //holds the boss's current hp
        int bossCurrentHP = 100;

        //image used for the game background
        Texture2D bgTex;

        //representation of the player
        Player player;

        //screen-boundary collision for the player
        Rectangle screenBoundary;

        //sounds used in-game
        SoundEffect gameMusic;
        SoundEffectInstance gameMusicInstance;
        SoundEffect playerLaserSound;
        SoundEffectInstance playerLaserSoundInstance;
        SoundEffect enemyLaserSound;
        SoundEffect explosions;

        //lasers
        Texture2D laserTex;
        Lasers lasers;

        //fire rate of the player
        TimeSpan fireDelay;
        TimeSpan prevFireTime;

        //enemies
        Texture2D enemy1Sprite;
        Texture2D enemy2Sprite;
        Texture2D enemy3Sprite;
        Texture2D enemy4Sprite;
        Enemies[,] enemies;
        int scoreValue = 0;
        int enemyWidth = 10;
        int enemyHeight = 4;
        float pauseAlpha;

        
        public EndlessGameplay()
        {
            TransOnTime = TimeSpan.FromSeconds(1.5);
            TransOffTime = TimeSpan.FromSeconds(0.5);
        }

        //sets initial values for all variables
        public void Initialize()
        {
            //sets time keeper to 0
            prevFireTime = TimeSpan.Zero;

            //sets the laser to fire every .25 of a second
            fireDelay = TimeSpan.FromSeconds(.25f);
        }

        //loads content, this is called once per game
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //load game font
            gameFont = content.Load<SpriteFont>("./UI Misc/MainFont");

            //load player resources
            Texture2D playerSprite = this.content.Load<Texture2D>("./Players/Player1");
            laserTex = this.content.Load<Texture2D>("./Players/Player Laser");
            Vector2 playerCoords = new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y + ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            player = new Player(playerCoords, playerSprite, screenBoundary);

            //load sounds
            gameMusic = this.content.Load<SoundEffect>("./Sounds/DST-CryolithicBreak");
            playerLaserSound = this.content.Load<SoundEffect>("./Sounds/happypew");
            enemyLaserSound = this.content.Load<SoundEffect>("./Sounds/sadpew");
            explosions = this.content.Load<SoundEffect>("./Sounds/Explosion");

            //load enemy textures
            enemy1Sprite = this.content.Load<Texture2D>("./Mobs/Mob1");
            enemy2Sprite = this.content.Load<Texture2D>("./Mobs/Mob2");
            enemy3Sprite = this.content.Load<Texture2D>("./Mobs/Mob3");
            enemy4Sprite = this.content.Load<Texture2D>("./Mobs/Mob4");
            laserTex = this.content.Load<Texture2D>("./Mobs/Mob Laser");

            //load boss texture
            bossSprite = this.content.Load<Texture2D>("./Mobs/Boss");
            bossLaserTex = this.content.Load<Texture2D>("./Mobs/Boss Laser");

            //load the game background
            bgTex = this.content.Load<Texture2D>("./UI Misc/earth");

            //reset time so it doesnt try to catch up
            ScreenManager.Game.ResetElapsedTime();
        }

        //adds the enemies
        private void AddEnemy()
        {

            
        }

        //adds the lasers
        private void AddLaser()
        {
            lasers = new Lasers(laserTex, screenBoundary);
        }

        //updates the enemies
        private void UpdateEnemies(GameTime gameTime)
        {

        }

        //updates the boss
        public void UpdateBoss(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            bossCurrentHP = (int)MathHelper.Clamp(bossCurrentHP, 0, 100);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            //fade in or out if covered by pause screen
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
        }

        //unloads content, called once per game end
        public override void UnloadContent()
        {
            content.Unload();
        }

        
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            //look up inputs for current player
            int playerIndex = (int)ConPlayer.Value;

            KeyboardState keys = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePad = input.CurrentGamePadStates[playerIndex];

           //pauses game if player presses pause, or if gamepad is disconnected
           //this means the game must keep track of if a gamepad was originally connected or not
            bool gamePadDisconnected = !gamePad.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ConPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseScreen(), ConPlayer);
            }
            else
            {
                //move the player 
                Vector2 movement = Vector2.Zero;

                if (keys.IsKeyDown(Keys.Left) || keys.IsKeyDown(Keys.A))
                {
                    player.movePlayerLeft();
                }

                if (keys.IsKeyDown(Keys.Right) || keys.IsKeyDown(Keys.D))
                {
                    player.movePlayerRight();
                }

                Vector2 thumbstick = gamePad.ThumbSticks.Left;
                movement.X += thumbstick.X;

                if (movement.Length() > 1)
                    movement.Normalize();

                player.playerCoords += movement * 2;

                //draw and move the laser
             //   if (keys.IsKeyDown(Keys.Space) || keys.IsKeyDown(Keys.F) || keys.IsKeyDown(Keys.NumPad0) && laser.ifisActive() == false)
           //     {
              //      laser.isActive = true;

              //      laser.laserPos = player.playerCoords;
                    //laserCoordinates.X += 20;
                    //laserCoordinates.Y -= 20;
           //     }

                //play player-laser sound only when player is firing a laser
                //   if (laser.isActive == true)
                //     {
                //        if (playerLaserSoundInstance.State == SoundState.Stopped)
                //        {
                //          playerLaserSoundInstance.IsLooped = true;
                //            playerLaserSoundInstance.Play();
                //      }
                //       else
                //         playerLaserSoundInstance.Resume();
                //  }
                //   else if (laser.isActive == false)
                //    {
                //        if (playerLaserSoundInstance.State == SoundState.Playing)
                //             playerLaserSoundInstance.Pause();
                //   }
            }
        }


        //draws the sprites onto the game screen
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            sBatch.Begin();
            sBatch.Draw(bossSprite, bossPos, Color.White);
            sBatch.Draw(bgTex, fullscreen, Color.White);

            //  foreach (Lasers l in PlayerLaserList)
            //     l.DrawLaser(sBatch);

            sBatch.End();

            if (TransPos > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
                base.Draw(gameTime);
            }

        }
    }
}
