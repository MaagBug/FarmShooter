namespace FarmShooter
{
    public class Entity : Drawable
    {
        public Sprite Sprite;
        public float Speed = 0;

        public Entity(Texture text) 
        {
            Sprite = new Sprite() { Texture = text, Origin = (Vector2f)(text.Size / 2) };
        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Sprite, states);
        }
    }
}
