using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace TestProject
{
    public class VertexArrayObject : IDisposable
    {
        private readonly List<BufferObject> _buffers = new List<BufferObject>();
        private readonly int _handle;
        
        public VertexArrayObject()
        {
            _handle = GL.GenVertexArray();
        }

        public void AddBuffer(ref BufferObject bufferObject, BufferLayout bufferLayout = null)
        {
            Bind();
            bufferObject.Bind();
            _buffers.Add(bufferObject);
            if (bufferLayout != null)
            {
                int offset = 0;
                for (int i = 0; i < bufferLayout.Elements.Count; i++)
                {
                    LayoutElement la = bufferLayout.Elements[i];
                    GL.VertexAttribPointer(i, la.Count, la.Type, la.Normalized, bufferLayout.Stride, offset);
                    GL.EnableVertexAttribArray(i);
                    offset += LayoutElement.GetSizeOfType(la.Type) * la.Count;
                }
            }
        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        #region Disposing

        private bool _isDisposed = false;
        
        public void Dispose()
        {
            if (!_isDisposed)
            {
                GL.BindVertexArray(0);
                GL.DeleteVertexArray(_handle);
                
                for (int i = 0; i < _buffers.Count; i++)
                {
                    BufferObject bo = _buffers[i];
                    bo.Dispose();
                }
                
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }

        ~VertexArrayObject()
        {
            Dispose();
        }

        #endregion
    }
}