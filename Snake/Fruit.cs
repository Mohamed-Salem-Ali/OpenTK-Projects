using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Snake
{
    public class Fruit : DrawAble
    {
        
        /// <summary>
        /// The current position of the fruit
        /// </summary>
        public Vector2 Position
        {
            get
            {
                Vector2 pos = _transformMatrix.ExtractTranslation().Xy;
                return new Vector2((float)Math.Round((pos.X + 1f) * Config.GridSize.X + .5f), 
                    (float)Math.Round((pos.Y + 1f) * Config.GridSize.Y + .5f));
            }
            set
            {
                _transformMatrix = _transformMatrix.ClearTranslation();
                _transformMatrix *= Matrix4.CreateTranslation((value.X-.5f) / Config.GridSize.X - 1f, (value.Y-.5f) / Config.GridSize.Y - 1f, 0);
            }
        }

        /// <summary>
        /// Generate a fruit at given position
        /// </summary>
        /// <param name="position">Position to generate fruit at</param>
        public Fruit(Vector2 position)
        {
            _transformMatrix *= Matrix4.CreateScale(.5f / Config.GridSize.X, .5f/ Config.GridSize.Y, 1);
            Position = position;
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

        /// <summary>
        /// Set the variables for the shader, then draw the fruit
        /// </summary>
        public override void Draw()
        {
            Shader.SetUniform("transform", ref _transformMatrix);

            base.Draw();
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

        /// <inheritdoc />
        /// <summary>
        /// Generate the IB data for the IBO
        /// </summary>
        /// <returns>The newly generated IB</returns>
        protected override uint[] IndexBuffer =>
            new uint[]
            {
                0, 1, 2,
                0, 2, 3
            };
    }
}