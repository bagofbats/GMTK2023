using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
        private List<Vector2> starting_pos= new List<Vector2>();
        public List<Key> keys = new List<Key>();

        public Level(GMTK2023 root, Rectangle bounds, Player player, Shadow shadow, Camera cam, List<Rectangle> walls, List<Vector2> doors, List<Vector2> starting_pos, int mirror) 
        {
            this.root = root;
            this.bounds = bounds;
            this.player = player;
            this.shadow = shadow;
            this.cam = cam;
            this.mirror = mirror;
            this.starting_pos = starting_pos;

            foreach (Rectangle wall in walls)
                this.walls.Add(wall);

            this.doors.Add(new Door(doors[0], true));
            this.doors.Add(new Door(doors[1], false));
            

            this.walls.Add(new Rectangle(bounds.X - 32, 0, 32, bounds.Height));
            this.walls.Add(new Rectangle(bounds.X + bounds.Width, 0, 32, bounds.Height));
        }

        public void Load(Texture2D white, Texture2D sheet)
        {
            this.white = white;

            doors[0].Load(sheet);
            doors[1].Load(sheet);
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            for (int i = 0; i < walls.Count; i++)
                _spriteBatch.Draw(white, walls[i], Color.LightGray);


            Rectangle mirror_rect = new Rectangle(0, mirror, bounds.Width, 1);
            _spriteBatch.Draw(white, mirror_rect, Color.White);

            doors[0].Draw(_spriteBatch);
            doors[1].Draw(_spriteBatch);

            foreach (Key key in keys)
                key.Draw(_spriteBatch);
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

        public void Initialize()
        {
            player.pos = starting_pos[0];
            shadow.pos = starting_pos[1];
            player.hsp = 0;
            shadow.hsp = 0;
            player.vsp = 0;
            shadow.vsp = 0;
        }

        public void AddKey(Key key)
        {
            this.keys.Add(key);
        }

        public void RemoveKey(Key key)
        {
            this.keys.Remove(key);
            root.sfx[2].Play();
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
        public bool locked = false;


        public Door(Vector2 pos, bool player_door) 
        { 
            this.pos = pos;
            this.player_door = player_door;
            bounds = new Rectangle((int)pos.X - 16, (int)pos.Y - 16, 32, 32);

            frame = new Rectangle(32, 0, 32, 32);
            if (!player_door)
                frame.Y = 32;
        }

        public void Load(Texture2D sheet)
        {
            this.sheet = sheet;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            if (locked && !player_door)
                frame.Y = 96;
            else if (locked)
                frame.Y = 64;
            else if (!player_door)
                frame.Y = 32;
            else
                frame.Y = 0;
            _spriteBatch.Draw(sheet, bounds, frame, Color.White);
        }
    }

    public class Key
    {
        public Vector2 pos;
        public bool player_half;

        private Texture2D sheet;
        private Rectangle bounds;
        private Rectangle frame;

        public Key(Vector2 pos, bool player_half) 
        { 
            this.pos = pos;
            this.player_half = player_half;

            bounds = new Rectangle((int)pos.X - 16, (int)pos.Y - 16, 32, 32);

            frame = new Rectangle(32, 128, 32, 32);
            if (!player_half)
                frame.Y = 160;
        }

        public void Load(Texture2D sheet)
        {
            this.sheet = sheet;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(sheet, bounds, frame, Color.White);
        }
    }
}
