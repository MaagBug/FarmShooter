namespace FarmShooter
{
    class ImagePoint
    {
        public Vector2f Point;

        public Vector2f GetImagePoint(Vector2f origin, float angle) 
        {
            return origin + new Vector2f(MathF.Cos(MathF.Atan2(Point.Y, Point.X) + angle / (180 / 3.14F)),
                                         MathF.Sin(MathF.Atan2(Point.Y, Point.X) + angle / (180 / 3.14F))) * Point.Length();
        }
    }
}
