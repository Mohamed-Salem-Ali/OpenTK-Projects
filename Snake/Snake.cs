using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Snake
{
    /// <summary>
    /// This class will manage the gameplay and function as the main window class
    /// </summary>
    public class Snake : GameWindow
    {
        private SnakeComponent _snakeComponent;
        
        /// <summary>
        /// Creates a basic window
        /// </summary>
        /// <param name="width">Width of the window</param>
        /// <param name="height">Height of the window</param>
        /// <param name="title">Title of the window</param>
        public Snake(int width, int height, string title) 
            : base(width, height, GraphicsMode.Default, title)
        { }

        /// <summary>
        /// Load and set up resources
        /// </summary>
        /// <param name="e">event arg passed to us by opentk</param>
        protected override void OnLoad(EventArgs e)
        {
            _snakeComponent = new SnakeComponent(Vector2.One);
            Shader.Initialize();
            
            GL.ClearColor(0.0f, 0.0f, 1.0f, 1.0f);
            base.OnLoad(e);
        }


        /// <summary>
        /// Renders everything
        /// </summary>
        /// <param name="e">event arg passed to us by opentk</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            //Draw everything here
            _snakeComponent.Draw();
            
            Context.SwapBuffers();
            
            base.OnRenderFrame(e);
        }

        /// <summary>
        /// Resize the GL viewport
        /// </summary>
        /// <param name="e">event arg passed to us by opentk</param>
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            
            base.OnResize(e);
        }
        
        /// <summary>
        /// Dispose all the snake components
        /// </summary>
        /// <param name="e">event arg passed to us by opentk</param>
        protected override void OnUnload(EventArgs e)
        {
            Shader.Dispose();
            _snakeComponent.Dispose();
            base.OnUnload(e);
        }
    }
}
