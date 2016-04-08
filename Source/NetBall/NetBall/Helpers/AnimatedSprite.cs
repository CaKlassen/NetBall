using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.Helpers
{
    public class AnimatedSprite
    {
        private Texture2D atlas;

        private int rows;
        private int columns;
        private int width;
        private int height;

        private float animationSpeed;
        private bool looping;
        private bool animationEnd = false;
        private float currentFrame;
        private float totalFrames;
        
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public float AnimationSpeed { get { return animationSpeed; } set { animationSpeed = value; } }
        public float CurrentFrame { get { return currentFrame; } set { currentFrame = value; } }
        public bool AnimationEnd { get { return animationEnd; } }


        public AnimatedSprite(Texture2D atlas, int rows, int columns, float animationSpeed, bool loop = true)
        {
            this.atlas = atlas;
            this.rows = rows;
            this.columns = columns;
            this.animationSpeed = animationSpeed;

            currentFrame = 0;
            looping = loop;
            totalFrames = rows * columns;

            width = atlas.Width / columns;
            height = atlas.Height / rows;
        }

        public void update(GameTime gameTime)
        {
            if (!animationEnd)
                currentFrame += animationSpeed;

            if (currentFrame >= totalFrames)
            {
                if (looping)
                {
                    currentFrame = currentFrame - totalFrames;
                }
                else
                {
                    currentFrame = totalFrames;
                    animationEnd = true;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, float depth = 0, float rotation = 0, float scale = 1, Color? color = null)
        {
            int row = (int)(currentFrame / columns);
            int column = (int) currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(width * scale), (int)(height * scale));

            spriteBatch.Draw(atlas, destinationRectangle, sourceRectangle, color ?? Color.White, rotation, origin, SpriteEffects.None, depth);
        }

        public Texture2D getSubImage(int num)
        {
            int targetRow, targetColumn;
            targetRow = 0;
            targetColumn = 0;

            for (int i = 0; i < num; i++)
            {
                targetColumn++;

                if (targetColumn > columns)
                {
                    targetColumn = 0;
                    targetRow++;
                }
            }

            Texture2D newImage = new Texture2D(BaseGame.instance.GraphicsDevice, width, height);

            Color[] data = new Color[width * height];

            Rectangle sourceRectangle = new Rectangle(width * targetColumn, height * targetRow, width, height);
            atlas.GetData(0, sourceRectangle, data, 0, data.Length);
            newImage.SetData(data);

            return newImage;
        }
    }
}
