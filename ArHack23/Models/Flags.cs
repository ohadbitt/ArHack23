
using System.Numerics;

namespace ArHack23.Models
{
    public class Flags
    {
        public Vec3 RedFlagBaseLocation { get; set;}
        public Vec3 BlueFlagBaseLocation { get; set; }

    }
    public class Vec3
    {
        public Vec3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vec3()
        {

        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
