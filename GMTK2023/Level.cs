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

        public Rectangle bounds
        { get; private set; }

        private GMTK2023 root;
        private Player player;
        private Shadow shadow;
        private Camera cam;

        private int mirror = 220;
        private Texture2D white;

        private List<Rectangle> walls = new List<Rectangle>();

        public Level(GMTK2023 root, Rectangle bounds, Player player, Shadow shadow, Camera cam) 
        {
            this.root = root;
            this.bounds = bounds;
            this.player = player;
            this.shadow = shadow;
            this.cam = cam;

            walls.Add(new Rectangle(300, 220 - 24, 96, 48));
            walls.Add(new Rectangle(396, 220 - 24, 96, 24));
            walls.Add(new Rectangle(396, 220, 96, 8));
            walls.Add(new Rectangle(396 + 96, 220 - 8, 64, 16));
        }

        public void Load()
        {
            white = root.Content.Load<Texture2D>("black");
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            for (int i = 0; i < walls.Count; i++)
                _spriteBatch.Draw(white, walls[i], Color.Gray);


            Rectangle mirror_rect = new Rectangle(0, mirror, bounds.Width, 1);
            _spriteBatch.Draw(white, mirror_rect, Color.White);
        }

        public Rectangle SimpleCheckCollision(Rectangle input)
        {
            for (int i = 0; i < walls.Count(); i++)
            {
                if (walls[i].Intersects(input))
                {
                    return walls[i];
                }
            }
            return new Rectangle(0,0,0,0);
        }
    }
}
