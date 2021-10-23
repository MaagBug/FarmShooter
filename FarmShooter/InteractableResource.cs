using System;
using SFML.Graphics;
using SFML.System;

namespace FarmShooter
{
    abstract class InteractableResource : Drawable
    {
        public Sprite MainSprite;
        public int Durability;

        public abstract void Draw(RenderTarget target, RenderStates states);

        public abstract void Interact(Tool tool);
    }
}
