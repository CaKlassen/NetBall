using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetBall.GameObjects.Entities;
using NetBall.GameObjects.Entities.HUD;

namespace NetBall.Scenes
{
    public class SceneManager : Entity, TransitionReceiver
    {
        public static SceneManager instance = null;

        public static ContentManager content;
        public static Scene currentScene;
        public static GraphicsDevice graphicsDevice;

        private ScreenTransition transition = null;
        private Scene previousScene = null;
        private Scene futureScene = null;

        public ScreenTransition Transition { get { return transition; } set { transition = value; } }

        public SceneManager(ContentManager contentManager, GraphicsDevice graphics)
        {
            instance = this;

            content = contentManager;
            graphicsDevice = graphics;
        }

        /// <summary>
        /// This function immediate transitions to a new scene.
        /// </summary>
        /// <param name="scene">The scene to change to</param>
        public void setScene(Scene scene)
        {
            previousScene = currentScene;
            currentScene = scene;
        }

        /// <summary>
        /// This function changes the current scene.
        /// </summary>
        /// <param name="scene">The scene to change to</param>
        public void changeScene(Scene scene)
        {
            futureScene = scene;
            transition = new ScreenTransition(this);
        }

        /// <summary>
        /// This function returns to the previously active scene.
        /// </summary>
        public void returnToPreviousScene()
        {
            futureScene = previousScene;
            transition = new ScreenTransition(this);
        }

        public Scene getScene()
        {
            return currentScene;
        }

        public override void update(GameTime gameTime)
        {
            currentScene.update(gameTime);

            if (transition != null)
            {
                transition.update(gameTime);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            currentScene.draw(spriteBatch);

            if (transition != null)
                transition.draw(spriteBatch);
        }

        public void transitionMiddle()
        {
            // Change to the next scene
            previousScene = currentScene;
            currentScene = futureScene;
        }

        public void transitionDone()
        {

        }
    }
}
