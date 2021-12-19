namespace FarmShooter
{
    public enum ToolType { Hoe, Axe, Pickaxe, Any };

    public class Tool : Handheld
    {
        public ToolType Type;
        public float Efficiency;

        public Tool(int id, string name, Texture text, Texture inv_text, ToolType type) : base(id, name, text, inv_text) 
        {
            Type = type;
            Name = name;
            InventorySprite = new Sprite(inv_text);
        }

        public Tool(Tool copy) : base(copy)
        {
            Type = copy.Type;
            Efficiency = copy.Efficiency;
        }

        public Tool(int id) : base(id)
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
