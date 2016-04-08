using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSharpQuadTree;
using NetBall.Scenes;

namespace NetBall.GameObjects.Entities
{
    public abstract class EntityGravity : EntityCollide
    {
        protected Rectangle queryRectangle;
        protected Vector2 speed;
        protected Vector2 counter;
        protected bool onGround;
        protected bool preciseCollisions = false;

        protected List<EntityCollide> queries;
        protected List<EntityCollide> jumpThrus;

        public Vector2 Speed { get { return speed; } set { speed = value; } }
        public bool OnGround { get { return onGround; } }

        public EntityGravity()
        {
            speed = new Vector2(0, 0);
            counter = new Vector2(0, 0);
            onGround = false;
        }

        public override void update(GameTime gameTime)
        {
            // Retrieve the list of collidable objects
            ActionScene scene = (ActionScene)SceneManager.currentScene;
            queryRectangle = getBoundingBox();
            queryRectangle.X -= (int)Math.Round(Math.Abs(speed.X)) + 1;
            queryRectangle.Y -= (int)Math.Round(Math.Abs(speed.Y)) + 1;
            queryRectangle.Width += (int)Math.Round(Math.Abs(speed.X)) * 2 + 2;
            queryRectangle.Height += (int)Math.Round(Math.Abs(speed.Y)) * 2 + 2;

            queries = scene.groundList.Query(queryRectangle);
            jumpThrus = scene.jumpThruList.Query(queryRectangle);

            // Handle gravity
            onGround = checkBelow(queries, jumpThrus);

            Vector2 pixelsToMove = new Vector2(0, 0);
            bool didCollide = false;

            // Determine the number of pixels to move this frame
            counter += speed;
            pixelsToMove.X = (float)Math.Round(counter.X);
            pixelsToMove.Y = (float)Math.Round(counter.Y);
            counter -= pixelsToMove;

            // Horizontal
            for (int i = 0; i < Math.Abs(pixelsToMove.X); i++)
            {
                if (collideList(queries, preciseCollisions, new Vector2(Math.Sign(pixelsToMove.X), 0)))
                {
                    didCollide = true;
                    break;
                }
                else
                {
                    position.X += Math.Sign(pixelsToMove.X);
                }
            }

            if (didCollide)
            {
                horizontalCollision();
            }

            didCollide = false;

            // Vertical
            for (int i = 0; i < Math.Abs(pixelsToMove.Y); i++)
            {
                if (speed.Y <= 0)
                {
                    if (collideList(queries, preciseCollisions, new Vector2(0, Math.Sign(pixelsToMove.Y))))
                    {
                        didCollide = true;
                        break;
                    }
                    else
                    {
                        position.Y += Math.Sign(pixelsToMove.Y);
                    }
                }
                else if (checkBelow(queries, jumpThrus))
                {
                    didCollide = true;
                    break;
                }
                else
                {
                    position.Y += Math.Sign(pixelsToMove.Y);
                }
            }

            if (didCollide)
            {
                verticalCollision();
            }
        }

        protected bool checkBelow(List<EntityCollide> queries, List<EntityCollide> jumpThrus)
        {
            return (collideList(queries, preciseCollisions, new Vector2(0, 1)) ||
                collideList(jumpThrus, preciseCollisions, new Vector2(0, 1)) && !collideList(jumpThrus, preciseCollisions) && speed.Y >= 0);
        }

        protected virtual void horizontalCollision()
        {
            Vector2 newSpeed = speed;
            newSpeed.X = 0;
            speed = newSpeed;
        }

        protected virtual void verticalCollision()
        {
            Vector2 newSpeed = speed;
            newSpeed.Y = 0;
            speed = newSpeed;
        }
    }
}
