using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.Helpers
{
    public interface EventListener
    {
        void eventTriggered(MessageData data);
    }
}
