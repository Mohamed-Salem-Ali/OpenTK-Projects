using System;
using System.Runtime.CompilerServices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Snake
{
    /// <summary>
    /// One component of the snake
    /// This class will manage drawing as well as movement of any particular parts of the snake
    /// </summary>
    public class SnakeComponent : IDisposable
    {
        /// <summary>
        /// Struct of a single vertex used for this snake
        /// </summary>
        private struct Vertex
        {
            /// <summary>
            /// Position of the vertex
            /// </summary>
            public Vector2 Position { get; set; }
        }

        /// <summary>
        /// The current position of the snake
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// OpenGL Vertex Array Object handle
        /// </summary>
        private int _vao;
        /// <summary>
        /// OpenGL Vertex Buffer Object handle
        /// </summary>
        private int _vbo;
        /// <summary>
        /// OpenGL Element Buffer Object Handle
        /// </summary>
        private int _ibo;

        #region Initialization
        /// <summary>
        /// Initialize Snake so it is ready to be used
        /// </summary>
        /// <param name="position">Position of this Snake Component</param>
        public SnakeComponent(Vector2 position)
        {
            //Set variables
            Position = position;

            InitializeOpenGl();
        }

        public unsafe void InitializeOpenGl()
        {
            //Initialize VAO and bind it before we initialize the buffers
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);
            
            //Initialize VBO
            Vertex[] vertices = GetVb(); 
            
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(Vertex) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            //Initialize IBO
            uint[] indices = GetIb();
            
            _ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);
            
            //Define the layout of the VBO
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(Vertex), 0);
            GL.EnableVertexAttribArray(0);
        }

        /// <summary>
        /// Generate the VB data for the VBO
        /// </summary>
        /// <returns>The newly generated VB</returns>
        private Vertex[] GetVb()
        {
            return new Vertex[]
            {
                new Vertex() {Position = Vector2.Zero},
                new Vertex() {Position = Vector2.UnitX},
                new Vertex() {Position = Vector2.One},
                new Vertex() {Position = Vector2.UnitY}
            };
        }

        /// <summary>
        /// Generate the IB data for the IBO
        /// </summary>
        /// <returns>The newly generated IB</returns>
        private uint[] GetIb()
        {
            return new uint[]
            {
                1, 2, 3,
                1, 3, 4
            };
        }
        #endregion Initialization

        /// <summary>
        /// Draw the snake
        /// </summary>
        public void Draw()
        {
            //TODO: Apply the position of the snake here
            
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
        
        #region Dispose
        /// <summary>
        /// Has this been disposed?
        /// </summary>
        private bool _isDisposed = false;
        
        /// <summary>
        /// Dispose the OpenGL handles
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                GL.DeleteBuffer(_vbo);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.DeleteBuffer(_ibo);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                
                GL.DeleteVertexArray(_vao);
                GL.BindVertexArray(0);

                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Last measure to try an expose the openGL handles before it is to late
        /// </summary>
        ~SnakeComponent()
        {
            Dispose();
        }
        #endregion Dispose
    }
}
