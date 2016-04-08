using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.Helpers.Network.Messages
{
    public class MessageDataBallSetup : MessageData
    {
        public Vector2 position { get; set; }

        public MessageDataBallSetup(Vector2 position)
        {
            this.position = position;
        }
    }
}
