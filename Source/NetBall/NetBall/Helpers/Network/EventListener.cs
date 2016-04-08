using NetBall.Helpers.Network.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.Helpers.Network
{
    public interface EventListener
    {
        void eventTriggered(MessageData data);
    }
}
