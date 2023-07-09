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
        private Level lvl1;
        private Level lvl0;
        private Level lvl2;
        private Level lvl3;
        private Level lvl4;
        private Level lvl5;
        private Level lvl6;
        private Level lvl_final;

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

        public float walk_timer = 0;

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
            new Rectangle(0, 140 - 12, 320, 8),
            new Rectangle(152, 64, 16, 140)
        };
        private List<Vector2> lvl3_doors = new List<Vector2>()
        {
            new Vector2(256, 140 - 27),
            new Vector2(256, 140 + 28)
        };
        private List<Vector2> lvl3_starting_pos = new List<Vector2>()
        {
            new Vector2(32, 140 - 32 - 16),
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

        private List<Rectangle> lvl4_walls = new List<Rectangle>()
        {
            new Rectangle(96 + 16, 120 - 16, 16, 8),
            new Rectangle(120 + 16, 121 + 16, 16, 8),
            new Rectangle(136 + 16, 0, 8, 120),
            new Rectangle(144 + 16, 121 - 32, 32, 32)
        };
        private List<Vector2> lvl4_doors = new List<Vector2>()
        {
            new Vector2(16, 120 - 16),
            new Vector2(16, 120 + 16)
        };
        private List<Vector2> lvl4_starting_pos = new List<Vector2>()
        {
            new Vector2(32, 120 - 32),
            new Vector2(32, 121)
        };
        private int lvl4_mirror = 120;

        private List<Rectangle> lvl5_walls = new List<Rectangle>()
        {
            new Rectangle(152, 32, 16, 140 - 32),
            new Rectangle(152, 132, 320, 8)
        };
        private List<Vector2> lvl5_doors = new List<Vector2>()
        {
            new Vector2(64, 140 - 16),
            new Vector2(64, 141 + 16)
        };
        private List<Vector2> lvl5_starting_pos = new List<Vector2>()
        {
            new Vector2(16, 140 - 32),
            new Vector2(16, 141)
        };
        private int lvl5_mirror = 140;

        private List<Rectangle> lvl6_walls = new List<Rectangle>()
        {
            new Rectangle(80, 0, 16, 240),
            new Rectangle(320 - 64 - 32, 0, 16, 240),
            new Rectangle(80, 120 - 16, 156, 16),
            new Rectangle(160, 120, 64, 32),
            new Rectangle(80, 120 + 24, 80, 120)
        };
        private List<Vector2> lvl6_doors = new List<Vector2>()
        {
            new Vector2(320 - 48, 120 - 16),
            new Vector2(48, 121 + 16)
        };
        private List<Vector2> lvl6_starting_pos = new List<Vector2>()
        {
            new Vector2(32, 120 - 32),
            new Vector2(320 - 64, 121)
        };
        private int lvl6_mirror = 120;

        private List<Rectangle> lvlx_walls = new List<Rectangle>();
        private List<Vector2> lvlx_doors = new List<Vector2>()
        {
            new Vector2(600, 0),
            new Vector2(600, 0)
        };
        private List<Vector2> lvlx_starting_pos = new List<Vector2>()
        {
            new Vector2(160, 140 - 32),
            new Vector2(160, 141)
        };
        private int lvlx_mirror = 140;

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
            lvl4 = new Level(this, new Rectangle(0, 0, 320, 240), player, shadow, cam, lvl4_walls, lvl4_doors, lvl4_starting_pos, lvl4_mirror);
            lvl5 = new Level(this, new Rectangle(0, 0, 320, 240), player, shadow, cam, lvl5_walls, lvl5_doors, lvl5_starting_pos, lvl5_mirror);
            lvl6 = new Level(this, new Rectangle(0, 0, 320, 240), player, shadow, cam, lvl6_walls, lvl6_doors, lvl6_starting_pos, lvl6_mirror);
            lvl_final = new Level(this, new Rectangle(0, 0, 320, 240), player, shadow, cam, lvlx_walls, lvlx_doors, lvlx_starting_pos, lvlx_mirror);

            lvl_map.Add(lvl0, lvl2);
            lvl_map.Add(lvl2, lvl3);
            lvl_map.Add(lvl3, lvl1);
            lvl_map.Add(lvl1, lvl5);
            lvl_map.Add(lvl5, lvl4);
            lvl_map.Add(lvl4, lvl6);
            lvl_map.Add(lvl6, lvl_final);

            current_level = lvl0;
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
            LevelGoto(current_level);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                LevelGoto(current_level);

            // TODO: Add your update logic here

            walk_timer += 10 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            current_level.doors[0].locked = current_level.keys.Count > 0;
            current_level.doors[1].locked = current_level.keys.Count > 0;

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

                LevelGoto(current_level);
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

        public void LevelGoto(Level lvl)
        {
            lvl.Initialize();
            lvl.Load(white, sheet);
            player_ready = false;
            shadow_ready = false;

            if (lvl == lvl4)
            {
                Key key = new Key(new Vector2(184, 64), true);
                key.Load(sheet);
                lvl.AddKey(key);
            }

            if (lvl == lvl5)
            {
                Key key = new Key(new Vector2(184, 184), false);
                key.Load(sheet);
                lvl.AddKey(key);
            }
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