#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GradedUnitGame
{
    class MsgboxScreen : GameScreen
    {
        #region attributes
        string message;
        Texture2D gradientTexture;
        #endregion

        //event flags for the messagebox
        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

       
        public MsgboxScreen(string message): this(message, true)
        { }

        //constructor for the messagebox
        public MsgboxScreen(string message, bool includeUsageText)
        {
            const string usageText = "\nA button, Space/Enter = Yes" +
                                     "\nB button, Esc = Cancel"; 
            
            if (includeUsageText)
                this.message = message + usageText;
            else
                this.message = message;

            IsPopup = true;

            TransOnTime = TimeSpan.FromSeconds(0.2);
            TransOffTime = TimeSpan.FromSeconds(0.2);
        }


        //loads content, called once per screen
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            gradientTexture = content.Load<Texture2D>("./UI Misc/gradient");
        }

        
        //accepts userinput, accepting or cancelling  
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ConPlayer, out playerIndex))
            {
                //raise accepted event then exit messagebox
                if (Accepted != null)
                {
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                }
                Exit();
            }
            else if (input.IsMenuCancel(ConPlayer, out playerIndex))
            {
                //raise cancelled event then exit messagebox
                if (Cancelled != null)
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));

                Exit();
            }
        }
        #region draw
        
        //draws message box
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            //darken any screens beneath popup
            ScreenManager.FadeBackBufferToBlack(TransAlpha * 2 / 3);

            //center the message text in the screen
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2 (viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle ((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            //fade the popup during screen transitions
            Color colour = Color.White * TransAlpha;

            sBatch.Begin();

            //draw the background 
            sBatch.Draw(gradientTexture, backgroundRectangle, colour);

            //draw the messagebox text
            sBatch.DrawString(font, message, textPosition, colour);

            sBatch.End();
        }
        #endregion
    }
}


