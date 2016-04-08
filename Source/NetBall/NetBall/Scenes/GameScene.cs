using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using NetBall.Helpers;
using NetBall.Helpers.Network;

namespace NetBall.Scenes
{
    public class GameScene : ActionScene
    {
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
            }
            else
            {
                NetworkClient netClient = new NetworkClient(data.peer, data.port);
                netClient.connectClient();
            }


            // TODO: Proper data transfer
            if (!data.isHost)
            {
                NetworkClient.instance.sendData(MessageUtils.constructMessage(MessageType.BALL_THROW, 
                    new MessageDataBallThrow(new Vector2(50, 50), 5, (float)Math.PI)));
            }
        }
       
        public override void update(GameTime gameTime)
        {
            base.update(gameTime);


        }

        public override void draw(SpriteBatch spriteBatch)
        {
            BaseGame.instance.GraphicsDevice.Clear(Color.Purple);

        }
    }
}
