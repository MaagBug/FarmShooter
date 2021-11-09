namespace FarmShooter
{
    internal class Plant : InteractableResource
    {
        public int Stage = 0;
        public readonly Item HarvestResource;

        public Plant(string text_name, int max_durability, ToolType type) 
        {
            MainSprite = new Sprite(Program.Textures[text_name]);
            MainSprite.Origin = new Vector2f(MainSprite.Texture.Size.X / 2, MainSprite.Texture.Size.Y);
            MaxDurability = max_durability;
            Durability = max_durability;
            InteractToolType = type;
        }

        public Plant(Plant copy) 
        {
            ID = copy.ID;
            MainSprite = new Sprite(copy.MainSprite);
            HarvestResource = copy.HarvestResource;
            MaxDurability = copy.MaxDurability;
            Durability = copy.Durability;
            InteractToolType = copy.InteractToolType;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(MainSprite, states);
        }

        public override void Interact(Tool tool)
        {
            throw new NotImplementedException();
        }
    }
}
