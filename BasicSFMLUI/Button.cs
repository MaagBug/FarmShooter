using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace BasicSFMLUI
{
    public class Button
    {
        public event EventHandler Clicked;
        public bool IsBeingPressed = false;

        public FloatRect BoundingBox;

        public Window ContainingWindow;

        public Button(Window cw, FloatRect bb) 
        {
            ContainingWindow = cw;

            BoundingBox = bb;

            ContainingWindow.MouseButtonPressed += (o, e) => 
            {
                if (BoundingBox.Contains(e.X, e.Y)) IsBeingPressed = true;
            };

            ContainingWindow.MouseButtonReleased += (o, e) => 
            {
                if (BoundingBox.Contains(e.X, e.Y) && IsBeingPressed) Clicked?.Invoke(this, new EventArgs());
                IsBeingPressed = false;
            };
        }
    }
}
