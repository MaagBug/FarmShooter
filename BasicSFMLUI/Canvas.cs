using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace BasicSFMLUI
{
    public class Canvas : Drawable
    {
        public List<(Vector2f, Transformable)> Items = new List<(Vector2f, Transformable)>();
        FloatRect Plane = new FloatRect(0, 0, 10, 10);

        public void SetPlaneCenter(Vector2f center)
        {
            Plane.Left = center.X - Plane.Width / 2;
            Plane.Top = center.Y - Plane.Height / 2;
        }

        public void SetPlaneSize(Vector2f size)
        {
            Plane.Width = size.X;
            Plane.Height = size.Y;

            foreach (var item in Items)
            {
                item.Item2.Position = new Vector2f(Plane.Width * item.Item1.X, Plane.Height * item.Item1.Y);
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var item in Items)
            {
                if (item.Item2 is Drawable) target.Draw(item.Item2 as Drawable, states);
            }
        }
    }
}
