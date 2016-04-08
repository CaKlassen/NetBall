using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetBall.Helpers;
using NetBall.Scenes;
using NetBall.Helpers.Network;
using NetBall.Helpers.Network.Messages;

namespace NetBall.GameObjects.Entities
{
    public class Hoop : EntityCollide
    {
        private Texture2D spriteBack;
        private Texture2D spriteFront;

        private bool leftSide;

        Vector2 prevBallPosition = Vector2.Zero;

        public Hoop(ContentManager content, Vector2 position, bool leftSide)
        {
            this.position = position;

            spriteBack = content.Load<Texture2D>("Sprites/HoopBack");
            spriteFront = content.Load<Texture2D>("Sprites/HoopFront");
            mask = content.Load<Texture2D>("Sprites/HoopMask");

            this.leftSide = leftSide;

            if (leftSide)
                origin = new Vector2(-spriteFront.Width + 32,  -spriteFront.Height / 2);
            else
                origin = new Vector2(spriteFront.Width, -spriteFront.Height / 2);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (leftSide)
            {
                spriteBatch.Draw(spriteFront, GameSettings.SCREEN_OFFSET + position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                spriteBatch.Draw(spriteBack, GameSettings.SCREEN_OFFSET + position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.7f);
            }
            else
            {
                spriteBatch.Draw(spriteFront, GameSettings.SCREEN_OFFSET + position + new Vector2(-spriteBack.Width, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0.2f);
                spriteBatch.Draw(spriteBack, GameSettings.SCREEN_OFFSET + position + new Vector2(-spriteBack.Width, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0.7f);
            }
        }

        public override void update(GameTime gameTime)
        {
            Entity ball = ((ActionScene)SceneManager.currentScene).getEntity(typeof(Ball));

            if (GameSettings.IS_HOST)
            {
                // Check for a basket
                Ball b = (Ball)ball;

                if (leftSide)
                {
                    if (b.Position.X < position.X - origin.X && b.Position.X > position.X && 
                        b.Position.Y > position.Y - origin.Y && prevBallPosition.Y < position.Y - origin.Y)
                    {
                        GameScene.instance.removeEntity(ball);
                        Vector2 ballPos = GameScene.instance.getBallStartPosition();

                        NetworkServer.instance.sendData(MessageUtils.constructMessage(MessageType.BALL_THROW,
                            new MessageDataBallSetup(ballPos)));

                        GameScene.instance.addEntity(new Ball(SceneManager.content, ballPos));
                    }
                }
                else
                {
                    if (b.Position.X > position.X - origin.X && b.Position.X < position.X &&
                        b.Position.Y > position.Y - origin.Y && prevBallPosition.Y < position.Y - origin.Y)
                    {
                        GameScene.instance.removeEntity(ball);
                        Vector2 ballPos = GameScene.instance.getBallStartPosition();

                        NetworkServer.instance.sendData(MessageUtils.constructMessage(MessageType.BALL_THROW,
                            new MessageDataBallSetup(ballPos)));

                        GameScene.instance.addEntity(new Ball(SceneManager.content, ballPos));
                    }
                }
            }

            prevBallPosition = ball.Position;
        }
    }
}
