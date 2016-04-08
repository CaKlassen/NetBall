using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetBall.Scenes;
using NetBall.Helpers;

namespace NetBall.GameObjects.Entities.HUD
{
    public enum TransitionState
    {
        START,
        MIDDLE,
        DONE
    }

    public enum TransitionType
    {
        VERTICAL_WIPE
    }

    public class ScreenTransition : Entity
    {
        private static int WAIT_TIME = 10;

        private int waitTime = WAIT_TIME;

        // Vertical Wipe
        private static float WIPE_SPEED = 7f;
        private static Color WIPE_COLOR = new Color(0.1f, 0.1f, 0.1f, 1);

        private Texture2D overlay;
        private Rectangle screenRect;
        private float rectY;
        private float rectHeight;

        private TransitionType transType;
        private TransitionState state = TransitionState.START;
        private TransitionReceiver caller;

        public TransitionState State { get { return state; } }

        public ScreenTransition(TransitionReceiver caller, TransitionType type = TransitionType.VERTICAL_WIPE)
        {
            state = TransitionState.START;
            this.caller = caller;
            this.transType = type;

            overlay = new Texture2D(SceneManager.graphicsDevice, 1, 1);
            overlay.SetData(new Color[] { Color.White });

            rectY = 0;
            rectHeight = 0;
            screenRect = new Rectangle(0, 0, (int)ScreenHelper.SCREEN_SIZE.X, 0);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            
            switch(transType)
            {
                case TransitionType.VERTICAL_WIPE:
                {
                    // Vertical Wipe
                    spriteBatch.Draw(overlay, screenRect, WIPE_COLOR);
                    break;
                }
            }

            spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
            // TODO: Proper scene transition and removing self on completion
            switch(state)
            {
                case TransitionState.START:
                {
                    switch(transType)
                    {
                        case TransitionType.VERTICAL_WIPE:
                        {
                            if (rectHeight < ScreenHelper.SCREEN_SIZE.Y - 1)
                            {
                                rectHeight += MathUtils.smoothChange(rectHeight, ScreenHelper.SCREEN_SIZE.Y, WIPE_SPEED);
                                screenRect.Height = (int)rectHeight;
                            }
                            else
                            {
                                screenRect.Height = (int)ScreenHelper.SCREEN_SIZE.Y;

                                caller.transitionMiddle();
                                state = TransitionState.MIDDLE;
                            }
                            break;
                        }
                    }

                    break;
                }

                case TransitionState.MIDDLE:
                {
                    if (waitTime > 0)
                    {
                        waitTime--;
                    }
                    else
                    {
                        state = TransitionState.DONE;
                    }

                    break;
                }

                case TransitionState.DONE:
                {
                    switch (transType)
                    {
                        case TransitionType.VERTICAL_WIPE:
                        {
                            if (rectY < ScreenHelper.SCREEN_SIZE.Y - 1)
                            {
                                rectY += MathUtils.smoothChange(rectY, ScreenHelper.SCREEN_SIZE.Y, WIPE_SPEED);
                                screenRect.Y = (int)rectY;
                            }
                            else
                            {
                                caller.transitionDone();

                                // Kill the transition
                                SceneManager.instance.Transition = null;
                            }
                            break;
                        }
                    }


                    break;
                }
            }
        }
    }
}
