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
        string message;
        Texture2D gradientTexture;

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        /// <summary>
        /// Constructor 
        /// </summary>
        public MsgboxScreen(string message): this(message, true)
        { }


        /// <summary>
        /// Constructor for calling the msgbox
        /// </summary>
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


        /// <summary>
        /// Loads graphics content for this screen. 
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            gradientTexture = content.Load<Texture2D>("./UI Misc/gradient");
        }

        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ConPlayer, out playerIndex))
            {
                // Raise the accepted event, then exit the message box.
                if (Accepted != null)
                {
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                }
                Exit();
            }
            else if (input.IsMenuCancel(ConPlayer, out playerIndex))
            {
                // Raise the cancelled event, then exit the message box.
                if (Cancelled != null)
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));

                Exit();
            }
        }
            
        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Darken any other screens drawn beneath popup.
            ScreenManager.FadeBackBufferToBlack(TransAlpha * 2 / 3);

            // Center the message text in the viewport.
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

            // Fade the popup alpha during transitions.
            Color colour = Color.White * TransAlpha;

            sBatch.Begin();

            // Draw the background rectangle.
            sBatch.Draw(gradientTexture, backgroundRectangle, colour);

            // Draw the message box text.
            sBatch.DrawString(font, message, textPosition, colour);

            sBatch.End();
        }
}
}


