namespace FarmShooter
{
    enum MaterialType { Wood, Stone, Iron, None }

    abstract class Item : Drawable
    {
        public int ID;
        public Sprite InventorySprite;
        public string Name;

        public MaterialType Material;

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

        public abstract void Draw(RenderTarget target, RenderStates states);

        public void Draw(RenderTexture texture, RenderStates states) 
        {
            texture.Draw(InventorySprite, states);
        }
    }
}
