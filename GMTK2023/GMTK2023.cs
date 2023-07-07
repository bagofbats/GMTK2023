using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GMTK2023
{
    public class GMTK2023 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Rectangle _screenRectangle;
        private RenderTarget2D _nativeRenderTarget;

        private Player player;
        private Level the_level;

        public GMTK2023()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            player = new Player(this, new Vector2(100, 100));
            Camera cam = new Camera();

            the_level = new Level(this, new Rectangle(0, 0, 480, 360), player, cam);
            


            // determine how much to scale the window up
            // given how big the monitor is
            int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            int target_w = 480;
            int target_h = 360;
            int scale = 1;

            while (target_w * (scale + 1) < w && target_h * (scale + 1) < h)
                scale++;

            target_w *= scale;
            target_h *= scale;

            _graphics.PreferredBackBufferWidth = target_w;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = target_h;   // set this value to the desired height of your window

            _screenRectangle = new Rectangle(0, 0, target_w, target_h);

            _graphics.ApplyChanges();

            

            Window.Title = "Shadow Twin";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            _nativeRenderTarget = new RenderTarget2D(GraphicsDevice, 320, 240);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            player.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
            GraphicsDevice.Clear(Color.DarkSlateGray);

            GraphicsDevice.SetRenderTarget(null);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_nativeRenderTarget, _screenRectangle, Color.White);

            player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}