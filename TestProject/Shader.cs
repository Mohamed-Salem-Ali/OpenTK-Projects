using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;

namespace TestProject
{
    public class Shader : IDisposable
    {
        private readonly int _handle;

        private struct ShaderStrings
        {
            public string Vertex { get; set; }
            public string Fragment { get; set; }
        }
        private struct ShaderInts
        {
            public int Vertex { get; set; }
            public int Fragment { get; set; }
        }
        
        #region Initialization
        private Shader(ShaderStrings shaderPaths)
        {
            ShaderStrings shaderSources = ReadShaders(shaderPaths);

            ShaderInts shaders = CompileShaders(shaderSources);
            
            _handle = CreateProgram(shaders);
        }
        public Shader(string vertexPath, string fragmentPath)
         : this(new ShaderStrings(){Vertex = vertexPath, Fragment = fragmentPath}) { }

        private static int CreateProgram(ShaderInts shaders)
        {
            var program = GL.CreateProgram();
            GL.AttachShader(program, shaders.Vertex);
            GL.AttachShader(program, shaders.Fragment);
            
            GL.LinkProgram(program);
            
            //Clean up the shaders
            GL.DetachShader(program, shaders.Vertex);
            GL.DetachShader(program, shaders.Fragment);
            GL.DeleteShader(shaders.Vertex);
            GL.DeleteShader(shaders.Fragment);
            return program;
        }
        
        private static ShaderInts CompileShaders(ShaderStrings shaderSources)
        {
            ShaderInts shaders = new ShaderInts();
            shaders.Vertex = CompileShader(shaderSources.Vertex, ShaderType.VertexShader);
            shaders.Fragment = CompileShader(shaderSources.Fragment, ShaderType.FragmentShader);
            return shaders;
        }
        private static int CompileShader(string shaderSource, ShaderType shaderType)
        {
            int shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, shaderSource);
            
            GL.CompileShader(shader);
            
            string infoLog = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                Console.WriteLine(infoLog);
            }

            return shader;
        }

        private static ShaderStrings ReadShaders(ShaderStrings shaderPaths)
        {
            ShaderStrings shaderSources = new ShaderStrings();
            shaderSources.Vertex = ReadShader(shaderPaths.Vertex);
            shaderSources.Fragment = ReadShader(shaderPaths.Fragment);
            return shaderSources;
        }
        private static string ReadShader(string shaderPath)
        {
            return File.ReadAllText(shaderPath);
        }
        #endregion Initialization
        
        public void Bind()
        {
            GL.UseProgram(_handle);
        }

        public void SetUniform(string name, ref Matrix4 value)
        {
            Bind();
            int location = GL.GetUniformLocation(_handle, name);
            GL.UniformMatrix4(location, true, ref value);
        }
        public void SetUniform(string name, int value)
        {
            Bind();
            int location = GL.GetUniformLocation(_handle, name);
            GL.Uniform1(location, value);
        }
        public void SetUniform(string name, float value)
        {
            Bind();
            int location = GL.GetUniformLocation(_handle, name);
            GL.Uniform1(location, value);
        }
        public void SetUniform(string name, Vector2 value)
        {
            Bind();
            int location = GL.GetUniformLocation(_handle, name);
            GL.Uniform2(location, value);
        }
        public void SetUniform(string name, Vector3 value)
        {
            Bind();
            int location = GL.GetUniformLocation(_handle, name);
            GL.Uniform3(location, value);
        }
        public void SetUniform(string name, Vector4 value)
        {
            Bind();
            int location = GL.GetUniformLocation(_handle, name);
            GL.Uniform4(location, value);
        }

        #region Disposing

        private bool _isDisposed = false;
        
        public void Dispose()
        {
            if (!_isDisposed)
            {
                GL.UseProgram(0);
                GL.DeleteProgram(_handle);
                
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }    
        }

        ~Shader()
        {
            Dispose();
        }
        #endregion Disposing
    }
}
