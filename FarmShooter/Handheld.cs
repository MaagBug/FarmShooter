namespace FarmShooter
{
    abstract class Handheld : Item
    {
        public static Handheld DeserializeHandheld(DataRow row)
        {
            if (((string[])row.ItemArray[3]).ToList().Contains("Tool"))
            {
                return new Tool((int)(long)row.ItemArray[0], (string)row.ItemArray[2], Program.Textures.GetValueOrSpecificDefault((string)row.ItemArray[5], Program.Textures["MISSING_TEXTURE"]), Program.Textures.GetValueOrSpecificDefault((string)row.ItemArray[1], Program.Textures["MISSING_TEXTURE"]), (ToolType)(long)row.ItemArray[7])
                { Efficiency = (int)(long)row.ItemArray[8], ItemTags = ((string[])row.ItemArray[3]).ToList() };
            }
            else return null;
        }

        public Sprite MainSprite;

        public Player Owner;

        public Handheld(Handheld copy) : base(copy)
        {
            MainSprite = new Sprite(copy.MainSprite);
            Owner = copy.Owner;
        }

        public Handheld(int id) : base(id)
        {
            MainSprite = new Sprite(((Handheld)AllItems[id]).MainSprite);
        }

        public Handheld(int id, string name, Texture text, Texture inv_text) : base(id, name, inv_text)
        {
            MainSprite = new Sprite(text);
            MainSprite.Origin = (Vector2f)(MainSprite.Texture.Size / 2);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(MainSprite, states);
        }

        public virtual void Update() 
        {
            MainSprite.Position = Owner.Handle.GetImagePoint(Owner.MainEntity.Sprite.Position);
            MainSprite.Rotation = MathF.Atan2(Program.TrueMousePosition.Y - Owner.MainEntity.Sprite.Position.Y, Program.TrueMousePosition.X - Owner.MainEntity.Sprite.Position.X) * (180 / 3.14F);
            if (MathF.Abs(MainSprite.Rotation) > 90) MainSprite.Scale = new Vector2f(1, -1);
            else MainSprite.Scale = new Vector2f(1, 1);
        }
    }
}
