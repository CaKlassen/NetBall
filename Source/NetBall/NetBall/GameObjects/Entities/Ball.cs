using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace NetBall.GameObjects.Entities
{
    public class Ball : EntityGravity
    {
        private Texture2D sprite;
        private float rotation = 0;

        public Ball(ContentManager content, Vector2 position)
        {
            this.position = position;

            sprite = content.Load<Texture2D>("Sprites/Ball");
            mask = sprite;
            origin = new Vector2(mask.Width / 2, mask.Height / 2);
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);

        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 0.5f);
        }
    }
}
