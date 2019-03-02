using System;
using OpenTK.Graphics.OpenGL4;

namespace TestProject
{
    public class BufferObject : IDisposable
    {
        private BufferTarget _bufferType;
        private readonly int _handle = 0;

        private bool DataInitialized { get; set; } = false;

        public BufferObject()
        {
            _handle = GL.GenBuffer();
        }
        
        public unsafe void SetData<TDataType>(
            BufferTarget bufferType,
            TDataType[] data, 
            BufferUsageHint hint = BufferUsageHint.StaticDraw) 
                where TDataType : unmanaged
        {
            if (!DataInitialized && !_isDisposed)
            {
                DataInitialized = true;
                _bufferType = bufferType;
                
                Bind();
                GL.BufferData(bufferType, sizeof(TDataType) * data.Length, data, hint);
            }
        }

        public void Bind()
        {
            if (DataInitialized && !_isDisposed)
            {
                GL.BindBuffer(_bufferType, _handle);
            }
        }

        #region Disposing

        private bool _isDisposed = false;
        
        public void Dispose()
        {
            if (!_isDisposed)
            {
                GL.BindBuffer(_bufferType, _handle);
                GL.DeleteBuffer(_handle);
                
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }

        ~BufferObject()
        {
            Dispose();
        }
        #endregion Disposing
    }
}