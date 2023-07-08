using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK2023
{
    public class Shadow
    {
        private GMTK2023 root;
        public Vector2 pos;
        private ControllerManager contManager;
        public Level current_level;
        public Player player;

        private Texture2D sheet;
        private Texture2D white;
        private Rectangle frame = new Rectangle(0, 32, 32, 32);

        // input fields
        private bool up;
        private bool down;
        private bool left;
        private bool right;
        private bool space;
        private bool enter;
        private bool space_released;
        private bool space_pressed;
        private bool enter_pressed;
        private bool enter_released;

        // gameplay fields
        private float hsp = 0f;
        public float vsp = 0f;
        private int hdir = 0;
        private int last_hdir = 1;
        private float hsp_max = 2f;
        private float grav = 0.211f;

        public Rectangle DrawBox
        { get { return new Rectangle((int)pos.X, (int)pos.Y, 32, 32); } }

        public Rectangle HitBox
        { get { return new Rectangle((int)pos.X + 10, (int)pos.Y, 12, 20); } }

        public Shadow(GMTK2023 root, Vector2 pos, ControllerManager contManager)
        {
            this.root = root;
            this.pos = pos;
            this.contManager = contManager;
        }

        public void Load()
        {
            white = root.Content.Load<Texture2D>("black");
            sheet = root.Content.Load<Texture2D>("gmtk2023_sheet");
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(sheet, DrawBox, frame, Color.White);
            //_spriteBatch.Draw(white, HitBox, Color.Blue * 0.4f);
        }

        public void Update(GameTime gameTime)
        {
            GetInput();

            // ---- horizontal movement ----
            hsp = hsp_max * hdir;

            float hsp_col_check = hsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
            if (hsp_col_check > 0)
                hsp_col_check += 1;
            else
                hsp_col_check -= 1;

            Rectangle hcheck = current_level.SimpleCheckCollision(new Rectangle((int)(HitBox.X + hsp_col_check), HitBox.Y, HitBox.Width, HitBox.Height));

            float diff = 0;

            if (hcheck != new Rectangle(0, 0, 0, 0))
            {
                if (hsp > 0)
                {
                    diff = (int)Math.Abs(pos.X - hcheck.Left + 32 - 10);
                    pos.X = hcheck.Left - 32 + 10;
                }
                else if (hsp < 0)
                {
                    diff = -1 * (int)Math.Abs(pos.X - hcheck.Right + 10);
                    pos.X = hcheck.Right - 10;
                }
                    
                hsp = 0;
            }


            pos.X += hsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
            player.pos.X -= diff + hsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;


            // ---- vertical movement ----
            vsp -= grav;

            if (space_pressed && pos.Y >= 221)
                vsp = 4.2f;

            if (space_released && vsp > 0)
                vsp /= 2;

            float vsp_col_check = vsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
            if (vsp_col_check > 0)
                vsp_col_check += 1;
            else
                vsp_col_check -= 1;

            Rectangle vcheck = current_level.SimpleCheckCollision(new Rectangle(HitBox.X, (int)(HitBox.Y + vsp_col_check), HitBox.Width, HitBox.Height));

            if (vcheck != new Rectangle(0, 0, 0, 0))
            {
                if (vsp < 0)
                    pos.Y = vcheck.Bottom;
                else if (vsp > 0)
                    pos.Y = vcheck.Top - 32;
                vsp = 0;
            }


            pos.Y += vsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;

            if (pos.Y <= 221)
                pos.Y = 221;
        }

        public void Follow(Rectangle player_rect)
        {
            // pos.X = player_rect.X;

            int diff = Math.Abs(player_rect.Y - 220 + 31);

            pos.Y = 220 + diff;
        }

        private void GetInput()
        {
            up = contManager.UP;
            down = contManager.DOWN;
            left = contManager.LEFT;
            right = contManager.RIGHT;
            space = contManager.SPACE;
            enter = contManager.ENTER;

            space_pressed = contManager.SPACE_PRESSED;
            space_released = contManager.SPACE_RELEASED;
            enter_pressed = contManager.ENTER_PRESSED;
            enter_released = contManager.ENTER_RELEASED;

            hdir = (int)(Convert.ToSingle(right) - Convert.ToSingle(left));
            if (hdir != 0)
                last_hdir = hdir;
        }
    }
}
