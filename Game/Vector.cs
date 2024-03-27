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

        //method to add vectors together
        public Vector Add(Vector v2)
        {
            //(x1 + x2, y1 + y2)
            return new Vector(x + v2.x, y + v2.y);
        }

        //method to multiply vectors by a scalar
        public Vector Multiply(float scalar)
        {
            //(x * a, y * a)
            return new Vector(x * scalar, y * scalar);
        }

    }
}
