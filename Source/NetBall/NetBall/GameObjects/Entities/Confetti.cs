using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using NetBall.Helpers;
using NetBall.Scenes;

namespace NetBall.GameObjects.Entities
{
    public class Confetti : Entity
    {
        private static float FRICTION = 0.1f;

        private Texture2D sprite;
        private float rotation;
        private float scale;
        private float rotateSpeed;
        private Vector2 speed;
        private Vector2 origin;

        public Confetti(ContentManager content, Vector2 position)
        {
            this.position = position;

            Random r = GameScene.instance.generator;

            Color c = new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());

            sprite = new Texture2D(BaseGame.instance.GraphicsDevice, 6, 2);
            sprite.SetData(new Color[] { c });

            rotation = MathUtils.randomFloat(r, 0, (float)Math.PI * 2);
            scale = MathUtils.randomFloat(r, 4, 15);

            rotateSpeed = 0;// MathUtils.randomFloat(r, -0.2f, 0.2f);

            if (GameSettings.IS_HOST)
                speed.X = MathUtils.randomFloat(r, 7, 15);
            else
                speed.X = MathUtils.randomFloat(r, -7, -15);

            speed.Y = MathUtils.randomFloat(r, 8, -8);

            origin = new Vector2(3, 1);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, GameSettings.SCREEN_OFFSET + position, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0.01f);
        }

        public override void update(GameTime gameTime)
        {
            position += speed;

            speed.X = MathUtils.approach(speed.X, 0, FRICTION);
            speed.Y = MathUtils.approach(speed.Y, 0, FRICTION);

            rotation += rotateSpeed;

            if (Math.Abs(speed.Length()) < 0.5f)
            {
                ((ActionScene)SceneManager.currentScene).removeEntity(this);
            }
        }
    }
}
