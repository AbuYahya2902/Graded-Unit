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


        bool hasBeenWritten;
        //representation of the player
        Player player;
        Boss boss;
        Lasers bosslaser; 
        //screen-boundary collision for the player
        Rectangle screenBoundary;

        DatabaseInt databaseInt;

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
        Texture2D bossSprite;
        Texture2D bossLaserTex;
        Lasers enemyLasers;
        Enemies[,] enemies;
        int scoreValue = 0;
        int enemyWidth = 11; //11
        int enemiesKilled = 0;
        int enemyHeight = 4; //4
        int waveskilled;
        bool isBossAlive;
        float pauseAlpha;
        int bosshealth;
        string mode;
        int playerinvinc;
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
            mode = "Arcade";

       
            //load screen boundary for collision detection
            screenBoundary = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);

            databaseInt = new DatabaseInt();

            //load game font
            gameFont = content.Load<SpriteFont>("./UI Misc/MainFont");

            //load player resources
            Texture2D playerSprite = this.content.Load<Texture2D>("./Players/Player1");
            playerLaserTex = this.content.Load<Texture2D>("./Players/Player Laser");
            Vector2 playerCoords = new Vector2(400, 425);
            player = new Player(playerCoords, playerSprite, screenBoundary);
            enemiesKilled = 0;
            waveskilled = 0;
            //load sounds
            bosshealth = 1; 
            gameMusic = this.content.Load<SoundEffect>("./Sounds/DST-CryolithicBreak");
            gameMusicInstance = gameMusic.CreateInstance();

            playerLaserSound = this.content.Load<SoundEffect>("./Sounds/happypew");
            enemyLaserSound = this.content.Load<SoundEffect>("./Sounds/sadpew");
            explosions = this.content.Load<SoundEffect>("./Sounds/Explosion");

            playerinvinc = 100;
            //load enemy textures
            enemy1Sprite = this.content.Load<Texture2D>("./Mobs/Mob1");
            enemy2Sprite = this.content.Load<Texture2D>("./Mobs/Mob2");
            enemy3Sprite = this.content.Load<Texture2D>("./Mobs/Mob3");
            enemy4Sprite = this.content.Load<Texture2D>("./Mobs/Mob4");
            enemyLaserTex = this.content.Load<Texture2D>("./Mobs/Mob Laser");
            bossSprite = this.content.Load<Texture2D>("./Mobs/Boss");
            bossLaserTex = this.content.Load<Texture2D>("./Mobs/Boss Laser");
            //add enemies
            AddEnemy();

            hasBeenWritten = false; 
            //add lasers
            playerLasers = new Lasers(playerLaserTex, screenBoundary);
            bosslaser = new Lasers(bossLaserTex, screenBoundary);
            enemyLasers = new Lasers(enemyLaserTex, screenBoundary);//plays sound
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
            foreach (Enemies enemy in enemies)
            {

                // enemy.SetPosition(new Vector2((enemy1Sprite.Width + (i * 50)), (i * enemy1Sprite.Height)));

                enemy.SetMotion(new Vector2(1, 0));
                AddEnemy();
                enemiesKilled = 0;
            }
            waveskilled += 1;
        }


        private void addBoss()
        {
            boss = new Boss(bossSprite, new Vector2(100, 100), 100, screenBoundary, 8);
            bosshealth = boss.getHealth();
            Debug.WriteLine(bosshealth);
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

        private void addEnemyLaser()
        {
                
        }
        //unloads the content, this is called once per game end
        public override void UnloadContent()
        {
            content.Unload();
        }
        #endregion

        #region Update&Draw
        //updates the game attributes
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

             bool wall = false;
            //todo UpdateEnemies();
            //todo call UpdateCollision();
            //todo check if player.isAlive == false, then end game + prompt user to enter name + call dataInt.WriteDatabase()
             if (IsActive)
             {
                 //if(player.isAlive)
                 //{
                 playerinvinc -= 1;
                     if(player.playerShield <=0)
                     {
                         player.isAlive = false;
                     }
                 
                 if (player.playerShield > 0 || bosshealth > 0)
                 {
                     foreach (Enemies enemy in enemies)
                     {
                         enemiesKilled += enemy.CollisionCheck(playerLasers, player);
                        // Debug.WriteLine("Enemy Killed");
                        //enemyLasers.isActive = false;
                        // Debug.WriteLine("Creates Laser");
                         if(enemy.enemyPos.X == player.playerCoords.X&& enemy.isAlive)
                         {
                             enemyLasers = new Lasers(enemyLaserTex, screenBoundary);
                             enemyLasers.FireEnemy(enemy.getBoundary());
                             Debug.WriteLine("firesLaser");
                             enemy.setFired(true);
                            
                              
                            
                         }

                        
                         wall = enemy.CollisionCheckWall();
                         enemy.MoveEnemies();
                         playerLasers.UpdatePosition();
                         enemyLasers.UpdateEnemyPosition();
                         player.CollisionCheck(enemyLasers);
                         
                         
                         
                     }

                     if (enemiesKilled == (enemyWidth * enemyHeight) && waveskilled < 1)
                     { ResetEnemies(); }
                     if (waveskilled >= 1 && !isBossAlive)
                     {
                         addBoss();
                         isBossAlive = true;
                     }
                     if (isBossAlive&& bosshealth >0)
                     {
                         boss.MoveEnemies();
                         bosslaser.UpdateEnemyPosition();
                         bosshealth -= boss.CollisionCheckBoss(playerLasers, player);
                         Debug.WriteLine("1 "+bosshealth);
                         boss.SetIsAlive(true);
                         boss.setHealth(bosshealth);
                         Debug.WriteLine("2 "+ bosshealth);
                         if( (boss.enemyPos.X == player.playerCoords.X))
                         {
                             bosslaser = new Lasers(bossLaserTex, screenBoundary);
                             bosslaser.FireEnemy(boss.getBoundary());
                             Debug.WriteLine("firing Boss LASER");
                         }
                     }
                         if (!hasBeenWritten && bosshealth <= 0 )
                         {
                             player.playerScore += boss.getScore();
                             writeDb();
                             foreach(Enemies enemy in enemies )
                             {
                                 enemy.isAlive = false;
                             }


                         }
                        if(player.playerShield<=0 &&!hasBeenWritten)
                        {
                            writeDb();
                            foreach (Enemies enemy in enemies)
                            {
                                enemy.isAlive = false;
                            }
                        }
                 }
                // }
                 
                 
             }
            // fade in or out if covered by pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
        }


        
        private void writeDb()
        {
            string username = Microsoft.VisualBasic.Interaction.InputBox("Name", "Please Enter Your Name", "").ToString();

            if (username.Length > 10)
            { username = username.Substring(0, 10); }

            databaseInt.AddNameDB(username);
            databaseInt.WriteDatabase(mode, player.playerScore);

            hasBeenWritten = true;
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
                if (enemyLasers.isActive)
                    enemyLasers.Draw(sBatch);

            //draws lasers
            playerLasers.Draw(sBatch);

            if (bosslaser.isActive)
                bosslaser.Draw(sBatch);
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
            if (isBossAlive &&bosshealth >0)
            {
                boss.Draw(sBatch);
            }
            if(bosshealth <=0 )
            {
                sBatch.DrawString(gameFont, "You Win", new Vector2(viewport.Width / 2, viewport.Height / 2), Color.Coral);
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



