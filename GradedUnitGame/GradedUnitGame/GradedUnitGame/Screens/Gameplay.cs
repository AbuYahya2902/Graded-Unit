#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace GradedUnitGame
{
    /// <summary>
    /// this is the class for the main gameplay, arcade mode
    /// </summary>
    class Gameplay : GameScreen
    {
        #region attributes
        ContentManager content;
        SpriteFont gameFont;

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
        Texture2D enemyLaserTex;
        Texture2D enemy1Sprite;
        Texture2D enemy2Sprite;
        Texture2D enemy3Sprite;
        Texture2D enemy4Sprite;
        Enemies[,] enemies;
        int scoreValue = 0;
        int enemyWidth = 10;
        int enemyHeight = 4;

        float pauseAlpha;
        #endregion

        #region Initilization
        public Gameplay()
        {
            TransOnTime = TimeSpan.FromSeconds(1.5);
            TransOffTime = TimeSpan.FromSeconds(0.5);
        }

        //initializes attributes
        public void Initialize()
        {

            //sets time keeper to 0
            prevFireTime = TimeSpan.Zero;

            //sets the laser to fire every .25 of a second
            fireDelay = TimeSpan.FromSeconds(.25f);
        }


        //loads content, called once per game
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            //load the game background
            bgTex = this.content.Load<Texture2D>("./UI Misc/earth");

            //load screen boundary for collision detection
            screenBoundary = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);

            //load game font
            gameFont = content.Load<SpriteFont>("./UI Misc/MainFont");

            //load player resources
            Texture2D playerSprite = this.content.Load<Texture2D>("./Players/Player1");
            laserTex = this.content.Load<Texture2D>("./Players/Player Laser");
            Vector2 playerCoords = new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Y + ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Width / 2);
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
            enemyLaserTex = this.content.Load<Texture2D>("./Mobs/Mob Laser");

            //add enemies
            AddEnemy();

            //add lasers
            lasers = new Lasers(laserTex, screenBoundary);

            //reset time so it doesnt try to catch up
            ScreenManager.Game.ResetElapsedTime();
        }

        //adds the enemies
        private void AddEnemy()
        {
            enemies = new Enemies[enemyWidth, enemyHeight];

            for (int e = 0; e < enemyHeight; e++)
            {
                Texture2D enemySprite = enemy1Sprite;
                int score = scoreValue;

                switch(e)
                {
                    case 0:
                        enemySprite = enemy2Sprite;
                        score = 75;
                        break;
                    case 1:
                        enemySprite = enemy1Sprite;
                        score = 50;
                        break;
                    case 2:
                        enemySprite = enemy3Sprite;
                        score = 25;
                        break;
                    case 3:
                        enemySprite = enemy4Sprite;
                        score = 15;
                        break;
                }
                for (int i = 0; i<enemyWidth; i++)
                {
                    enemies[i, e] = new Enemies(enemySprite, new Rectangle(i *  enemySprite.Width, e * enemySprite.Height, enemySprite.Width, enemySprite.Height), score);
                }
            }

            
        }

        //adds the lasers
        private void AddLaser()
        {
            if (!lasers.ifisActive())
            lasers.Fire(player.GetBoundary());
            
        }

        //unloads the content, this is called once per game end
        public override void UnloadContent()
        {
            content.Unload();
        }
        #endregion

        #region Update&Draw
        private void UpdateEnemies(GameTime gameTime)
        {

        }

        //updates the game attributes
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // fade in or out if covered by pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            lasers.UpdatePosition();

        }

        //handles the player input
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            //looks up input for player
            int playerIndex = (int)ConPlayer.Value;

            KeyboardState keys = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePad = input.CurrentGamePadStates[playerIndex];

            //pauses if user presses pause, of if gamepad becomes disconnected
            //this means game has to track if a gamepad was originally connected
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

                // draw and move the laser
               
               if (input.isFired(ConPlayer))
              {
                  AddLaser();
              }

                //play player-laser sound only when player is firing a laser
                //   if (laser.isActive)
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
            sBatch.Draw(bgTex, fullscreen, Color.White);
            player.Draw(sBatch);
            foreach (Enemies enemy in enemies)
                enemy.draw(sBatch);
            lasers.Draw(sBatch);

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
        #endregion



