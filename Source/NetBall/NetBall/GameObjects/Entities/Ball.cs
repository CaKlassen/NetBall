using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NetBall.Helpers;
using NetBall.Helpers.Network;
using NetBall.Helpers.Network.Messages;

namespace NetBall.GameObjects.Entities
{
    public class Ball : EntityGravity, EventListener
    {
        private static float MOVE_RATE = 3;
        private static float GRAVITY = 0.4f;
        private static float FRICTION = 0.05f;
        private static float RESTITUTION = 0.9f;
        private static float SPEED_DIVISOR = 2;

        private Texture2D sprite;
        private Texture2D spriteLights;
        private float rotation = 0;
        private float radius;

        private ButtonState prevMouseState;
        private Vector2 prevMousePosition;
        private bool held = false;

        public Ball(ContentManager content, Vector2 position)
        {
            this.position = position;

            sprite = content.Load<Texture2D>("Sprites/Ball");
            spriteLights = content.Load<Texture2D>("Sprites/BallLights");
            mask = sprite;

            radius = mask.Width / 2;
            origin = new Vector2(mask.Width / 2, mask.Height / 2);

            MessageUtils.registerListener(this, MessageType.BALL_THROW);

            prevMouseState = ButtonState.Released;
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            MouseState mouse = Mouse.GetState();
            Vector2 mousePos = mouse.Position.ToVector2() - GameSettings.SCREEN_OFFSET;

            Rectangle mouseValid = new Rectangle(64 + (int)(radius), 64 + (int)(radius), (int)(ScreenHelper.SCREEN_SIZE.X * 2 - (radius + 64) * 2), (int)(ScreenHelper.SCREEN_SIZE.Y - (radius + 64) * 2));
            Rectangle centreArea = new Rectangle((int)(ScreenHelper.SCREEN_SIZE.X - radius), 0, (int)radius * 2, (int)ScreenHelper.SCREEN_SIZE.Y);

            if (!held)
            {
                speed.Y += GRAVITY;
                speed.X = MathUtils.approach(speed.X, 0, FRICTION);

                if (mouse.LeftButton == ButtonState.Pressed && prevMouseState == ButtonState.Released &&
                    mouseValid.Contains(mousePos) && !centreArea.Contains(mousePos) && Vector2.Distance(position, mousePos) <= radius)
                {
                    held = true;


                    if (GameSettings.IS_HOST)
                        GameSettings.P1_TOUCHED_LAST = true;
                    else
                        GameSettings.P1_TOUCHED_LAST = false;

                    if (GameSettings.IS_HOST)
                    {
                        NetworkServer.instance.sendData(MessageUtils.constructMessage(MessageType.BALL_THROW,
                            new MessageDataBallThrow(position, 0, 0)));
                    }
                    else
                    {
                        NetworkClient.instance.sendData(MessageUtils.constructMessage(MessageType.BALL_THROW,
                            new MessageDataBallThrow(position, 0, 0)));
                    }
                }
            }
            else
            {
                speed = Vector2.Zero;

                position.X += MathUtils.smoothChange(position.X, mousePos.X, MOVE_RATE);
                position.Y += MathUtils.smoothChange(position.Y, mousePos.Y, MOVE_RATE);

                if (mouse.LeftButton == ButtonState.Released || !mouseValid.Contains(mousePos) || centreArea.Contains(mousePos))
                {
                    held = false;
                    float angle = MathUtils.pointAngle(prevMousePosition, mousePos);
                    float newSpeed = Vector2.Distance(mousePos, prevMousePosition) / SPEED_DIVISOR;

                    if (GameSettings.IS_HOST)
                    {
                        NetworkServer.instance.sendData(MessageUtils.constructMessage(MessageType.BALL_THROW,
                            new MessageDataBallThrow(position, newSpeed, angle)));
                    }
                    else
                    {
                        NetworkClient.instance.sendData(MessageUtils.constructMessage(MessageType.BALL_THROW,
                            new MessageDataBallThrow(position, newSpeed, angle)));
                    }

                    speed.X = MathUtils.lengthdirX(angle, newSpeed);
                    speed.Y = MathUtils.lengthdirY(angle, newSpeed);
                }
            }

            prevMousePosition = mousePos;
            prevMouseState = mouse.LeftButton;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, GameSettings.SCREEN_OFFSET + position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 0.5f);

            Color c = GameSettings.P1_TOUCHED_LAST ? GameSettings.P1_COLOR : GameSettings.P2_COLOR;
            spriteBatch.Draw(spriteLights, GameSettings.SCREEN_OFFSET + position, null, c, rotation, origin, 1, SpriteEffects.None, 0.49f);
        }

        public void eventTriggered(MessageData data)
        {
            if (data.GetType() == typeof(MessageDataBallThrow))
            {
                if (GameSettings.IS_HOST)
                    GameSettings.P1_TOUCHED_LAST = false;
                else
                    GameSettings.P1_TOUCHED_LAST = true;

                MessageDataBallThrow castData = (MessageDataBallThrow)data;
                position = castData.position;
                speed.X = MathUtils.lengthdirX(castData.angle, castData.speed);
                speed.Y = MathUtils.lengthdirY(castData.angle, castData.speed);
            }
        }

        protected override void horizontalCollision()
        {
            speed.X = -speed.X * RESTITUTION;
        }

        protected override void verticalCollision()
        {
            if (speed.Y < 0.5f)
            {
                speed.Y = 0;
            }
            else
            {
                speed.Y = -speed.Y * RESTITUTION;
            }
        }
    }
}
