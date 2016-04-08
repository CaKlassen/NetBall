using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.GameObjects.Entities.HUD
{
    public interface TransitionReceiver
    {
        void transitionMiddle();

        void transitionDone();
    }
}
