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
            int numBlocks = (int)(ScreenHelper.SCREEN_SIZE.X * 2) / 64;

            // Floor
            for (int i = 0; i < numBlocks; i++)
            {
                Block b = new Block(content, new Vector2(i * 64, ScreenHelper.SCREEN_SIZE.Y - 64));
                addEntity(b);
                groundList.Insert(b);

                b = new Block(content, new Vector2(i * 64, 0));
                addEntity(b);
                groundList.Insert(b);
            }

            // Walls
            for (int i = 0; i < 20; i++)
            {
                Block b = new Block(content, new Vector2(0, GameSettings.SCREEN_OFFSET.Y + i * 64));
                addEntity(b);
                groundList.Insert(b);

                b = new Block(content, new Vector2(ScreenHelper.SCREEN_SIZE.X * 2 - 64, GameSettings.SCREEN_OFFSET.Y + i * 64));
                addEntity(b);
                groundList.Insert(b);
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
                addEntity(new Ball(SceneManager.content, castData.position));
            }
        }
    }
}
