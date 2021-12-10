namespace FarmShooter
{
    class FarmCell : Cell
    {
        public Plant PlantedPlant;

        public FarmCell() : base(4)
        {
            
        }

        public void Plant(Plant plant) 
        {
            PlantedPlant = new Plant(plant);
            PlantedPlant.MainSprite.Position = MainSprite.Position + new Vector2f(50, 70);
            Program.InteractableResources.Add(PlantedPlant);
        }
    }
}
