using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK2023
{
    public class Player
    {
        private GMTK2023 root;
        private Vector2 pos;

        //private Texture2D sheet;
        private Texture2D white;

        public Rectangle DrawBox
        { get { return new Rectangle((int)pos.X, (int)pos.Y, 16, 16); } }

        public Player(GMTK2023 root, Vector2 pos) 
        {
            this.root = root;
            this.pos = pos;
        }

        public void Load()
        {
            white = root.Content.Load<Texture2D>("black");
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(white, DrawBox, Color.White);
        }
    }
}
