using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Snake
{
    public class SnakeHead : SnakeComponent
    {
        public SnakeHead(Vector2 positionInt)
            : base(positionInt)
        { }

        public override void Draw()
        {
            base.DrawHead();
        }
        
        /// <summary>
        /// Define the layout of a vertex, this is called in the constructor from drawable
        /// </summary>
        protected override void VertexAttributeLayout()
        {
            //Define the layout of the VBO
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 4, sizeof(float) * 2);
            GL.EnableVertexAttribArray(1);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Generate the VB data for the VBO
        /// </summary>
        protected override float[] VertexBuffer
        {
            get
            {
                const float minX = -1f, minY = -1f, maxX = 1f, maxY = 1f;
                return new[]
                {
                    minX, minY, 0f, 1f,
                    maxX, minY, 1f, 1f,
                    maxX, maxY, 1f, 0f,
                    minX, maxY, 0f, 0f
                };
            }
        }
    }
}