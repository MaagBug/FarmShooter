using System;
using SFML.Graphics;
using SFML.System;

namespace FarmShooter
{
    class Entity : Drawable
    {
        public Sprite Sprite;
        public float Speed = 0;

        public Entity(Texture text) 
        {
            Sprite = new Sprite() { Texture = text, Origin = (Vector2f)(text.Size / 2) };
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Sprite, states);
        }
    }
}
