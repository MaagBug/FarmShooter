namespace FarmShooter
{
    abstract class Handheld : Item
    {
        public static List<Handheld> AllHandhelds = new List<Handheld>();
        public static void LoadHandhelds(string json) 
        {
            DataTable table = JsonConvert.DeserializeObject<DataTable>(json);

            for (int i = 0; i < table.Rows.Count; ++i) 
            {
                if ((string)table.Rows[i].ItemArray[1] == "Tool")
                {
                    var row = table.Rows[i];
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
            MainSprite.Origin = new Vector2f(30, 53);
        }

        public abstract void Update();
    }
}
