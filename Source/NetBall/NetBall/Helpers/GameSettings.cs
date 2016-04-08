using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.Helpers
{
    /// <summary>
    /// This class contains functionality related to game mechanics and controls.
    /// </summary>
    public static class GameSettings
    {
        public static Vector2 MAX_LEVEL_SIZE = new Vector2(16000, 16000);
        public static float LEVEL_BUFFER = 128;

        public static int MIN_QUADTREE_LEAF_SIZE = 64;
        public static int MIN_QUADTREE_ELEMENTS = 10;
    }
}
