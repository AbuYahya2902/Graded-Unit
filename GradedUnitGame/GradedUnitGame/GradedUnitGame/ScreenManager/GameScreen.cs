#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GradedUnitGame
{
        public enum ScreenState
    {
        TransOn,
        Active,
        TransOff,
        Hidden,
    }


    /// <summary>
    /// A screen is a single layer that has update and draw logic, and which
    /// can be combined with other layers to build up a complex menu system.
    /// For instance the main menu, the options menu, the "are you sure you
    /// want to quit" message box, and the main game itself are all implemented
    /// as screens.
    /// </summary>
    public abstract class GameScreen
    {
        #region Properties
        /// <summary>
        /// indicates whether the screen is only a 
        /// popup, in which case screens underneath  do not need to transition off
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        bool isPopup = false;


        /// <summary>
        /// Indicates how long the screen takes to
        /// transition on when it is activated.
        /// </summary>
        public TimeSpan TransOnTime
        {
            get { return transOnTime; }
            protected set { transOnTime = value; }
        }

        TimeSpan transOnTime = TimeSpan.Zero;


        /// <summary>
        /// Indicates how long the screen takes to
        /// transition off when it is deactivated.
        /// </summary>
        public TimeSpan TransOffTime
        {
            get { return transOffTime; }
            protected set { transOffTime = value; }
        }

        TimeSpan transOffTime = TimeSpan.Zero;


        /// <summary>
        /// Gets current screen transition, ranging
        /// from 0 (fully active) to 1 (transitioned fully off)
        /// </summary>
        public float TransPos
        {
            get { return transPos; }
            protected set { transPos = value; }
        }

        float transPos = 1;


        /// <summary>
        /// Gets current alpha of the transition, ranging
        /// from 1 (fully active) to 0 (transitioned fully off)
        /// </summary>
        public float TransAlpha
        {
            get { return 1f - TransPos; }
        }


        /// <summary>
        /// Gets current screen transition state.
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        ScreenState screenState = ScreenState.TransOn;


        /// <summary>
        /// Indicates whether the screen is exiting
        /// if true, screen will automatically remove itself once transition finishes.
        /// </summary>
        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        bool isExiting = false;


        /// <summary>
        /// Checks whether this screen is active and can respond to user input.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                       (screenState == ScreenState.TransOn ||
                        screenState == ScreenState.Active);
            }
        }

        bool otherScreenHasFocus;


        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;


        /// <summary>
        /// Gets the index of the player currently controlling this screen,
        /// or null if it is accepting input from any player. This is used to lock
        /// the game to a specific player profile. 
        /// </summary>
        public PlayerIndex? ConPlayer
        {
            get { return conPlayer; }
            internal set { conPlayer = value; }
        }

        PlayerIndex? conPlayer;


        #endregion

        #region Initialization


        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public virtual void LoadContent() { }


        /// <summary>
        /// Unload content for the screen.
        /// </summary>
        public virtual void UnloadContent() { }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                // If the screen is going away to die, it should transition off.
                screenState = ScreenState.TransOff;

                if (!UpdateTrans(gameTime, transOffTime, 1))
                {
                    // When the transition finishes, remove the screen.
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                if (UpdateTrans(gameTime, transOffTime, 1))
                {
                    // Still busy transitioning.
                    screenState = ScreenState.TransOff;
                }
                else
                {
                    // Transition finished!
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if (UpdateTrans(gameTime, transOnTime, -1))
                {
                    // Still busy transitioning.
                    screenState = ScreenState.TransOn;
                }
                else
                {
                    // Transition finished!
                    screenState = ScreenState.Active;
                }
            }
        }


        /// <summary>
        /// Helper for updating the screen transition position.
        /// </summary>
        bool UpdateTrans(GameTime gameTime, TimeSpan time, int direction)
        {
            // How much should we move by?
            float transDelta;

            if (time == TimeSpan.Zero)
                transDelta = 1;
            else
                transDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            // Update the transition position.
            transPos += transDelta * direction;

            // Did we reach the end of the transition?
            if (((direction < 0) && (transPos <= 0)) ||
                ((direction > 0) && (transPos >= 1)))
            {
                transPos = MathHelper.Clamp(transPos, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }


        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public virtual void HandleInput(InputState input) { }


        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }

        public void Exit()
        {
            if (TransOffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                isExiting = true;
            }
        }

    }
}
        #endregion