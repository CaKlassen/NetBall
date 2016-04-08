using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using NetBall.Helpers;
using NetBall.Helpers.Network;
using NetBall.Helpers.Network.Messages;
using NetBall.GameObjects.Entities;
using NetBall.GameObjects.Props;

namespace NetBall.Scenes
{
    public class GameScene : ActionScene, EventListener
    {
        public Random generator { get; set; }

        public GameScene(ContentManager content) : base()
        {
            MessageUtils.initialize();

            //fetch the network data from the configure file
            FileData data = StartupUtils.readfileData();

            //determine if this computer is a client or a server and connect them
            if (data.isHost)
            {
                NetworkServer netServ = new NetworkServer(data.peer, data.port);
                netServ.startServer();
                GameSettings.IS_HOST = true;
            }
            else
            {
                NetworkClient netClient = new NetworkClient(data.peer, data.port);
                netClient.connectClient();
                GameSettings.IS_HOST = false;
            }

            // Wait until the connection to the other computer has been made
            while (!GameSettings.CONNECTED);

            generator = new Random();

            if (GameSettings.IS_HOST)
            {
                // Set up the initial round
                bool ballSide = generator.Next(2) == 0 ? true : false;

                Vector2 ballPos = new Vector2(ScreenHelper.SCREEN_SIZE.X + GameSettings.BALL_OFFSET.X, GameSettings.BALL_OFFSET.Y);

                NetworkServer.instance.sendData(MessageUtils.constructMessage(MessageType.BALL_SETUP,
                    new MessageDataBallSetup(ballPos)));

                addEntity(new Ball(content, ballPos));
            }
            else
            {
                MessageUtils.registerListener(this, MessageType.BALL_SETUP);
                GameSettings.SCREEN_OFFSET.X = -ScreenHelper.SCREEN_SIZE.X;
            }

            initialize(content);
        }

        private void initialize(ContentManager content)
        {
            for (int i = 0; i < 300; i++)
            {
                addEntity(new Block(content, new Vector2(GameSettings.SCREEN_OFFSET.X + i * 64, GameSettings.SCREEN_OFFSET.Y + ScreenHelper.SCREEN_SIZE.Y - 64)));
            }
        }

       
        public override void update(GameTime gameTime)
        {
            base.update(gameTime);

            // Update all the scene's entities
            foreach (Entity e in entityList)
            {
                e.update(gameTime);
            }

            foreach (Entity e in entityHUDList)
            {
                e.update(gameTime);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            BaseGame.instance.GraphicsDevice.Clear(Color.Purple);

            // In-game entities
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            foreach (Prop p in propList)
            {
                p.draw(spriteBatch);
            }

            foreach (Entity e in entityList)
            {
                e.draw(spriteBatch);
            }

            spriteBatch.End();

            // HUD
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            foreach (Entity e in entityHUDList)
            {
                e.draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public void eventTriggered(MessageData data)
        {
            if (data.GetType() == typeof(MessageDataBallSetup))
            {
                MessageDataBallSetup castData = (MessageDataBallSetup)data;
                addEntity(new Ball(SceneManager.content, GameSettings.SCREEN_OFFSET + castData.position));
            }
        }
    }
}
