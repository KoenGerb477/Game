using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class Vector
    {
        public float x;
        public float y;

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector Add(Vector v2)
        {
            return new Vector(x + v2.x, y + v2.y);
        }

        public Vector Multiply(float scalar)
        {
            return new Vector(x * scalar, y * scalar);
        }

    }
}
