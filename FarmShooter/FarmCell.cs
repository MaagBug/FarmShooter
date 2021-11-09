namespace FarmShooter
{
    class FarmCell : Cell
    {
        public FarmCell() : base(6)
        {
            
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(MainSprite, states);
        }
    }
}
