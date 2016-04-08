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

        private Texture2D sprite;
        private float rotation = 0;
        private float radius;

        private Vector2 prevMousePosition;
        private bool held = false;

        public Ball(ContentManager content, Vector2 position)
        {
            this.position = position;

            sprite = content.Load<Texture2D>("Sprites/Ball");
            mask = sprite;

            radius = mask.Width / 2;
            origin = new Vector2(mask.Width / 2, mask.Height / 2);

            MessageUtils.registerListener(this, MessageType.BALL_THROW);
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);
            MouseState mouse = Mouse.GetState();
            Vector2 mousePos = mouse.Position.ToVector2() - GameSettings.SCREEN_OFFSET;

            if (!held)
            {
                if (mouse.LeftButton == ButtonState.Pressed && Vector2.Distance(position, mousePos) <= radius)
                {
                    held = true;
                }
            }
            else
            {
                position.X += MathUtils.smoothChange(position.X, mouse.Position.X, MOVE_RATE);
                position.Y += MathUtils.smoothChange(position.Y, mouse.Position.Y, MOVE_RATE);

                if (mouse.LeftButton == ButtonState.Released)
                {
                    held = false;
                    float angle = MathUtils.pointAngle(prevMousePosition, mousePos);

                    if (GameSettings.IS_HOST)
                    {
                        NetworkServer.instance.sendData(MessageUtils.constructMessage(MessageType.BALL_THROW,
                            new MessageDataBallThrow(position, 5, angle)));
                    }
                    else
                    {
                        NetworkClient.instance.sendData(MessageUtils.constructMessage(MessageType.BALL_THROW,
                            new MessageDataBallThrow(position, 5, angle)));
                    }

                    speed.X = MathUtils.lengthdirX(angle, 5);
                    speed.Y = MathUtils.lengthdirY(angle, 5);
                }
            }

            prevMousePosition = mousePos;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, GameSettings.SCREEN_OFFSET + position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 0.5f);
        }

        public void eventTriggered(MessageData data)
        {
            if (data.GetType() == typeof(MessageDataBallThrow))
            {
                MessageDataBallThrow castData = (MessageDataBallThrow)data;
                position = castData.position;
                speed.X = MathUtils.lengthdirX(castData.angle, castData.speed);
                speed.Y = MathUtils.lengthdirY(castData.angle, castData.speed);
            }
        }
    }
}
