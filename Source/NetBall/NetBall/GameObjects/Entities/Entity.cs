using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.GameObjects.Entities
{
    public abstract class Entity
    {
        protected Vector2 position;
        public Vector2 Position { get { return position; } set { position = value; } }

        public abstract void update(GameTime gameTime);
        public abstract void draw(SpriteBatch spriteBatch);
    }
}
