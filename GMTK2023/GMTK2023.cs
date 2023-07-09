﻿using Microsoft.Xna.Framework;
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
        private Level lvl1;
        private Level lvl0;
        private Level lvl2;
        private Level lvl3;
        private Level lvl4;

        private Dictionary<Level, Level> lvl_map = new Dictionary<Level, Level>();

        public bool player_ready = false;
        public bool shadow_ready = false;


        public Level current_level
        { get; private set; }

        public Camera cam
        { get; private set; }
        private ControllerManager contManager;

        private Texture2D white;
        private Texture2D sheet;

        /**
        private List<Rectangle> lvlx_walls = new List<Rectangle>();
        private List<Vector2> lvlx_doors = new List<Vector2>();
        private List<Vector2> lvlx_starting_pos = new List<Vector2>();
        private int lvlx_mirror = 140;
        */

        private List<Rectangle> lvl0_walls = new List<Rectangle>()
        {
            new Rectangle(156, 140 - 16, 300, 24)
        };
        private List<Vector2> lvl0_doors = new List<Vector2>()
        {
            new Vector2(240, 140 - 48),
            new Vector2(240, 140 + 48)
        };
        private List<Vector2> lvl0_starting_pos = new List<Vector2>()
        {
            new Vector2(32, 100),
            new Vector2(32, 200)
        };
        private int lvl0_mirror = 140;

        private List<Rectangle> lvl2_walls = new List<Rectangle>()
        {
            new Rectangle(0, 140 - 12, 320, 25),
            new Rectangle(152, 64, 16, 64)
        };
        private List<Vector2> lvl2_doors = new List<Vector2>()
        {
            new Vector2(256, 140 - 32),
            new Vector2(96, 140 + 32)
        };
        private List<Vector2> lvl2_starting_pos = new List<Vector2>()
        {
            new Vector2(32, 100),
            new Vector2(32, 200)
        };
        private int lvl2_mirror = 140;

        private List<Rectangle> lvl3_walls = new List<Rectangle>()
        {
            new Rectangle(0, 140 - 4, 320, 9),
            new Rectangle(110, 140 - 12, 320, 8),
            new Rectangle(152, 64, 16, 140)
        };
        private List<Vector2> lvl3_doors = new List<Vector2>()
        {
            new Vector2(256, 140 - 27),
            new Vector2(256, 140 + 28)
        };
        private List<Vector2> lvl3_starting_pos = new List<Vector2>()
        {
            new Vector2(32, 140 - 32 - 8),
            new Vector2(32, 140 + 8)
        };
        private int lvl3_mirror = 140;

        private List<Rectangle> lvl1_walls = new List<Rectangle>()
        {
            new Rectangle(140, 110, 32, 48)
        };
        private List<Vector2> lvl1_doors = new List<Vector2>()
        {
            new Vector2(156, 74),
            new Vector2(156, 206)
        };
        private List<Vector2> lvl1_starting_pos = new List<Vector2>()
        {
            new Vector2(64, 140 - 32),
            new Vector2(64, 141)
        };
        private int lvl1_mirror = 140;

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

            lvl1 = new Level(this, new Rectangle(0, 0, 320, 240), player, shadow, cam, lvl1_walls, lvl1_doors, lvl1_starting_pos, lvl1_mirror);
            lvl0 = new Level(this, new Rectangle(0, 0, 320, 240), player, shadow, cam, lvl0_walls, lvl0_doors, lvl0_starting_pos, lvl0_mirror);
            lvl2 = new Level(this, new Rectangle(0, 0, 320, 240), player, shadow, cam, lvl2_walls, lvl2_doors, lvl2_starting_pos, lvl2_mirror);
            lvl3 = new Level(this, new Rectangle(0, 0, 320, 240), player, shadow, cam, lvl3_walls, lvl3_doors, lvl3_starting_pos, lvl3_mirror);

            lvl_map.Add(lvl0, lvl2);
            lvl_map.Add(lvl2, lvl3);
            lvl_map.Add(lvl3, lvl1);

            current_level = lvl0;
            current_level.Initialize();
            player.shadow = shadow;
            shadow.player = player;

            // determine how much to scale the window up
            // given how big the monitor is
            int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            int target_w = 320;
            int target_h = 240;
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

            white = Content.Load<Texture2D>("black");
            sheet = Content.Load<Texture2D>("gmtk2023_sheet");

            player.Load();
            shadow.Load();
            current_level.Load(white, sheet);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            contManager.GetInputs(Keyboard.GetState());

            if (contManager.SHIFT_PRESSED)
            {
                if (player_active)
                {
                    Rectangle check = current_level.SimpleCheckCollision(shadow.HitBox);
                    if (check == new Rectangle(0, 0, 0, 0))
                    {
                        player_active = false;
                        shadow.vsp = -player.vsp;
                    }
                        
                }
                else
                {
                    Rectangle check = current_level.SimpleCheckCollision(player.HitBox);
                    if (check == new Rectangle(0, 0, 0, 0))
                    {
                        player_active = true;
                        player.vsp = -shadow.vsp;
                    }
                        
                }
            }
                

            if (player_active)
            {
                player.Update(gameTime);
                shadow.Follow(player.DrawBox);
            }
            else
            {
                shadow.Update(gameTime);
                player.Follow(shadow.DrawBox);
            }

            if (player_ready && shadow_ready)
            {
                current_level = lvl_map[current_level];
                current_level.Initialize();
                current_level.Load(white, sheet);
                player_ready = false;
                shadow_ready = false;
            }

            int x_follow;
            int y_follow;

            if (player_active)
            {
                x_follow = player.DrawBox.X - 160 + 16;
                y_follow = 0;
            }
            else
            {
                x_follow = shadow.DrawBox.X - 160 + 16;
                y_follow = 0;
            }
                

            x_follow = Math.Clamp(x_follow, current_level.bounds.Left, current_level.bounds.Right - 320);

            cam.Follow(new Vector2(x_follow, y_follow));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_nativeRenderTarget);
            GraphicsDevice.Clear(Color.DimGray);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, transformMatrix: cam.Transform);

            current_level.Draw(_spriteBatch);

            player.Draw(_spriteBatch);
            shadow.Draw(_spriteBatch);

            (float cam_x, float cam_y) = cam.GetPos();

            
            if (player_active)
                _spriteBatch.Draw(white, new Rectangle((int)cam_x, (int)cam_y + current_level.mirror, 320, 240), Color.Indigo * 0.3f);

            else
                _spriteBatch.Draw(white, new Rectangle((int)cam_x, (int)cam_y, 320, current_level.mirror), Color.LightBlue * 0.3f);
            
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