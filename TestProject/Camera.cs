using OpenTK;

namespace TestProject
{
    public class Camera : Transform
    {
        public static Camera Main;

        private Matrix4 _projection;
        private Shader _shader;
        
        public float FieldOfView
        {
            get => _projection.ExtractProjection().X;
        }

        public Camera(float width, float height)
        {
            if (Main == null)
            {
                Main = this;
            }
            
            Move(0, 0, -3f);
            _projection = Matrix4.CreatePerspectiveFieldOfView((float)MathHelper.DegreesToRadians(90),
                width / height, 0.01f, 100.0f);
        }

        public override void UpdateShader()
        {
            _shader?.SetUniform("u_projection", ref _projection);
            _shader?.SetUniform("u_view", ref TransformMatrix);
        }

        public void ApplyToShader(ref Shader shader)
        {
            _shader = shader;
            UpdateShader();
        }
    }
}