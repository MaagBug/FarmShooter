namespace FarmShooter
{
    abstract class Item : Drawable
    {
        public int ID;
        public Sprite InventorySprite;
        public string Name;
        public List<string> ItemTags;

        public int Quantity;

        public Item(int ID) 
        {
            this.ID = ID;
        }

        public Item(string name, Texture inv_text) 
        {
            Name = name;
            InventorySprite = new Sprite(inv_text);
        }

        public virtual void Draw(RenderTarget target, RenderStates states) 
        {
            target.Draw(InventorySprite, states);
        }
    }
}
