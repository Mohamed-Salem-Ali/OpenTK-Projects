using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Snake
{
    /// <summary>
    /// Holds and manages the openGl shader program handle
    /// </summary>
    public class Shader : IDisposable
    {
        /// <summary>
        /// Program handle
        /// </summary>
        private readonly int _handle;

        private static int _currentHandle;

        /// <summary>
        /// Creates a shader from given paths
        /// </summary>
        /// <param name="vertexPath">Path of the vertex shader</param>
        /// <param name="fragmentPath">Path of the fragment shader</param>
        public Shader(string vertexPath = "basic.vert", string fragmentPath = "basic.frag")
        {
            //Create the shaders
            int vertex = BuildShader(vertexPath, ShaderType.VertexShader);
            int fragment = BuildShader(fragmentPath, ShaderType.FragmentShader);

            //Create the program
            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, vertex);
            GL.AttachShader(_handle, fragment);
            GL.LinkProgram(_handle);
            
            //Detach and delete the shaders again
            GL.DetachShader(_handle, vertex);
            GL.DeleteShader(vertex);
            GL.DetachShader(_handle, fragment);
            GL.DeleteShader(vertex);
        }

        /// <summary>
        /// Builds a shader and returns the handle to the shader
        /// </summary>
        /// <param name="path">Path of the shader</param>
        /// <param name="shaderType">Type of the shader</param>
        /// <returns>Handle of the shader</returns>
        public static int BuildShader(string path, ShaderType shaderType)
        {
            //Create the shader
            int handle = GL.CreateShader(shaderType);
            string source = File.ReadAllText(path);
            GL.ShaderSource(handle, source);
            
            //Compile the shader
            GL.CompileShader(handle);
            string infoLog = GL.GetShaderInfoLog(handle);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                Console.WriteLine($"{shaderType} at {path} compiled with error: {infoLog}");
            }
            
            return handle;
        }

        /// <summary>
        /// Binds the shader
        /// </summary>
        public void Bind()
        {
            GL.UseProgram(_handle);
            _currentHandle = _handle;
        }

        /// <summary>
        /// Sets a matrix uniform to given location from name
        /// </summary>
        /// <param name="name">Name used to look for uniform on current shader</param>
        /// <param name="value">Value to set uniform to</param>
        /// <exception cref="ArgumentException">Thrown when uniform could not be found from name</exception>
        public static void SetUniform(string name, ref Matrix4 value)
        {
            int location = GL.GetUniformLocation(_currentHandle, name);
            if (location == -1)
            {
                throw new ArgumentException("uniform name not found");
            }
            GL.UniformMatrix4(location, true, ref value);
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
                GL.DeleteProgram(_handle);
                if (_currentHandle == _handle)
                {
                    _currentHandle = 0;
                    GL.UseProgram(0);
                }
                
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~Shader()
        {
            Dispose();
        }
        #endregion Disposing
    }
}