using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK2023
{
    public class Level
    {
        private GMTK2023 root;
        private Rectangle bounds;
        private Player player;
        private Camera cam;

        private int mirror = 220;
        private Texture2D white;

        public Level(GMTK2023 root, Rectangle bounds, Player player, Camera cam) 
        {
            this.root = root;
            this.bounds = bounds;
            this.player = player;
            this.cam = cam;
        }

        public void Load()
        {
            white = root.Content.Load<Texture2D>("black");
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            Rectangle mirror_rect = new Rectangle(0, mirror, bounds.Width, 1);
            _spriteBatch.Draw(white, mirror_rect, Color.White);
        }
    }
}
