using OpenTK;

namespace TestProject
{
    public class Transform
    {
        protected Matrix4 TransformMatrix = Matrix4.Identity;
        private Shader _shader;
        public Vector3 Position
        {
            get => TransformMatrix.ExtractTranslation();
            set
            {
                TransformMatrix.ClearTranslation();
                TransformMatrix *= Matrix4.CreateTranslation(value);
                UpdateShader();
            }
        }
        public Vector3 Scale
        {
            get => TransformMatrix.ExtractScale();
            set
            {
                TransformMatrix.ClearScale();
                TransformMatrix *= Matrix4.CreateScale(value);
                UpdateShader();
            }
        }
        public Quaternion Rotation
        {
            get => TransformMatrix.ExtractRotation();
            set
            {
                TransformMatrix.ClearRotation();
                TransformMatrix *= Matrix4.CreateFromQuaternion(value);
                UpdateShader();
            }
        }
        public Vector4 AxisAngle
        {
            get => TransformMatrix.ExtractRotation().ToAxisAngle();
            set
            {
                TransformMatrix.ClearRotation();
                TransformMatrix *= Matrix4.CreateFromAxisAngle(new Vector3(value), value.W);
                UpdateShader();
            }
        }

        public void SetShader(ref Shader shader)
        {
            _shader = shader;
            UpdateShader();
        }
        
        public virtual void UpdateShader()
        {
            _shader?.SetUniform("u_transform", ref TransformMatrix);
        }

        public override string ToString()
        {
            return $"{base.ToString()} Position: {Position}, Scale: {Scale}, Rotation: {Rotation}";
        }

        public void Move(Vector3 movement)
        {
            TransformMatrix *= Matrix4.CreateTranslation(movement);
        }
        public void Move(float x, float y, float z)
        {
            TransformMatrix *= Matrix4.CreateTranslation(x, y, z);
        }

        public void ReScale(Vector3 scale)
        {
            TransformMatrix *= Matrix4.CreateScale(scale);
        }

        public void Rotate(Quaternion rotation)
        {
            TransformMatrix *= Matrix4.CreateFromQuaternion(rotation);
        }
        public void Rotate(Vector4 rotation)
        {
            TransformMatrix *= Matrix4.CreateFromAxisAngle(new Vector3(rotation), rotation.W);
        }
        public void Rotate(Vector3 axis, float angle)
        {
            TransformMatrix *= Matrix4.CreateFromAxisAngle(axis, angle);
        }
    }
}