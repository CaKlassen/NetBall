using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSharpQuadTree;

namespace NetBall.GameObjects.Entities
{
    public abstract class EntityCollide : Entity, IQuadObject
    {
        public Texture2D mask { get; set; }
        public Vector2 origin { get; set; }

        private float scale = 1;
        public float Scale { get { return scale; } set { scale = value; } }

        public Rectangle rectangle
        {
            get
            {
                return getBoundingBox();
            }
        }

        public event EventHandler BoundsChanged;

        public Rectangle getBoundingBox()
        {
            return new Rectangle((int) (position.X - origin.X * scale), (int) (Position.Y - origin.Y * scale), (int)(mask.Width * scale), (int)(mask.Height * scale));
        }

        public bool collideList(List<EntityCollide> list, bool precise, Vector2? offset = null)
        {
            foreach (EntityCollide en in list)
            {
                if (collide(en, precise, offset))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This function checks for a collision between two collidable entities.
        /// </summary>
        /// <param name="other">The other collidable entity</param>
        /// <param name="precise">Whether or not to use pixel-perfect checking</param>
        /// <returns></returns>
        public bool collide(EntityCollide other, bool precise, Vector2? offset = null)
        {
            Rectangle bbox = getBoundingBox();

            if (offset.HasValue)
            {
                bbox.X += (int)offset.Value.X;
                bbox.Y += (int)offset.Value.Y;
            }

            if (!precise)
            {
                return (other.getBoundingBox().Intersects(bbox));
            }
            else
            {
                return (other.getBoundingBox().Intersects(bbox) && perPixelCollision(other, offset));
            }
        }

        /// <summary>
        /// This function checks the pixel values of the two entities' masks for a collision.
        /// </summary>
        /// <param name="other">The other entity collide</param>
        /// <param name="offset">The optional offset for collision checking</param>
        /// <returns>Whether or not there was a collision</returns>
        private bool perPixelCollision(EntityCollide other, Vector2? offset = null)
        {
            // Calculate origin x and y
            Vector2 bounds = position - origin;
            Rectangle otherBounds = other.rectangle;

            if (offset.HasValue)
            {
                bounds += offset.Value;
            }

            Texture2D otherMask = other.mask;

            // Get the colour data for both textures
            Color[] bitsA = new Color[mask.Width * mask.Height];
            mask.GetData(bitsA);

            Color[] bitsB = new Color[otherMask.Width * otherMask.Height];
            otherMask.GetData(bitsB);

            // Calculate the intersecting rectangle
            int x1 = Math.Max((int)bounds.X, otherBounds.X);
            int x2 = Math.Min((int)bounds.X + mask.Bounds.Width, otherBounds.X + otherMask.Bounds.Width);

            int y1 = Math.Max((int)bounds.Y, otherBounds.Y);
            int y2 = Math.Min((int)bounds.Y + mask.Bounds.Height, otherBounds.Y + otherMask.Bounds.Height);

            // For each single pixel in the intersecting rectangle
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; ++x)
                {
                    // Get the color from each texture
                    Color a = bitsA[(x - (int)bounds.X) + (y - (int)bounds.Y) * mask.Width];
                    Color b = bitsB[(x - otherBounds.X) + (y - otherBounds.Y) * otherMask.Width];

                    if (a.A != 0 && b.A != 0) // If both colors are not transparent (the alpha channel is not 0), then there is a collision
                    {
                        return true;
                    }
                }
            }

            // If no collision occurred by now, we're clear.
            return false;
        }
    }
}
