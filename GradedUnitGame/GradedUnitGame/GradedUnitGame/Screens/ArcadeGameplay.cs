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

        //holds if the highscore has been written, to stop spam
        bool hasBeenWritten;

        //representation of the player
        Player player;
        Texture2D playerSprite;
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

        //enemy textures 
        Texture2D enemyLaserTex;
        Texture2D enemy1Sprite;
        Texture2D enemy2Sprite;
        Texture2D enemy3Sprite;
        Texture2D enemy4Sprite;
        Texture2D bossSprite;
        Texture2D bossLaserTex;

        //creates the lasers for the enemies and creates the enemies as an array
        Lasers enemyLasers;
        Enemies[,] enemies;

        //holds how many points each enemy is worth
        int scoreValue = 0;

        //holds how many enemies are spawned 
        int enemyWidth = 11; 
        int enemyHeight = 4; 

        //holds the 
        int enemiesKilled = 0;
        int wavesKilled;

        //holds if the boss is currently alive and its current hp 
        bool isBossAlive;
        int bossHealth;

        //holds the gamemode for writing to the database
        string mode; 

        //holds the alpha for fading to black during pause screen
        float pauseAlpha;
        #endregion

        #region Initilization
        //constructor for this game mode
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
            //set mode
            mode = "Arcade";

       
            //load screen boundary for collision detection
            screenBoundary = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);

            //loads a new database int for writing to database
            databaseInt = new DatabaseInt();

            //load game font
            gameFont = content.Load<SpriteFont>("./UI Misc/MainFont");

            //load player resources
            if(OptionsScreen.currentShip == OptionsScreen.playerShip.One)
            playerSprite = this.content.Load<Texture2D>("./Players/Player1");
            if (OptionsScreen.currentShip == OptionsScreen.playerShip.Two)
                playerSprite = this.content.Load<Texture2D>("./Players/Player2");
            if (OptionsScreen.currentShip == OptionsScreen.playerShip.Three)
                playerSprite = this.content.Load<Texture2D>("./Players/Player3");

            playerLaserTex = this.content.Load<Texture2D>("./Players/Player Laser");
            Vector2 playerCoords = new Vector2(400, 425);
            player = new Player(playerCoords, playerSprite, screenBoundary);

            //initialises boss health, enemies killed and waves killed counters
            enemiesKilled = 0;
            wavesKilled = 0;
            bossHealth = 1; 

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
            bossSprite = this.content.Load<Texture2D>("./Mobs/Boss");
            bossLaserTex = this.content.Load<Texture2D>("./Mobs/Boss Laser");

            //add enemies
            AddEnemy();

            //initialises hasbeenwritten
            hasBeenWritten = false; 

            //add lasers
            playerLasers = new Lasers(playerLaserTex, screenBoundary);
            bosslaser = new Lasers(bossLaserTex, screenBoundary);
            enemyLasers = new Lasers(enemyLaserTex, screenBoundary);

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
            foreach (Enemies enemy in enemies)
            {

                // enemy.SetPosition(new Vector2((enemy1Sprite.Width + (i * 50)), (i * enemy1Sprite.Height)));

                enemy.SetMotion(new Vector2(1, 0));
                AddEnemy();
                enemiesKilled = 0;
            }
            //increase counter 
            wavesKilled += 1;
        }

        //adds the boss to the game screen
        private void addBoss()
        {
            boss = new Boss(bossSprite, new Vector2(100, 100), 100, screenBoundary, 8);
            bossHealth = boss.GetHealth();
            Debug.WriteLine(bossHealth);
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
        //updates the game attributes
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

         
             if (IsActive)
             {
                    //if player shield drops to 0, player is considered dead
                     if(player.playerShield <=0)
                     {
                         player.isAlive = false;
                     }
                 
                 if (player.playerShield > 0 || bossHealth > 0)
                 {
                     foreach (Enemies enemy in enemies)
                     {
                         enemiesKilled += enemy.CollisionCheck(playerLasers, player);
                         if(enemy.enemyPos.X == player.playerCoords.X&& enemy.isAlive)
                         {
                             enemyLasers = new Lasers(enemyLaserTex, screenBoundary);
                             enemyLasers.FireEnemy(enemy.getBoundary());
                             Debug.WriteLine("firesLaser");
                             enemy.setFired(true);
                         }
                         //moves enemies
                         enemy.MoveEnemies();
                         //updates laser position
                         playerLasers.UpdatePosition();
                         enemyLasers.UpdateEnemyPosition();
                         //checks for player collision with enemy lasers
                         player.CollisionCheck(enemyLasers);
                     }

                     //if all enemies are dead, reset them
                     if (enemiesKilled == (enemyWidth * enemyHeight) && wavesKilled < 1)
                     { ResetEnemies(); }
                     if (wavesKilled >= 1 && !isBossAlive)
                     {
                         addBoss();
                         isBossAlive = true;
                     }
                     //if boss is up, handle it
                     if (isBossAlive && bossHealth >0)
                     {
                         boss.MoveEnemies();
                         bosslaser.UpdateEnemyPosition();
                         bossHealth -= boss.CollisionCheckBoss(playerLasers, player);
                         Debug.WriteLine("1 "+bossHealth);
                         boss.SetIsAlive(true);
                         boss.SetHealth(bossHealth);
                         Debug.WriteLine("2 "+ bossHealth);
                         if( (boss.enemyPos.X == player.playerCoords.X))
                         {
                             bosslaser = new Lasers(bossLaserTex, screenBoundary);
                             bosslaser.FireEnemy(boss.getBoundary());
                             Debug.WriteLine("firing Boss LASER");
                         }
                     }
                         //if palyer defeats boss, write highscore  
                         if (!hasBeenWritten && bossHealth <= 0 )
                         {
                             player.playerScore += boss.GetScore();
                             WriteDb();
                             foreach (Enemies enemy in enemies)
                             {
                                 enemy.isAlive = false;
                             }
                         }
                         //if player is dead, write highscore
                        if(player.playerShield<=0 && !hasBeenWritten)
                        {
                            WriteDb();
                            foreach (Enemies enemy in enemies)
                            {
                                enemy.isAlive = false;
                            }
                        }
                 }
                
                 
                 
             }

            // fade in or out if covered by pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
        }


        //writes the high score to the database
        private void WriteDb()
        {
            //uses visual basic input box to prompt user for their name
            string playerName = Microsoft.VisualBasic.Interaction.InputBox("Name", "Please Enter Your Name", "").ToString();

            if (playerName.Length > 10)
            { playerName = playerName.Substring(0, 10); }

            databaseInt.AddNameDB(playerName);
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

            //draws boss laser
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
            //if boss is alive, draw on screen
            if (isBossAlive && bossHealth >0)
            {
                boss.Draw(sBatch);
            }
            //if boss is dead, display victory & prompt player for name
            if(bossHealth <=0 )
            {
                sBatch.DrawString(gameFont, "Victory!", new Vector2(viewport.Width / 2, viewport.Height / 2), Color.Coral);
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



