using System;
using SFML.System;
using SFML.Graphics;

namespace FarmShooter
{
    enum ToolType { Hoe, Axe, Pickaxe };

    class Tool : Handheld
    {
        public ToolType Type;
        public float Efficiency;

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(MainSprite, states);
        }

        public override void Update()
        {
            MainSprite.Position = Owner.Handle.GetImagePoint(Owner.MainEntity.Sprite.Position, Owner.MainEntity.Sprite.Rotation);
        }

        public void Interact(InteractableResource res) 
        {
            if (res != null) 
            {
                res.Interact(this);
            }
        }
    }
}
