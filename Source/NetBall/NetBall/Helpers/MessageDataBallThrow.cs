using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.Helpers
{
    public class MessageDataBallThrow : MessageData
    {
        public Vector2 position { get; set; }
        public float speed { get; set; }
        public float angle { get; set; }

        public MessageDataBallThrow(Vector2 position, float speed, float angle)
        {
            this.position = position;
            this.speed = speed;
            this.angle = angle;
        }
    }
}
