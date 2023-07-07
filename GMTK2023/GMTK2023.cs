using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK2023
{
    public class GMTK2023 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Rectangle _screenRectangle;
        private RenderTarget2D _nativeRenderTarget;

        private Player player;
        private Shadow shadow;
        private Level the_level;
        private Camera cam;
        private ControllerManager contManager;

        public bool player_active
        { get; private set; }

        public GMTK2023()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            player_active = true;

            contManager = new ControllerManager();

            player = new Player(this, new Vector2(100, 100), contManager);
            shadow = new Shadow(this, new Vector2(100, 300), contManager);
            cam = new Camera();

            the_level = new Level(this, new Rectangle(0, 0, 480 * 2, 360), player, shadow, cam);

            player.current_level = the_level;

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

            _nativeRenderTarget = new RenderTarget2D(GraphicsDevice, 480, 360);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            player.Load();
            shadow.Load();
            the_level.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            contManager.GetInputs(Keyboard.GetState());

            if (contManager.SHIFT_PRESSED)
                player_active = !player_active;

            if (player_active)
            {
                player.Update(gameTime);
                shadow.Follow(player.DrawBox);
            }
            else
            {

            }
            

            int x_follow = player.DrawBox.X - 240 + 8;
            x_follow = Math.Clamp(x_follow, the_level.bounds.Left, the_level.bounds.Right - 480);

            cam.Follow(new Vector2(x_follow, 0));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, transformMatrix: cam.Transform);

            the_level.Draw(_spriteBatch);

            player.Draw(_spriteBatch);
            shadow.Draw(_spriteBatch);

            _spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_nativeRenderTarget, _screenRectangle, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}