using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Snake
{
    public abstract class DrawAble : IDisposable
    {
        /// <summary>
        /// Matrix of the transformation of this snake component
        /// </summary>
        protected Matrix4 _transformMatrix = Matrix4.Identity;

        /// <summary>
        /// OpenGL Vertex Array Object handle
        /// </summary>
        private readonly int _vao;
        /// <summary>
        /// OpenGL Vertex Buffer Object handle
        /// </summary>
        private readonly int _vbo;
        /// <summary>
        /// OpenGL Element Buffer Object Handle
        /// </summary>
        private readonly int _ibo;
        /// <summary>
        /// Size of the index buffer
        /// </summary>
        private readonly int _indexCount;

        protected DrawAble()
        {
            //Initialize VAO and bind it before we initialize the buffers
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);
            
            //Initialize VBO
            float[] vertices = VertexBuffer; 
            
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            
            //Initialize IBO
            uint[] indices = IndexBuffer;
            _indexCount = indices.Length;
            
            _ibo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ibo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * indices.Length, indices, BufferUsageHint.StaticDraw);
            
            VertexAttributeLayout();
        }
        
        /// <summary>
        /// Draw the drawable
        /// </summary>
        public void Draw()
        {
            Shader.SetUniform("transform", ref _transformMatrix);
            
            GL.BindVertexArray(_vao);
            GL.DrawElements(PrimitiveType.Triangles, _indexCount, DrawElementsType.UnsignedInt, 0);
        }
        
        /// <summary>
        /// Should define the vertex attrib layout of the vbo
        /// </summary>
        protected abstract void VertexAttributeLayout();
        
        /// <summary>
        /// The vertex buffer this is used to generate the VBO for the VAO
        /// Used once in the constructor
        /// </summary>
        protected abstract float[] VertexBuffer { get; }
        /// <summary>
        /// The index buffer this is used to generate the IBO for the VAO
        /// Used once in constructor
        /// </summary>
        protected abstract uint[] IndexBuffer { get; }
        
        #region Dispose
        /// <summary>
        /// Has this been disposed?
        /// </summary>
        private bool _isDisposed;
        
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
        ~DrawAble()
        {
            Dispose();
        }
        #endregion Dispose
    }
}