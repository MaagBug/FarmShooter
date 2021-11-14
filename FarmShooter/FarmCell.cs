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
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(MainSprite, states);
            if(PlantedPlant != null) target.Draw(PlantedPlant.MainSprite, states);
        }
    }
}
