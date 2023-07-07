using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK2023
{
    public class Shadow
    {
        private GMTK2023 root;
        private Vector2 pos;
        private ControllerManager contManager;

        //private Texture2D sheet;
        private Texture2D white;

        public Rectangle DrawBox
        { get { return new Rectangle((int)pos.X, (int)pos.Y, 16, 16); } }

        public Shadow(GMTK2023 root, Vector2 pos, ControllerManager contManager)
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
            _spriteBatch.Draw(white, DrawBox, Color.Black);
        }

        public void Follow(Rectangle player_rect)
        {
            pos.X = player_rect.X;

            int diff = Math.Abs(player_rect.Y - 220 + 15);

            pos.Y = 220 + diff;
        }
    }
}
