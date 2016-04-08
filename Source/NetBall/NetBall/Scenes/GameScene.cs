using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace NetBall.Scenes
{
    public class GameScene : ActionScene
    {
        public GameScene(ContentManager content) : base()
        {

        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);


        }

        public override void draw(SpriteBatch spriteBatch)
        {
            BaseGame.instance.GraphicsDevice.Clear(Color.Purple);

        }
    }
}
