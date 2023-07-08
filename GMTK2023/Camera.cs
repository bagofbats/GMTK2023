using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK2023
{
    public class Camera
    {
        float current_x = 0;
        float current_y = 0;
        public Level root;

        public Camera() { }

        public Matrix Transform { get; private set; }

        public void Follow(Vector2 target)
        {
            current_x = target.X;
            current_y = target.Y;

            var position = Matrix.CreateTranslation(
              -(int)target.X,
              -(int)target.Y,
              0);
            var offset = Matrix.CreateTranslation(
                0,
                0,
                0);
            Transform = position * offset;
        }

        public (float, float) GetPos()
        {
            return (current_x, current_y);
        }
    }
}
