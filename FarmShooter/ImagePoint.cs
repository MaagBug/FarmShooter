namespace FarmShooter
{
    class ImagePoint
    {
        public Vector2f Point;

        public Vector2f GetImagePoint(Vector2f origin, float angle) 
        {
            return origin + new Vector2f((float)Math.Cos(Math.Atan2(Point.Y, Point.X) / (180 / 3.14) + angle / (180 / 3.14)),
                                      (float)Math.Sin(Math.Atan2(Point.Y, Point.X) / (180 / 3.14) + angle / (180 / 3.14))) * (float)Point.Length();
        }
    }
}
