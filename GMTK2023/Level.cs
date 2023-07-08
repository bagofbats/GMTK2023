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

        public int mirror;
        private Texture2D white;

        private List<Rectangle> walls = new List<Rectangle>();
        public List<Door> doors = new List<Door>();

        public Level(GMTK2023 root, Rectangle bounds, Player player, Shadow shadow, Camera cam, List<Rectangle> walls, List<Vector2> doors, List<Vector2> starting_pos, int mirror) 
        {
            this.root = root;
            this.bounds = bounds;
            this.player = player;
            this.shadow = shadow;
            this.cam = cam;
            this.mirror = mirror;

            player.pos = starting_pos[0];
            shadow.pos = starting_pos[1];

            foreach (Rectangle wall in walls)
                this.walls.Add(wall);

            this.doors.Add(new Door(doors[0], true));
            this.doors.Add(new Door(doors[1], false));
            

            this.walls.Add(new Rectangle(bounds.X - 32, 0, 32, bounds.Height));
            this.walls.Add(new Rectangle(bounds.X + bounds.Width, 0, 32, bounds.Height));
        }

        public void Load()
        {
            white = root.Content.Load<Texture2D>("black");

            doors[0].Load(root);
            doors[1].Load(root);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            for (int i = 0; i < walls.Count; i++)
                _spriteBatch.Draw(white, walls[i], Color.LightGray);


            Rectangle mirror_rect = new Rectangle(0, mirror, bounds.Width, 1);
            _spriteBatch.Draw(white, mirror_rect, Color.White);

            doors[0].Draw(_spriteBatch);
            doors[1].Draw(_spriteBatch);
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

        public bool WallBelow(Rectangle input)
        {
            Rectangle check = new Rectangle(input.X, input.Y + 1, input.Width, input.Height);

            for (int i = 0; i < walls.Count(); i++)
            {
                if (walls[i].Intersects(check))
                {
                    return true;
                }
            }
            return false;
        }

        public bool WallAbove(Rectangle input)
        {
            Rectangle check = new Rectangle(input.X, input.Y - 1, input.Width, input.Height);

            for (int i = 0; i < walls.Count(); i++)
            {
                if (walls[i].Intersects(check))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Door
    {
        public Vector2 pos
        { get; private set; }
        private bool player_door;
        private Texture2D sheet;
        private Rectangle bounds;
        private Rectangle frame;


        public Door(Vector2 pos, bool player_door) 
        { 
            this.pos = pos;
            this.player_door = player_door;
            bounds = new Rectangle((int)pos.X - 16, (int)pos.Y - 16, 32, 32);

            frame = new Rectangle(32, 0, 32, 32);
            if (!player_door)
                frame.Y = 32;
        }

        public void Load(GMTK2023 root)
        {
            sheet = root.Content.Load<Texture2D>("gmtk2023_sheet");
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(sheet, bounds, frame, Color.White);
        }
    }
}
