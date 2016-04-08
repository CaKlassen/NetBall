using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CSharpQuadTree;
using NetBall.GameObjects.Entities;
using NetBall.GameObjects.Props;
using NetBall.Helpers;

namespace NetBall.Scenes
{
    public abstract class ActionScene : Scene
    {
        protected List<Entity> addList { get; }
        protected List<Entity> addHUDList { get; }
        protected List<Entity> removeList { get; }
        protected List<Entity> removeHUDList { get; }
        protected List<Entity> entityList { get; }
        protected List<Entity> entityHUDList { get; }
        protected List<Prop> propList { get; }
        public QuadTree<EntityCollide> groundList { get; set; }
        public QuadTree<EntityCollide> jumpThruList { get; set; }

        public ActionScene()
        {
            entityList = new List<Entity>();
            addList = new List<Entity>();
            removeList = new List<Entity>();
            entityHUDList = new List<Entity>();
            addHUDList = new List<Entity>();
            removeHUDList = new List<Entity>();
            propList = new List<Prop>();

            groundList = new QuadTree<EntityCollide>(new Vector2(GameSettings.MIN_QUADTREE_LEAF_SIZE, GameSettings.MIN_QUADTREE_LEAF_SIZE), GameSettings.MIN_QUADTREE_ELEMENTS);
            jumpThruList = new QuadTree<EntityCollide>(new Vector2(GameSettings.MIN_QUADTREE_LEAF_SIZE, GameSettings.MIN_QUADTREE_LEAF_SIZE), GameSettings.MIN_QUADTREE_ELEMENTS);
        }

        public void addEntity(Entity e, bool hud = false)
        {
            if (!hud)
                addList.Add(e);
            else
                addHUDList.Add(e);
        }

        public void removeEntity(Entity e)
        {
            removeList.Add(e);
        }

        public void addDeco(Prop p)
        {
            propList.Add(p);
        }

        public void removeEntities(List<Entity> entities)
        {
            foreach (Entity e in entities)
            {
                removeList.Add(e);
            }
        }

        public Entity getEntity(Type t)
        {
            foreach (Entity e in entityList)
            {
                if (t.IsAssignableFrom(e.GetType()))
                {
                    return e;
                }
            }

            foreach (Entity e in entityHUDList)
            {
                if (t.IsAssignableFrom(e.GetType()))
                {
                    return e;
                }
            }

            return null;
        }

        public List<Entity> getEntities(Type t)
        {
            List<Entity> returnList = new List<Entity>();

            foreach (Entity e in entityList)
            {
                if (t.IsAssignableFrom(e.GetType()))
                {
                    returnList.Add(e);
                }
            }

            foreach (Entity e in entityHUDList)
            {
                if (t.IsAssignableFrom(e.GetType()))
                {
                    returnList.Add(e);
                }
            }

            return returnList;
        }

        public override void update(GameTime gameTime)
        {
            // Remove all destroyed entities
            foreach (Entity e in removeList)
            {
                entityList.Remove(e);
                entityHUDList.Remove(e);
            }

            removeList.Clear();
            removeHUDList.Clear();

            // Add new entities
            foreach (Entity e in addList)
            {
                entityList.Add(e);
            }

            foreach (Entity e in addHUDList)
            {
                entityHUDList.Add(e);
            }

            addList.Clear();
            addHUDList.Clear();
        }
    }
}
