namespace FarmShooter
{
    abstract class Handheld : Item
    {
        public static List<Handheld> AllHandhelds = new List<Handheld>();
        public static void LoadHandhelds(string json) 
        {
            AllHandhelds.Clear();

            DataTable table = JsonConvert.DeserializeObject<DataTable>(json);

            foreach(DataRow row in table.Rows)
            {
                if ((string)row.ItemArray[1] == "Tool")
                {
                    AllHandhelds.Add(new Tool((int)(long)row.ItemArray[0], (string)row.ItemArray[4], Program.Textures[(string)row.ItemArray[2]], Program.Textures[(string)row.ItemArray[3]], (ToolType)(long)row.ItemArray[6]) { Efficiency = (int)(long)row.ItemArray[7] });
                }
            }
        }

        public Sprite MainSprite;

        public Player Owner;

        public Handheld(int id, string name, Texture text, Texture inv_text) : base(name, inv_text)
        {
            ID = id;
            MainSprite = new Sprite(text);
            //MainSprite.Origin = new Vector2f(30, 53);
            MainSprite.Origin = (Vector2f)(MainSprite.Texture.Size / 2);
        }

        public abstract void Update();
    }
}
