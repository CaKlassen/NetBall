using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.GameObjects.Props
{
    public class Prop
    {
        private Texture2D sprite;
        private Vector2 pos;
        private Vector2 origin;
        private float depth;

        public Prop(Texture2D sprite, Vector2 pos, float depth = 0, bool centred = false)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.depth = depth;

            if (centred)
                origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
            else
                origin = Vector2.Zero;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, pos, null, Color.White, 0, origin, 1, SpriteEffects.None, depth);
        }
    }
}
