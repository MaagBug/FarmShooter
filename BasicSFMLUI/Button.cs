using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace BasicSFMLUI
{
    public class Button
    {
        public event EventHandler Clicked;

        public Sprite MainSprite;

        public Texture MainTexture;
        public Texture HoverTexture;
        public Texture ClickTexture;

        public Window ContainingWindow;

        private bool is_being_pressed = false;

        public Button(Texture main, Texture hover, Texture click) 
        {
            MainTexture = main;
            HoverTexture = hover;
            ClickTexture = click;

            MainSprite = new Sprite(MainTexture);

            ContainingWindow.MouseButtonPressed += (o, e) => 
            {
                if (MainSprite.GetGlobalBounds().Contains(e.X, e.Y)) is_being_pressed = true;
                MainSprite.Texture = ClickTexture;
            };

            ContainingWindow.MouseButtonReleased += (o, e) => 
            {
                if (MainSprite.GetGlobalBounds().Contains(e.X, e.Y) && is_being_pressed) Clicked?.Invoke(this, new EventArgs());
                MainSprite.Texture = MainTexture;
                is_being_pressed = false;
            };

            ContainingWindow.MouseMoved += (o, e) => 
            {
                if (!is_being_pressed && MainSprite.GetGlobalBounds().Contains(e.X, e.Y)) MainSprite.Texture = MainTexture;
            };
        }
    }
}
