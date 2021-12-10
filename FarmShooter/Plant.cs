namespace FarmShooter
{
    internal class Plant : InteractableResource
    {
        public static List<Plant> AllPlants = new List<Plant>();

        //public Timer GrowTimer;

        public static void LoadPlants(string json) 
        {
            AllPlants.Clear();

            DataTable table = JsonConvert.DeserializeObject<DataTable>(json);

            foreach(DataRow row in table.Rows) 
            {
                List<Texture> stages = new List<Texture>();
                for (int t = 0; t < (int)(long)row.ItemArray[2]; ++t) 
                {
                    stages.Add(Program.Textures.GetValueOrSpecificDefault((string)row.ItemArray[1] + "_Stage_" + t.ToString(), Program.Textures["MISSING_TEXTURE"]));
                }

                AllPlants.Add
                    (
                    new Plant((int)(long)row.ItemArray[0], stages, (int)(long)row.ItemArray[2], 100, (ToolType)(long)row.ItemArray[5], (string)row.ItemArray[4])
                    { HarvestResource = new Item((int)(long)row.ItemArray[6]), PlantSeed = new Item((int)(long)row.ItemArray[7]) }
                    );
            }
        }

        public int Stage 
        {
            get { return _Stage; }
            set 
            {
                if (value < MaxStage)
                {
                    _Stage = value;
                    MainSprite.Texture = StagesTextures[value];
                }
            }
        }

        private int _Stage;

        public int MaxStage;
        public Item HarvestResource;
        public Item PlantSeed;

        public FarmCell PlantedCell;

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

        public Plant(Plant copy) 
        {
            ID = copy.ID;
            MainSprite = new Sprite(copy.MainSprite);
            HarvestResource = copy.HarvestResource;

            Name = copy.Name;

            MaxDurability = copy.MaxDurability;
            Durability = copy.Durability;

            InteractToolType = copy.InteractToolType;

            StagesTextures = copy.StagesTextures;
            MaxStage = copy.MaxStage;

            PlantedCell = copy.PlantedCell;

            _Stage = MaxStage - 1;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(MainSprite, states);
        }

        public override void Interact(Tool tool)
        {
            if (tool != null) 
            {
                if (InteractToolType == tool.Type) Durability -= tool.Efficiency;
                else if (InteractToolType == ToolType.Any) Durability -= 10;

                if (Durability <= 0) 
                {
                    Program.InteractableResources.Remove(this);
                    PlantedCell.PlantedPlant = null;

                    if (Stage == MaxStage - 1) 
                    {
                        tool.Owner.Inventory.AddItem(new Item(HarvestResource), out _);
                    }
                }
            }
        }
    }
}
