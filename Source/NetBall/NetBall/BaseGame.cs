using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetBall.Helpers;
using NetBall.Scenes;

namespace NetBall
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class BaseGame : Game
    {
        public static BaseGame instance;

        public GraphicsDeviceManager graphics { get; }
        private SpriteBatch spriteBatch;
        public RenderTarget2D renderTarget { get; set; }

        private SceneManager sceneManager;

        public BaseGame()
        {
            instance = this;

            graphics = new GraphicsDeviceManager(this);

            // Set the default screen size
            graphics.PreferredBackBufferWidth = (int)ScreenHelper.VIEW_SIZE.X;
            graphics.PreferredBackBufferHeight = (int)ScreenHelper.VIEW_SIZE.Y;

            IsMouseVisible = true;
            //graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create the render target for scaling
            renderTarget = new RenderTarget2D(GraphicsDevice, (int)ScreenHelper.SCREEN_SIZE.X, (int)ScreenHelper.SCREEN_SIZE.Y, true, SurfaceFormat.Color, DepthFormat.Depth16, 1, RenderTargetUsage.PreserveContents);

            // Create the initial game scene
            sceneManager = new SceneManager(Content, GraphicsDevice);
            sceneManager.setScene(new GameScene(Content));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update the current scene
            sceneManager.update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Set the render target
            GraphicsDevice.SetRenderTarget(renderTarget);

            GraphicsDevice.Clear(Color.DarkGray);

            // Draw the current scene
            sceneManager.draw(spriteBatch);

            // Reset the render target and draw to the screen
            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, (int)ScreenHelper.VIEW_SIZE.X, (int)ScreenHelper.VIEW_SIZE.Y), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
