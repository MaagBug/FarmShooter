namespace FarmShooter
{
    internal class Plant : InteractableResource
    {
        public static List<Plant> AllPlants = new List<Plant>();

        public static void LoadPlants(string json) 
        {
            AllPlants.Clear();

            DataTable table = JsonConvert.DeserializeObject<DataTable>(json);

            foreach(DataRow row in table.Rows) 
            {
                List<Texture> stages = new List<Texture>();
                for (int t = 0; t < (int)(long)row.ItemArray[2]; ++t) 
                {
                    stages.Add(Program.Textures[(string)row.ItemArray[1] + "_Stage_" + t.ToString()]);
                }

                AllPlants.Add
                    (
                    new Plant((int)(long)row.ItemArray[0], stages, (int)(long)row.ItemArray[2], 100, (ToolType)(long)row.ItemArray[5], (string)row.ItemArray[4])
                    );
            }
        }

        public int Stage 
        {
            get { return _Stage; }
            set 
            {
                _Stage = value;
                MainSprite.Texture = StagesTextures[value];
            }
        }

        private int _Stage = 0;

        public int MaxStage;
        public Item HarvestResource;
        public Item HarvestSeed;

        public Item PlantSeed;

        public List<Texture> StagesTextures;

        public Plant(int ID, List<Texture> stages, int max_stages, int max_durability, ToolType type, string name) 
        {
            this.ID = ID;

            MainSprite = new Sprite(stages[0]);
            MainSprite.Origin = new Vector2f(MainSprite.Texture.Size.X / 2, MainSprite.Texture.Size.Y);
            Name = name;

            MaxDurability = max_durability;
            Durability = max_durability;

            InteractToolType = type;

            StagesTextures = stages;
            MaxStage = max_stages;
        }

        public Plant(int ID) 
        {
            this.ID = ID;
            MainSprite = new Sprite(AllPlants[ID].MainSprite);
            HarvestResource = AllPlants[ID].HarvestResource;

            Name = AllPlants[ID].Name;

            MaxDurability = AllPlants[ID].MaxDurability;
            Durability = AllPlants[ID].Durability;

            InteractToolType = AllPlants[ID].InteractToolType;

            StagesTextures = AllPlants[ID].StagesTextures;
            MaxStage = AllPlants[ID].MaxStage;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(MainSprite, states);
        }

        public override void Interact(Tool tool)
        {
            throw new NotImplementedException();
        }
    }
}
