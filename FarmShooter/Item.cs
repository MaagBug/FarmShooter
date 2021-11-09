namespace FarmShooter
{
    abstract class Item : Drawable
    {
        public int ID;
        public Sprite InventorySprite;
        public string Name;

        public int Quantity;

        public Item(int ID) 
        {
            this.ID = ID;
        }

        public Item(string name, string inv_text_name) 
        {
            Name = name;
            InventorySprite = new Sprite(Program.Textures[inv_text_name]);
        }

        public abstract void Draw(RenderTarget target, RenderStates states);

        public void Draw(RenderTexture texture, RenderStates states) 
        {
            texture.Draw(InventorySprite, states);
        }
    }
}
