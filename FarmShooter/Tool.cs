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

        public Tool(int id) : base(id, ((Tool)AllItems[id]).Name, ((Tool)AllItems[id]).MainSprite.Texture, ((Tool)AllItems[id]).InventorySprite.Texture)
        {
            Type = ((Tool)AllItems[id]).Type;
            Efficiency = ((Tool)AllItems[id]).Efficiency;
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
