using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Snake
{
    /// <summary>
    /// Manages a texture and ability to bind it
    /// </summary>
    public class Texture : IDisposable
    {
        /// <summary>
        /// Handle of the texture in opengl
        /// </summary>
        private readonly int _handle;

        /// <summary>
        /// Create a new texture from a image path
        /// </summary>
        /// <param name="path">Path of the image to load</param>
        public Texture(string path)
        {
            _handle = GL.GenTexture();
            Bind();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            Bitmap image = new Bitmap(path);
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0); 
            image.UnlockBits(data);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        /// <summary>
        /// Binds the texture
        /// </summary>
        /// <param name="textureUnit">Texture unit to bind to, leave at zero for default texture</param>
        public void Bind(TextureUnit textureUnit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }
        
        #region Disposing
        /// <summary>
        /// Is this object disposed?
        /// </summary>
        private bool _isDisposed;
        public void Dispose()
        {
            if (!_isDisposed)
            {
                GL.DeleteTexture(_handle);
                GL.BindTexture(TextureTarget.Texture2D, 0);
                
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~Texture()
        {
            Dispose();
        }
        #endregion Disposing
    }
}