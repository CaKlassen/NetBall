using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.Helpers
{
    public static class ScreenHelper
    {
        public static Vector2 SCREEN_SIZE = new Vector2(1366, 768);
        public static float SCREEN_SCALE = 1f;
        public static Vector2 VIEW_SIZE = SCREEN_SIZE * SCREEN_SCALE;

        /// <summary>
        /// This function resizes the physical game window based on the new rendering scale.
        /// </summary>
        /// <param name="scale">The new scale to render at</param>
        public static void updateScreenScale(float scale)
        {
            SCREEN_SCALE = scale;
            VIEW_SIZE = SCREEN_SIZE * SCREEN_SCALE;

            // Update the actual screen size
            BaseGame game = BaseGame.instance;
            game.graphics.PreferredBackBufferWidth = (int) VIEW_SIZE.X;
            game.graphics.PreferredBackBufferHeight = (int) VIEW_SIZE.Y;
            game.graphics.ApplyChanges();
        }
    }
}
