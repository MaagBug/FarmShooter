using System;
using System.IO;
using System.Collections.Generic;
using SFML.Graphics;
using Newtonsoft.Json;
using SFML.System;
using System.Data;

namespace FarmShooter
{
    abstract class Handheld : Item
    {
        public static List<Handheld> AllHandhelds = new List<Handheld>();
        public static void LoadHandhelds(string json) 
        {
            //DataTable dt = new DataTable();
            //dt.Columns.Add("ID", Type.GetType("System.Int32"));
            //dt.Columns.Add("Name", Type.GetType("System.String"));
            //dt.Columns.Add("TextureName", Type.GetType("System.String"));
            //dt.Columns.Add("Efficiency", Type.GetType("System.Int32"));

            //DataRow dtr = dt.NewRow();
            //dtr["ID"] = 0;
            //dtr["Name"] = "Iron Axe";
            //dtr["TextureName"] = "Base_Axe_Iron";
            //dtr["Efficiency"] = 30;

            //dt.Rows.Add(dtr);

            //using FileStream stream = new FileStream("text.txt", FileMode.OpenOrCreate);
            //using StreamWriter writer = new StreamWriter(stream);
            //writer.WriteLine(JsonConvert.SerializeObject(dt));

            //DataTable dt = JsonConvert.DeserializeObject<DataTable>(json);

            //for (int i = 0; i < dt.Rows.Count; ++i) 
            //{
            //    AllHandhelds.Add();
            //}
        }

        public Sprite MainSprite;

        public int ID;

        public Player Owner;

        public Handheld(int id, string name, string text_name, string inv_text_name) : base(name, inv_text_name)
        {
            ID = id;
            MainSprite = new Sprite(Program.Textures[text_name]);
        }

        public abstract void Update();
    }
}
