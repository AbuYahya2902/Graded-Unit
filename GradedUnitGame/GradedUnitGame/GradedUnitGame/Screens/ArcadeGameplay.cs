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
using System.Diagnostics;
#endregion

namespace GradedUnitGame
{
    /// <summary>
    /// this is the class for the main gameplay, arcade mode
    /// </summary>
    class ArcadeGameplay : GameScreen
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
        SoundEffect enemyLaserSound;
        SoundEffect explosions;

        //lasers
        Texture2D playerLaserTex;
        Lasers playerLasers;

        //fire rate of the player
        TimeSpan fireDelay;
        TimeSpan prevFireTime;

        //enemies
        Texture2D enemyLaserTex;
        Texture2D enemy1Sprite;
        Texture2D enemy2Sprite;
        Texture2D enemy3Sprite;
        Texture2D enemy4Sprite;
        Lasers enemyLasers;
        Enemies[,] enemies;
        int scoreValue = 0;
        int enemyWidth = 1; //11
        int enemiesKilled = 0;
        int enemyHeight = 1; //4
        

        float pauseAlpha;
        #endregion

        #region Initilization
        public ArcadeGameplay()
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
            playerLaserTex = this.content.Load<Texture2D>("./Players/Player Laser");
            Vector2 playerCoords = new Vector2(400, 425);
            player = new Player(playerCoords, playerSprite, screenBoundary);
            enemiesKilled = 0; 
            
            //load sounds
            gameMusic = this.content.Load<SoundEffect>("./Sounds/DST-CryolithicBreak");
            gameMusicInstance = gameMusic.CreateInstance();

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
            playerLasers = new Lasers(playerLaserTex, screenBoundary);

            //plays sound
            PlaySound();

            //reset time so it doesnt try to catch up
            ScreenManager.Game.ResetElapsedTime();
        }

        //plays game music if its not already playing, unless music is disabled in menu
        private void PlaySound()
        {
            if (OptionsScreen.currentMusic == OptionsScreen.music.Off)
            {
                gameMusicInstance.Stop();
            }
            else if (OptionsScreen.currentMusic == OptionsScreen.music.On && gameMusicInstance.State == SoundState.Stopped)
            {
                gameMusicInstance.IsLooped = true;
                gameMusicInstance.Play();
            }
            else if (gameMusicInstance.State == SoundState.Playing)
            {
                gameMusicInstance.Pause();
            }
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
                    enemies[i, e] = new Enemies(enemySprite, new Vector2((i+5) * enemySprite.Width,  (e+1)* enemySprite.Height), score, screenBoundary);
                }
            } 
        }

        //resets the enemies when they are killed
        public void ResetEnemies()
        {
            int i = 0;
            foreach (Enemies enemy in enemies)
            {

               // enemy.SetPosition(new Vector2((enemy1Sprite.Width + (i * 50)), (i * enemy1Sprite.Height)));
              
               enemy.SetMotion(new Vector2(1, 0));
                AddEnemy();
               
                enemy.MoveEnemies();
                i++;

            }
        }

        //adds the lasers
        private void AddLaser()
        {
            if (!playerLasers.IfIsActive())
            playerLasers.Fire(player.GetBoundary());

            //play player-laser sound only when player is firing a laser
            if (playerLasers.IfIsActive())
            {
                playerLaserSound.Play();
            }
            
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
            //todo: make enemies move and attack
            //enemies move left until they hit screen bounds, then move down and then move right until screen bounds
            //down then left, repeat
        }

        //updates the game attributes
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            playerLasers.UpdatePosition();
             bool wall = false;
            //todo UpdateEnemies();
            //todo call UpdateCollision();
            //todo check if player.isAlive == false, then end game + prompt user to enter name + call dataInt.WriteDatabase()
            
            foreach(Enemies enemy in enemies)
            {
               enemiesKilled+= enemy.CollisionCheck(playerLasers, player);
               //Debug.WriteLine(enemiesKilled);
               
                wall = enemy.CollisionCheckWall();
                enemy.MoveEnemies();
            }

            for (int i = 1; i <= 10;i++ )
            { 
                if (enemiesKilled == i*(enemyWidth * enemyHeight))
                { ResetEnemies(); }
            }

            // fade in or out if covered by pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
        }

        private void UpdateCollision()
        {
            Rectangle enemyBox;
            Rectangle enemyLaserBox;
            Rectangle playerBox;
            Rectangle playerLaserBox;
        }

            // todo make rectangle for playerlaser position
            // if (playerLaserBox.Intersects(enemyBox)) then kill enemy
            // if (enemyLaserBox.Intersects(playerBox)) remove 1 charge from player shield
            //if  (enemyBox.Intersects(screenBoundary?????)) enemy collides wth bottom of screen, gg 

            //playerBox = new Rectangle((int)player.playerCoords.X, (int)player.playerCoords.Y, player.Width, player.Height);

            // Do the collision between the player and the enemies
            /*for (int i = 0; i < enemies.Count; i++)
            {
                enemyBox = new Rectangle((int)enemies[i].Position.X,
                (int)enemies[i].Position.Y,
                enemies[i].Width,
                enemies[i].Height);

                // Determine if the two objects collided with each
                // other
                if (playerBox.Intersects(enemyBox))
                {
                    //subtract 1 charge from player shie/d
                    player.playerShield -= 1;

                    // Since the enemy collided with the player
                    // destroy it
                    enemies[i].Health = 0;

                  
                }

            }*/
        

       
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
                    player.MovePlayerLeft();
                }

                if (keys.IsKeyDown(Keys.Right) || keys.IsKeyDown(Keys.D))
                {
                    player.MovePlayerRight();
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

               
            }
        }


        //draws the sprites onto the game screen
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            sBatch.Begin();
            //draws the game background
            sBatch.Draw(bgTex, fullscreen, Color.White);
            //draws the player
            player.Draw(sBatch);
            //draws the array of enemies
            foreach (Enemies enemy in enemies)
            {
                    enemy.Draw(sBatch);
            }
            //draws lasers
            playerLasers.Draw(sBatch);

            //display players current score
            sBatch.DrawString(gameFont, "Score: " + player.playerScore, new Vector2(10,1), Color.HotPink);
            //display players current shields
            sBatch.DrawString(gameFont, "Shields: " + player.playerShield, new Vector2(690, 1), Color.HotPink);

            //ends game screen + displays score
            if (player.isAlive == false)
            {
                sBatch.DrawString(gameFont, "Game Over!", new Vector2(viewport.Width / 2, viewport.Height / 2), Color.Coral);
                sBatch.DrawString(gameFont, "Final Score: " + player.playerScore, new Vector2(450, 300), Color.DeepPink);
            }
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



