using System;
using SFML.Graphics;
using SFML.System;

namespace FarmShooter
{
    abstract class Handheld : Drawable
    {
        public static event EventHandler<int> HandheldChanged;

        public Sprite MainSprite;

        public int ID 
        {
            get { return _ID; }
            set 
            {
                _ID = value;
                HandheldChanged?.Invoke(this, _ID);
            }
        }
        private int _ID;

        public Player Owner;

        public abstract void Draw(RenderTarget target, RenderStates states);
        public abstract void Update();
    }
}
