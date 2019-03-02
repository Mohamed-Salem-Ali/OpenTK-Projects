using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace TestProject
{
    public class Window : GameWindow
    {
        private BufferObject _vbo;
        private BufferLayout _layout;
        private BufferObject _ebo;
        private Shader _shader;
        private Texture _texture;
        
        private Vector3 _cameraMovement;
        private Camera _camera;
        private Transform _transform;
        
        private VertexArrayObject _vao;

        public Window(int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            
            GL.DebugMessageCallback(Debug, IntPtr.Zero);
        }

        private static void Debug(DebugSource source, DebugType type, int id, DebugSeverity severity, int length,
            IntPtr message, IntPtr param)
        {
            Console.WriteLine($"{severity} | {source}");
        }
        
        protected override void OnLoad(EventArgs e)
        {
            float[] vertices = {
                 0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
                 0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
                -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
                -0.5f,  0.5f, 0.0f, 0.0f, 1.0f 
            };
            uint[] indices =
            {
                0, 1, 3,
                1, 2, 3
            };
            _vbo = new BufferObject();
            _ebo = new BufferObject();
            _layout = new BufferLayout();
            _vao = new VertexArrayObject();
            
            _vbo.SetData(BufferTarget.ArrayBuffer, vertices);
            _ebo.SetData(BufferTarget.ElementArrayBuffer, indices);
            
            _layout.AddLayoutItem(VertexAttribPointerType.Float, 3);
            _layout.AddLayoutItem(VertexAttribPointerType.Float, 2);

            _vao.AddBuffer(ref _ebo);
            _vao.AddBuffer(ref _vbo, _layout);
            
            
            _shader = new Shader("shader.vert", "shader.frag");
            _texture = new Texture("test");

            _transform = new Transform();
            _camera = new Camera(Width, Height);
            _transform.SetShader(ref _shader);
            _camera.ApplyToShader(ref _shader);
            
            GL.ClearColor(1f, 1f, 1f, 1f);

            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Camera.Main.Move(_cameraMovement * (float)e.Time * 5f);
            _cameraMovement = Vector3.Zero;
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            _transform.UpdateShader();
            _camera.UpdateShader();
            
            _shader.Bind();
            _texture.Bind();
            _vao.Bind();
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();
            base.OnUpdateFrame(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w':
                    _cameraMovement = Vector3.UnitZ;
                    break;
                case 'a':
                    _cameraMovement = -Vector3.UnitX;
                    break;
                case 's':
                    _cameraMovement = -Vector3.UnitZ;
                    break;
                case 'd':
                    _cameraMovement = Vector3.UnitX;
                    break;
                default:
                    _cameraMovement = Vector3.Zero;
                    break;
            }
            base.OnKeyPress(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            _shader.Dispose();
            _texture.Dispose();
            _vao.Dispose();

            base.OnUnload(e);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
