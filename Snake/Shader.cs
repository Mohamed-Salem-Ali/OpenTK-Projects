using System;
using OpenTK.Graphics.OpenGL4;

namespace Snake
{
    /// <summary>
    /// A class to manage the shaders of the program
    /// </summary>
    public static class Shader
    {
        /// <summary>
        /// Handle for the shader program
        /// </summary>
        public static int _handle;

        /// <summary>
        /// Source of the Vertex shader.
        /// This is defined in c# so we dont
        /// have to load any external files
        /// </summary>
        private static string VertexSource
        {
            get
            {
                return
@"#version 330 core
layout (location = 0) in vec2 pos;

void main(){
    gl_Position = vec4(pos, 0.0f, 1.0f);
}";

            }
        }
        /// <summary>
        /// Source of the Fragment shader.
        /// This is defined in c# so we dont
        /// have to load any external files
        /// </summary>
        private static string FragmentSource
        {
            get
            {
                return
@"#version 330 core
out vec4 color;

void main(){
    color = vec4(1.0f, 1.0f, 1.0f, 1.0f);
}";

            }
        }

        #region Initialization
        /// <summary>
        /// Initializes the shader
        /// </summary>
        public static void Initialize()
        {
            _handle = CreateProgram();
            GL.UseProgram(_handle);
        }

        /// <summary>
        /// Creates a openGl shader program
        /// </summary>
        /// <returns>The handle of the openGL shader program</returns>
        private static int CreateProgram()
        {
            //First we create the two shaders
            CreateShaders(out int vertex, out int fragment);
            
            //Next we compile them
            CompileShader(vertex);
            CompileShader(fragment);
            
            //Now we want to attach the shaders to the program
            int handle = LinkShaders(vertex, fragment);

            //Dispose the two individual shaders after linking
            DetachShaders(handle, vertex, fragment);
            
            return handle;
        }

        /// <summary>
        /// Detach the two shaders from the program and delete them
        /// </summary>
        /// <param name="program">Program to detach from</param>
        /// <param name="vertex">Vertex shader to delete</param>
        /// <param name="fragment">Fragment shader to delete</param>
        private static void DetachShaders(int program, int vertex, int fragment)
        {
            GL.DetachShader(program, vertex);
            GL.DetachShader(program, fragment);
            GL.DeleteShader(vertex);
            GL.DeleteShader(fragment);
        }

        /// <summary>
        /// Links two shaders to a new program and returns the handle
        /// </summary>
        /// <param name="vertex">Vertex shader to link to the new program</param>
        /// <param name="fragment">Fragment shader to link to the new program</param>
        /// <returns>The newly generated handle</returns>
        private static int LinkShaders(int vertex, int fragment)
        {
            int handle = GL.CreateProgram();
            GL.AttachShader(handle, vertex);
            GL.AttachShader(handle, fragment);
            GL.LinkProgram(handle);
            return handle;
        }

        /// <summary>
        /// Compiles a shader and checks it for errors
        /// </summary>
        /// <param name="shader">handle of shader to compile</param>
        /// <exception cref="Exception">Thrown if shader fails to compile</exception>
        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);
            //Check for errors
            string infoLog = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(infoLog))
            {
                Console.WriteLine($"Shader failed to load | {infoLog}");
                throw new Exception($"Shader failed to load | {infoLog}");
            }
        }

        /// <summary>
        /// Creates two shaders and returns the handles
        /// </summary>
        /// <param name="vertex">handle of the vertex shader</param>
        /// <param name="fragment">handle of the fragment shader</param>
        private static void CreateShaders(out int vertex, out int fragment)
        {
            vertex = GL.CreateShader(ShaderType.VertexShader);
            fragment = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(vertex, VertexSource);
            GL.ShaderSource(fragment, FragmentSource);
        }
        #endregion Initialization

        #region Dispose

        /// <summary>
        /// Is this class disposed?
        /// </summary>
        private static bool _isDisposed = false;

        /// <summary>
        /// Dispose the class and get rid of openGl handles that will no longer be used
        /// </summary>
        public static void Dispose()
        {
            if (!_isDisposed)
            {
                GL.DeleteProgram(_handle);
                GL.UseProgram(0);
                
                _isDisposed = true;
            }
        }
        #endregion Dispose
    }
}
