using Microsoft.Xna.Framework;
using NetBall.GameObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetBall.Helpers
{
    public static class MathUtils
    {

        public static int roundDownToMultiple(float value, int multiple)
        {
            int newVal = (int)Math.Floor((decimal)((int)value / multiple));

            return newVal * multiple;
        }

        /// <summary>
        /// This function approaches a certain value at a linear rate without going over.
        /// </summary>
        /// <param name="start">The initial position</param>
        /// <param name="end">The final position</param>
        /// <param name="maxChange">The speed to change at</param>
        /// <returns></returns>
        public static float approach(float start, float end, float maxChange)
        {
            if (start < end)
            {
                return Math.Min(start + maxChange, end);
            }
            else
            {
                return Math.Max(start - maxChange, end);
            }
        }

        /// <summary>
        /// This function smoothly changes a value to a target at a certain ratio.
        /// 
        /// A higher ratio (eg. 20) is slow, while a lower ratio (eg. 5) is fast.
        /// </summary>
        /// <param name="startValue">The initial value</param>
        /// <param name="endValue">The target value</param>
        /// <param name="ratio">The ratio to change at</param>
        /// <returns></returns>
        public static float smoothChange(float startValue, float endValue, float ratio)
        {
            return -((startValue - endValue) / ratio);
        }

        /// <summary>
        /// This function returns a random float between two values.
        /// </summary>
        /// <param name="r">The random generator</param>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>The random float</returns>
        public static float randomFloat(Random r, float min, float max)
        {
            return (float) r.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// This function returns the angle between two points in radians.
        /// </summary>
        /// <param name="start">The starting point</param>
        /// <param name="end">The ending point</param>
        /// <returns>The angle in radians</returns>
        public static float pointAngle(Vector2 start, Vector2 end)
        {
            Vector2 dif = end - start;
            return (float)Math.Atan2(dif.Y, dif.X);
        }

        /// <summary>
        /// This function calculates the length of the x component for a certain angle.
        /// </summary>
        /// <param name="angle">The angle in radians</param>
        /// <param name="length">The length of the line</param>
        /// <returns>The length of the x component</returns>
        public static float lengthdirX(float angle, float length)
        {
            return (float) Math.Cos(angle) * length;
        }

        /// <summary>
        /// This function calculates the length of the y component for a certain angle.
        /// </summary>
        /// <param name="angle">The angle in radians</param>
        /// <param name="length">The length of the line</param>
        /// <returns>The length of the y component</returns>
        public static float lengthdirY(float angle, float length)
        {
            return (float)Math.Sin(angle) * length;
        }

        public static bool collisionLine(Vector2 start, Vector2 end, List<EntityCollide> objects)
        {
            Vector2 tracer = start;
            float angle = pointAngle(start, end);

            while (Vector2.Distance(tracer, end) > 4)
            {
                foreach (EntityCollide entity in objects)
                {
                    if (entity.rectangle.Contains(tracer))
                        return true;
                }

                tracer.X += lengthdirX(angle, 4);
                tracer.Y += lengthdirY(angle, 4);
            }

            return false;
        }

        public static Vector2 getCollisionNormal(EntityCollide entityA, EntityCollide entityB, bool precise = false)
        {
            Vector2 normal = Vector2.Zero;

            if (!precise)
            {
                Vector2 r = entityB.Position - entityA.Position;

                if (Math.Abs(Vector2.Dot(r, Vector2.UnitX)) > Math.Abs(Vector2.Dot(r, Vector2.UnitY)))
                {
                    normal = Vector2.UnitX * -Math.Sign(Vector2.Dot(r, Vector2.UnitX));
                }
                else
                {
                    normal = Vector2.UnitY * -Math.Sign(Vector2.Dot(r, Vector2.UnitY));
                }
            }
            else
            {
                normal = entityA.Position - entityA.Position;
            }

            return normal;
        }

    }
}
