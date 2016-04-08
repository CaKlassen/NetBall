using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetBall.Helpers;

namespace NetBall.GameObjects.Entities
{
    public class Block : EntityCollide
    {
        private Texture2D sprite;

        public Block(ContentManager content, Vector2 position)
        {
            this.position = position;

            sprite = content.Load<Texture2D>("Sprites/Block");
            mask = sprite;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, GameSettings.SCREEN_OFFSET + position, null, Color.White, 0, origin, 1, SpriteEffects.None, 0.9f);
        }

        public override void update(GameTime gameTime)
        {
            
        }
    }
}
