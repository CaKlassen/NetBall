using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.Scenes
{
    public abstract class Scene
    {
        public abstract void update(GameTime gameTime);
        public abstract void draw(SpriteBatch spriteBatch);
    }
}
