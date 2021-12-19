namespace FarmShooter
{
    public class Cell : Drawable
    {
        public static event EventHandler<Vector2i> CellUpdated;

        public Sprite MainSprite;
        public Vector2i MapCoords;

        public int ID 
        {
            get { return _ID; }
            set 
            {
                _ID = value;
                MainSprite.Texture = Program.Textures["FieldTile_" + ID.ToString()];

                CellUpdated?.Invoke(this, MapCoords);
            }
        }
        private int _ID;

        public Cell(int ID) 
        {
            MainSprite = new Sprite(Program.Textures["FieldTile_" + ID.ToString()]);
            _ID = ID;
        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(MainSprite, states);
        }
    }
}
