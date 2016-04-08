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
        public static Vector2 BALL_OFFSET = new Vector2(352, ScreenHelper.SCREEN_SIZE.Y / 2);
        public static Vector2 HOOP_POSITION = new Vector2(0, 300);
        public static Vector2 SCREEN_OFFSET = new Vector2(0, 0);

        public static Color P1_COLOR = new Color(1.0f, 0.0f, 1.0f, 1.0f);
        public static Color P2_COLOR = new Color(0.0f, 1.0f, 0.0f, 1.0f);

        public static bool P1_TOUCHED_LAST = false;

        public static int MIN_QUADTREE_LEAF_SIZE = 64;
        public static int MIN_QUADTREE_ELEMENTS = 10;

        public static bool IS_HOST = false;
        public static bool CONNECTED = false;
    }
}
