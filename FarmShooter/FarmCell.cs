namespace FarmShooter
{
    class FarmCell : Cell
    {
        public Plant PlantedPlant;

        public FarmCell() : base(4)
        {
            
        }

        public void Plant(int plant_id) 
        {
            PlantedPlant = new Plant(plant_id);
            PlantedPlant.MainSprite.Position = MainSprite.Position + new Vector2f(50, 70);
            Program.InteractableResources.Add(PlantedPlant);
        }
    }
}
