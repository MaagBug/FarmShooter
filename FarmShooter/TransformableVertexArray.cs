namespace FarmShooter
{
    class TransformableVertexArray : Transformable, Drawable
    {
        public VertexArray Vertexes = new();

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform = Transform;
            target.Draw(Vertexes, states);
        }
    }
}
