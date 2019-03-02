using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using System.Drawing;

namespace TestProject
{
    public class Texture : IDisposable
    {
        private readonly int _handle;

        public Texture(string texturePath)
        {
            _handle = GL.GenTexture();

            Bitmap image = new Bitmap(texturePath);
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            
            List<byte> pixels = new List<byte>();
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Color color = image.GetPixel(x, y);
                    pixels.Add(color.R);
                    pixels.Add(color.G);
                    pixels.Add(color.B);
                    pixels.Add(color.A);
                }
            }

            Bind();
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Bind(TextureUnit textureUnit = TextureUnit.Texture0)
        {
            if (!_isDisposed)
            {
                GL.ActiveTexture(textureUnit);
                GL.BindTexture(TextureTarget.Texture2D, _handle);
            }
        }

        private bool _isDisposed = false;
        public void Dispose()
        {
            if (!_isDisposed)
            {
                GL.BindTexture(TextureTarget.Texture2D, _handle);
                GL.DeleteTexture(_handle);
                
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~Texture()
        {
            Dispose();
        }
    }
}