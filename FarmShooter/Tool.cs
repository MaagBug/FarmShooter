namespace FarmShooter
{
    enum ToolType { Hoe, Axe, Pickaxe, Any };

    class Tool : Handheld
    {
        public ToolType Type;
        public float Efficiency;

        public Tool(int id, string name, Texture text, Texture inv_text, ToolType type) : base(id, name, text, inv_text) 
        {
            Type = type;
        }

        public Tool(int id) : base(id, AllHandhelds[id].Name, AllHandhelds[id].MainSprite.Texture, AllHandhelds[id].InventorySprite.Texture)
        {
            Type = ((Tool)AllHandhelds[id]).Type;
            Efficiency = ((Tool)AllHandhelds[id]).Efficiency;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(MainSprite, states);
        }

        public override void Update()
        {
            MainSprite.Position = Owner.Handle.GetImagePoint(Owner.MainEntity.Sprite.Position, 0);
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
