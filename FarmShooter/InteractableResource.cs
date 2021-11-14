namespace FarmShooter
{
    abstract class InteractableResource : Drawable
    {
        public int ID;
        public string Name;
        public Sprite MainSprite;
        public int Durability;
        public int MaxDurability;

        public ToolType InteractToolType;

        public abstract void Draw(RenderTarget target, RenderStates states);

        public abstract void Interact(Tool tool);
    }
}
