﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK2023
{
    public class Player
    {
        private GMTK2023 root;
        public Vector2 pos;
        private ControllerManager contManager;
        public Shadow shadow;

        private Texture2D sheet;
        private Texture2D white;
        public Rectangle frame = new Rectangle(0, 0, 32, 32);


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
        public float hsp = 0f;
        public float vsp = 0f;
        private int hdir = 0;
        public int last_hdir = 1;
        private float hsp_max = 1f;
        private float grav = 0.211f;
        private bool wall_down = true;


        public Rectangle DrawBox
        { get { return new Rectangle((int)pos.X, (int)pos.Y, 32, 32); } }

        public Rectangle HitBox
        { get { return new Rectangle((int)pos.X + 10, (int)pos.Y + 12, 12, 20); } }

        public Player(GMTK2023 root, Vector2 pos, ControllerManager contManager) 
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
            float transparency = 1f;
            if (!root.player_active)
                transparency = 0.7f;

            if (root.player_ready)
                frame.Y = 128;
            else
                frame.Y = 0;

            if (last_hdir == -1)
                frame.Y += 64;

            if (!wall_down && pos.Y < root.current_level.mirror - 32)
                frame.X = 96;
            else if (hsp != 0)
                frame.X = 64 + (32 * ((int)root.walk_timer % 4));
            else
                frame.X = 0;

            

            if (!root.player_active)
                frame.X = shadow.frame.X;

            _spriteBatch.Draw(sheet, DrawBox, frame, Color.White * transparency);
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

            Rectangle hcheck = root.current_level.SimpleCheckCollision(new Rectangle((int)(HitBox.X + hsp_col_check), HitBox.Y, HitBox.Width, HitBox.Height));

            int diff = 0;

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

            (float cam_x, float cam_y) = root.cam.GetPos();

            if (shadow.pos.X >= cam_x + 298)
                hsp = Math.Min(hsp, 0);

            if (shadow.pos.X <= cam_x - 10)
                hsp = Math.Max(hsp, 0);


            pos.X += (int)(hsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60);
            shadow.pos.X += (int)(diff + (hsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60));


            // ---- vertical movement ----
            vsp += grav;

            wall_down = root.current_level.WallBelow(HitBox);

            if (space_pressed && (pos.Y >= root.current_level.mirror - 32 || wall_down))
            {
                vsp = -2.8f;
                root.sfx[1].Play();
            }
                

            if (space_released && vsp < 0)
                vsp /= 2;

            float vsp_col_check = vsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
            if (vsp_col_check > 0)
                vsp_col_check += 1;
            else
                vsp_col_check -= 1;

            Rectangle vcheck = root.current_level.SimpleCheckCollision(new Rectangle(HitBox.X, (int)(HitBox.Y + vsp_col_check), HitBox.Width, HitBox.Height));

            if (vcheck != new Rectangle(0, 0, 0, 0))
            {
                if (vsp < 0)
                    pos.Y = vcheck.Bottom;
                else if (vsp > 0)
                    pos.Y = vcheck.Top - 32;
                vsp = 0;
            }


            pos.Y += vsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;

            if (pos.Y >= root.current_level.mirror - 32)
                pos.Y = root.current_level.mirror - 32;

            // misc
            var door = root.current_level.doors[0];
            if (HitBox.Contains(door.pos) && !door.locked)
                root.player_ready = true;
            else
                root.player_ready = false;

            for (int i = root.current_level.keys.Count - 1; i >= 0; i--)
                if (HitBox.Contains(root.current_level.keys[i].pos) && root.current_level.keys[i].player_half)
                    root.current_level.RemoveKey(root.current_level.keys[i]);
        }

        public void Follow(Rectangle player_rect)
        {
            // pos.X = player_rect.X;

            int diff = Math.Abs(player_rect.Y - root.current_level.mirror);

            pos.Y = root.current_level.mirror - diff - 31;

            last_hdir = -1 * shadow.last_hdir;

            var door = root.current_level.doors[0];
            if (HitBox.Contains(door.pos))
                root.player_ready = true;
            else
                root.player_ready = false;
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
