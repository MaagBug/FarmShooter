namespace FarmShooter
{
    public class ImagePoint
    {
        public Vector2f Point;
        public float Angle;

        public Vector2f GetImagePoint(Vector2f origin) 
        {
            return origin + new Vector2f(MathF.Cos(MathF.Atan2(Point.Y, Point.X) + Angle / (180 / 3.14F)),
                                         MathF.Sin(MathF.Atan2(Point.Y, Point.X) + Angle / (180 / 3.14F))) * Point.Length();
        }
    }
}
