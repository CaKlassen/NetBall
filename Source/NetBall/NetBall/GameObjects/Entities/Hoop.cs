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
        private Texture2D spriteLights;

        private bool leftSide;

        Vector2 prevBallPosition = Vector2.Zero;

        public Hoop(ContentManager content, Vector2 position, bool leftSide)
        {
            this.position = position;

            spriteBack = content.Load<Texture2D>("Sprites/HoopBack");
            spriteFront = content.Load<Texture2D>("Sprites/HoopFront");
            spriteLights = content.Load<Texture2D>("Sprites/HoopLights");
            mask = content.Load<Texture2D>("Sprites/HoopMask");

            this.leftSide = leftSide;

            if (leftSide)
                origin = new Vector2(-spriteFront.Width + mask.Width,  -spriteFront.Height / 2);
            else
                origin = new Vector2(spriteFront.Width, -spriteFront.Height / 2);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if (leftSide)
            {
                spriteBatch.Draw(spriteFront, GameSettings.SCREEN_OFFSET + position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                spriteBatch.Draw(spriteLights, GameSettings.SCREEN_OFFSET + position, null, GameSettings.P1_COLOR, 0, Vector2.Zero, 1, SpriteEffects.None, 0.1f);
                spriteBatch.Draw(spriteBack, GameSettings.SCREEN_OFFSET + position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.7f);
            }
            else
            {
                spriteBatch.Draw(spriteFront, GameSettings.SCREEN_OFFSET + position + new Vector2(-spriteBack.Width, 0), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0.2f);
                spriteBatch.Draw(spriteLights, GameSettings.SCREEN_OFFSET + position + new Vector2(-spriteBack.Width, 0), null, GameSettings.P2_COLOR, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0.1f);
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
                    if (b.Position.X < position.X - origin.X - 8 && b.Position.X > position.X && 
                        b.Position.Y > position.Y - origin.Y && prevBallPosition.Y < position.Y - origin.Y)
                    {
                        GameScene.instance.removeEntity(ball);
                        Vector2 ballPos = GameScene.instance.getBallStartPosition();

                        NetworkServer.instance.sendData(MessageUtils.constructMessage(MessageType.BALL_SETUP,
                            new MessageDataBallSetup(ballPos)));

                        GameScene.instance.addEntity(new Ball(SceneManager.content, ballPos));


                        //NetworkServer.instance.sendData(MessageUtils.constructMessage(MessageType.GOAL,
                        //    new MessageDataGoal()));

                        // Create score confetti
                        for (int i = 0; i < 60; i++)
                        {
                            //GameScene.instance.addEntity(new Confetti(SceneManager.content, GameSettings.HOOP_POSITION));
                        }
                    }
                }
                else
                {
                    if (b.Position.X > position.X - origin.X + 8 && b.Position.X < position.X &&
                        b.Position.Y > position.Y - origin.Y && prevBallPosition.Y < position.Y - origin.Y)
                    {
                        GameScene.instance.removeEntity(ball);
                        Vector2 ballPos = GameScene.instance.getBallStartPosition();

                        NetworkServer.instance.sendData(MessageUtils.constructMessage(MessageType.GOAL,
                            new MessageDataGoal()));

                        GameScene.instance.addEntity(new Ball(SceneManager.content, ballPos));

                        //NetworkServer.instance.sendData(MessageUtils.constructMessage(MessageType.GOAL,
                        //    new MessageDataGoal()));

                        // Create score confetti
                        for (int i = 0; i < 60; i++)
                        {
                            //GameScene.instance.addEntity(new Confetti(SceneManager.content, GameSettings.HOOP_POSITION));
                        }
                    }
                }
            }

            prevBallPosition = ball.Position;
        }
    }
}
