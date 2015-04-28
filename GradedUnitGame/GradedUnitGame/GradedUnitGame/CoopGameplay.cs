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
    /// this is the class for the co-op gameplay mode
    /// </summary>
    class CoopGameplay : GameScreen
    {
        #region attributes
        SpriteFont gameFont;
        ContentManager content;

        //screen-boundary collision for the player
        Rectangle screenBoundary;

        //image used for the game background
        Texture2D bgTex;

        //representation of the player
        Player player;

        //representation for player two
        Player player2;

        //texture for player two
        Texture2D player2Sprite;

        //current position for player two
        Vector2 player2Pos = new Vector2(500, 425);

        //boss textures
        Texture2D bossSprite;
        Texture2D bossLaserTex;
        //boss current position
        Vector2 bossPos = new Vector2(350, 40);
        //holds the boss's current hp
        int bossCurrentHP = 100;

        //sounds used in-game
        SoundEffect gameMusic;
        SoundEffectInstance gameMusicInstance;
        SoundEffect playerLaserSound;
        SoundEffect enemyLaserSound;
        SoundEffect explosions;

        //lasers
        Texture2D laserTex;
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
        int enemyWidth = 10;
        int enemyHeight = 4;

        DatabaseInt dataInt;

        float pauseAlpha;
        #endregion


        public CoopGameplay()
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

        //loads the content, this is called once per game 
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //load the game background
            bgTex = this.content.Load<Texture2D>("./UI Misc/earth");

            //load game font
            gameFont = content.Load<SpriteFont>("./UI Misc/MainFont");

            //load screen boundary for collision detection
            screenBoundary = new Rectangle(0, 0, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);

            //load player resources
            Texture2D playerSprite = this.content.Load<Texture2D>("./Players/Player1");
            laserTex = this.content.Load<Texture2D>("./Players/Player Laser");
            Vector2 playerCoords = new Vector2(300, 425);
            player = new Player(playerCoords, playerSprite, screenBoundary);

            //load player 2 resources
            player2Sprite = this.content.Load<Texture2D>("./Players/Player2");
            player2 = new Player(player2Pos, player2Sprite, screenBoundary);
          
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

            //load boss texture
            bossSprite = this.content.Load<Texture2D>("./Mobs/Boss");
            bossLaserTex = this.content.Load<Texture2D>("./Mobs/Boss Laser");
            //add enemies
            AddEnemy();

            //add lasers
            playerLasers = new Lasers(laserTex, screenBoundary);

            //plays music
            PlaySound();

            //reset time so it doesnt try to catch up
            ScreenManager.Game.ResetElapsedTime();
        }

        //plays game music if its not already playing
        private void PlaySound()
        {
            if (gameMusicInstance.State == SoundState.Stopped)
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

                switch (e)
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
                for (int i = 0; i < enemyWidth; i++)
                {
                    enemies[i, e] = new Enemies(enemySprite, new Rectangle((i + 5) * enemySprite.Width, (e + 1) * enemySprite.Height, enemySprite.Width, enemySprite.Height), score);
                }
            }
        }

        //adds the lasers
        private void AddLaser()
        {
            if (!playerLasers.ifisActive())
                playerLasers.Fire(player.getBoundary());

            //play player-laser sound only when player is firing a laser
            if (playerLasers.ifisActive())
            {
                playerLaserSound.Play();
            }

        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            playerLasers.UpdatePosition();

            foreach (Enemies enemy in enemies)
            {
                enemy.CollisionCheck(playerLasers, player);
            }
            //todo UpdateEnemies();
            //todo UpdateCollision();
            //todo check if player.isAlive == false, then end game + prompt user to enter name + call dataInt.WriteDatabase()

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


       public void UpdateEnemies()
       {
           //todo: make enemies move and attack
           //enemies move left until they hit screen bounds, then move down and then move right until screen bounds
           //down then left, repeat
       }

       //updates the boss
       public void UpdateBoss(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
       {
           bossCurrentHP = (int)MathHelper.Clamp(bossCurrentHP, 0, 100);
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
            //display player current score
            sBatch.DrawString(gameFont, "Score: " + player.playerScore, new Vector2(10, 1), Color.HotPink);
            //display players current shields
            sBatch.DrawString(gameFont, "Shields: " + player.playerShield, new Vector2(690, 1), Color.HotPink);

            //draws player 1
            player.Draw(sBatch);
            //draws player 2
            player2.Draw(sBatch);
            //draws the array of enemies
            foreach (Enemies enemy in enemies)
                enemy.draw(sBatch);
            //draws the player lasers
            playerLasers.Draw(sBatch);
            //draws the boss
            sBatch.Draw(bossSprite, bossPos, Color.White);
            //draws the enemies lasers
            //draws the boss laser

            //todo: end game screen + displayscores
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
    

