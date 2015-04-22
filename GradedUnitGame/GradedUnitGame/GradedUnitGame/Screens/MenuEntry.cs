#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace GradedUnitGame
{
    class MenuEntry
    {
        string text;
        float selFade;
        Vector2 pos;



        public string Text
        {
            get { return text; }
            set { text = value;  }
        }
        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        public event EventHandler<PlayerIndexEventArgs> Selected;
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }

        public MenuEntry(string Text)
        {
            this.text = Text;
        }

         public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selFade = Math.Min(selFade + fadeSpeed, 1);
            else
                selFade = Math.Max(selFade - fadeSpeed, 0);
        }

         public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            Color color = isSelected ? Color.Purple : Color.White;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;

            float pulsate = (float)Math.Sin(time * 6) + 1;

            float scale = 1 + pulsate * 0.05f * selFade;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransAlpha;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch sBatch = screenManager.SpriteBatch;
            SpriteFont font = screenManager.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            sBatch.DrawString(font, text, pos, color, 0,
                                   origin, scale, SpriteEffects.None, 0);
        }


         /// <summary>
         /// Queries how much space menu entry requires.
         /// </summary>
         public virtual int GetHeight(MenuScreen screen)
         {
             return screen.ScreenManager.Font.LineSpacing;
         }


         /// <summary>
         /// Queries how wide the entry is, used for centering on the screen.
         /// </summary>
         public virtual int GetWidth(MenuScreen screen)
         {
             return (int)screen.ScreenManager.Font.MeasureString(text).X;
         }

    }
}
