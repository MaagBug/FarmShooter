using SFML.System;
using System;

namespace FarmShooter
{
    public static class Vector2Extension
    {
        public static float Length(this Vector2f v) 
        {
            return (float)Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2));
        }

        public static Vector2f Normalized(this Vector2f v) 
        {
            if (v.Length() == 0) return new Vector2f(0, 0);
            return v / v.Length();
        }
    }
}
