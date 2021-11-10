namespace FarmShooter
{
    class ImagePoint
    {
        public Vector2f Point;

        public Vector2f GetImagePoint(Vector2f origin, float angle) 
        {
            //return origin + new Vector2f((float)Math.Cos(Math.Atan2(Point.Y, Point.X) * (180 / Math.PI)),
            //                             (float)Math.Sin(Math.Atan2(Point.Y, Point.X) * (180 / Math.PI))) * Point.Length();

            return origin + new Vector2f(MathF.Cos(MathF.Atan2(Point.Y, Point.X) + angle / (180 / 3.14F)),
                                         MathF.Sin(MathF.Atan2(Point.Y, Point.X) + angle / (180 / 3.14F))) * Point.Length();
        }
    }
}
