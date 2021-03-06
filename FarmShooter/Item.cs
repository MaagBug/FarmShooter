namespace FarmShooter
{
    public class Item : Drawable
    {
        public static List<Item> AllItems = new List<Item>();

        public static void LoadItems(string json) 
        {
            AllItems.Clear();

            DataTable table = JsonConvert.DeserializeObject<DataTable>(json);

            foreach (DataRow row in table.Rows) 
            {
                Item added;

                if (((string[])row.ItemArray[3]).ToList().Contains("Handheld")) 
                {
                    added = Handheld.DeserializeHandheld(row);
                }
                else
                {
                    added = new Item((int)(long)row.ItemArray[0], (string)row.ItemArray[2], Program.Textures.GetValueOrSpecificDefault((string)row.ItemArray[1], Program.Textures["MISSING_TEXTURE"])) { ItemTags = ((string[])row.ItemArray[3]).ToList() };
                    if ((bool)row.ItemArray[4]) added.Quantity = 1;
                    else added.Quantity = 0;
                }

                AllItems.Add(added);
            }
        }

        public int ID;
        public Sprite InventorySprite;
        public string Name;
        public List<string> ItemTags = new List<string>();

        public int Quantity;

        public Item(int ID) 
        {
            this.ID = ID;
            InventorySprite = new Sprite(AllItems[ID].InventorySprite);
            Name = AllItems[ID].Name;
            ItemTags = AllItems[ID].ItemTags;
            Quantity = AllItems[ID].Quantity;
        }

        public Item(Item clone) 
        {
            ID = clone.ID;
            InventorySprite = clone.InventorySprite;
            Name = clone.Name;
            ItemTags = clone.ItemTags;
            Quantity = clone.Quantity;
        }

        public Item(int id, string name, Texture inv_text) 
        {
            ID = id;
            Name = name;
            InventorySprite = new Sprite(inv_text);
        }

        public virtual void Draw(RenderTarget target, RenderStates states) 
        {
            target.Draw(InventorySprite, states);
        }
    }
}
