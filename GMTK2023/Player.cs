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
        private Vector2 pos;
        private ControllerManager contManager;
        public Level current_level;

        //private Texture2D sheet;
        private Texture2D white;


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
        private float vsp = 0f;
        private int hdir = 0;
        private int last_hdir = 1;
        private float hsp_max = 2f;
        private float grav = 0.211f;


        public Rectangle DrawBox
        { get { return new Rectangle((int)pos.X, (int)pos.Y, 16, 16); } }

        public Player(GMTK2023 root, Vector2 pos, ControllerManager contManager) 
        {
            this.root = root;
            this.pos = pos;
            this.contManager = contManager;
        }

        public void Load()
        {
            white = root.Content.Load<Texture2D>("black");
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(white, DrawBox, Color.White);
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

            Rectangle hcheck = current_level.SimpleCheckCollision(new Rectangle((int)(DrawBox.X + hsp_col_check), DrawBox.Y, DrawBox.Width, DrawBox.Height));

            if (hcheck != new Rectangle(0, 0, 0, 0))
            {
                if (hsp > 0)
                    pos.X = hcheck.Left - 16;
                else if (hsp < 0)
                    pos.X = hcheck.Right;
                hsp = 0;
            }


            pos.X += hsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;


            // ---- vertical movement ----
            vsp += grav;

            if (space_pressed && pos.Y <= 220 - 16)
                vsp = -4.2f;

            if (space_released && vsp < 0)
                vsp /= 2;

            float vsp_col_check = vsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
            if (vsp_col_check > 0)
                vsp_col_check += 1;
            else
                vsp_col_check -= 1;

            Rectangle vcheck = current_level.SimpleCheckCollision(new Rectangle(DrawBox.X, (int)(DrawBox.Y + vsp_col_check), DrawBox.Width, DrawBox.Height));

            if (vcheck != new Rectangle(0, 0, 0, 0))
            {
                if (vsp < 0)
                    pos.Y = vcheck.Bottom;
                else if (vsp > 0)
                    pos.Y = vcheck.Top - 16;
                vsp = 0;
            }


            pos.Y += vsp * (float)gameTime.ElapsedGameTime.TotalSeconds * 60;

            if (pos.Y >= 220 - 16)
                pos.Y = 220 - 16;
        }

        public void Follow(Rectangle player_rect)
        {
            pos.X = player_rect.X;

            int diff = Math.Abs(player_rect.Y - 220);

            pos.Y = 220 - diff - 15;
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
