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
#region
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
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

    //a screen is a single layer with update and draw logic, and each screen can be combined to build up a menu system
    public abstract class GameScreen
    {
        #region attributes
        //indicates if screen is a popup or not
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        bool isPopup = false;
        #endregion

       

       #region getters & setters

        //indicates how long screen will take to transition on
        public TimeSpan TransOnTime
        {
            get { return transOnTime; }
            protected set { transOnTime = value; }
        }

        TimeSpan transOnTime = TimeSpan.Zero;


        //indicates how long it will the screen to transition off
        public TimeSpan TransOffTime
        {
            get { return transOffTime; }
            protected set { transOffTime = value; }
        }

        TimeSpan transOffTime = TimeSpan.Zero;


        //gets current screen state 
        public float TransPos
        {
            get { return transPos; }
            protected set { transPos = value; }
        }

        float transPos = 1;


        //gets current transition alpha
        public float TransAlpha
        {
            get { return 1f - TransPos; }
        }


        //gets current screen transition state
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        ScreenState screenState = ScreenState.TransOn;


        //indicates if screen is exiting; if true, screen will automatically remove itself
        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        bool isExiting = false;


        //indicates if screen is active and can accept userinput
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


        //gets screen manager
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;


        //gets the playerindex of the controlling player, or null if it will accept input from any
        public PlayerIndex? ConPlayer
        {
            get { return conPlayer; }
            internal set { conPlayer = value; }
        }

        PlayerIndex? conPlayer;


        #endregion

        #region initialization
        //loads graphics content
        public virtual void LoadContent() { }


        //unloads content
        public virtual void UnloadContent() { }
        #endregion

        #region update & draw
        //allows screen to run logic
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                //if screen is exiting, it will transition off
                screenState = ScreenState.TransOff;

                if (!UpdateTrans(gameTime, transOffTime, 1))
                {
                    //removes screen when transition is finished
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                //if screen is covered, transition off
                if (UpdateTrans(gameTime, transOffTime, 1))
                {
                   //it is busy transitioning
                    screenState = ScreenState.TransOff;
                }
                else
                {
                    //transition finished
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
                //else screen should transition on
                if (UpdateTrans(gameTime, transOnTime, -1))
                {
                    //it is busy transitioning
                    screenState = ScreenState.TransOn;
                }
                else
                {
                    //transition finished
                    screenState = ScreenState.Active;
                }
            }
        }


        //helper to update screen transition position
        bool UpdateTrans(GameTime gameTime, TimeSpan time, int direction)
        {
            //indicates how much to move by
            float transDelta;

            if (time == TimeSpan.Zero)
                transDelta = 1;
            else
                transDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            //updates position
            transPos += transDelta * direction;

            //indicates if the end of transition was reached
            if (((direction < 0) && (transPos <= 0)) ||
                ((direction > 0) && (transPos >= 1)))
            {
                transPos = MathHelper.Clamp(transPos, 0, 1);
                return false;
            }

            //else still transitioning
            return true;
        }


      //allows screen to handle user input, this is only called when screen is active
        public virtual void HandleInput(InputState input) { }


        //draws screen
        public virtual void Draw(GameTime gameTime) { }

        public void Exit()
        {
            if (TransOffTime == TimeSpan.Zero)
            {
                //removes screen
                ScreenManager.RemoveScreen(this);
            }
            else
            {
                //flags screen to transition off
                isExiting = true;
            }
        }

    }
}
        #endregion