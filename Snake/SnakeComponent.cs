using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Snake
{
    /// <summary>
    /// One component of the snake
    /// This class will manage drawing as well as movement of any particular parts of the snake
    /// </summary>
    public class SnakeComponent : DrawAble
    {
        /// <summary>
        /// The current position of the snakecomponent
        /// The position will be rounded to the nearest integer so whole numbers are guaranteed
        /// </summary>
        public Vector2 PositionInt
        {
            get
            {
                Vector2 pos = _transformMatrix.ExtractTranslation().Xy;
                return new Vector2((float)Math.Round((pos.X + 1f) * Config.GridSize.X + .5f), 
                    (float)Math.Round((pos.Y + 1f) * Config.GridSize.Y + .5f));
            }
            private set
            {
                _transformMatrix = _transformMatrix.ClearTranslation();
                _transformMatrix *= Matrix4.CreateTranslation((value.X-.5f) / Config.GridSize.X - 1f, (value.Y-.5f) / Config.GridSize.Y - 1f, 0);
            }
        }
        /// <summary>
        /// The current position of the snakecomponent
        /// The position will be a floating point number so precision is guaranteed
        /// </summary>
        public Vector2 PositionFloat
        {
            get
            {
                Vector2 pos = _transformMatrix.ExtractTranslation().Xy;
                return new Vector2((pos.X + 1f) * Config.GridSize.X + .5f, 
                    (pos.Y + 1f) * Config.GridSize.Y + .5f);
            }
            private set
            {
                _transformMatrix = _transformMatrix.ClearTranslation();
                _transformMatrix *= Matrix4.CreateTranslation((value.X-.5f) / Config.GridSize.X - 1f, (value.Y-.5f) / Config.GridSize.Y - 1f, 0);
            }
        }
        /// <summary>
        /// Rotation of the snake part
        /// </summary>
        private float Rotation
        {
            get => _transformMatrix.ExtractRotation().ToAxisAngle().W;
            set
            {
                Vector2 pos = PositionFloat;
                PositionFloat = Vector2.Zero;
                _transformMatrix = _transformMatrix.ClearRotation();
                _transformMatrix *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(value));
                PositionFloat = pos;
            }
        }

        /// <summary>
        /// Field of the direction
        /// </summary>
        private Vector2 _direction;
        /// <summary>
        /// Direction of this part of the snake
        /// Used to make smooth movement
        /// </summary>
        public Vector2 Direction
        {
            get => _direction;
            set
            {
                if (value.X == 1)
                {
                    //E: Rot 90
                    Rotation = 270;
                }
                else if (value.X == -1)
                {
                    //W: Rot 270 || -90
                    Rotation = 90;
                }
                else if (value.Y == 1)
                {
                    //N: Rot 0
                    Rotation = 0;
                }
                else if (value.Y == -1)
                {
                    //S: Rot 180
                    Rotation = 180;
                }
                else
                {
                    //Not a valid direction, so we just return
                    return;
                }
                _direction = value;
                PositionInt = PositionInt;
            }
        }

        /// <summary>
        /// Basic constructor to initialize the snake component with a given position
        /// </summary>
        /// <param name="positionInt">Position to initialize snake component at</param>
        public SnakeComponent(Vector2 positionInt)
        {
            _transformMatrix *= Matrix4.CreateScale(.5f / Config.GridSize.X, .5f/ Config.GridSize.Y, 1);
            PositionInt = positionInt;
        }

        /// <summary>
        /// Define the layout of a vertex, this is called in the constructor from drawable
        /// </summary>
        protected override void VertexAttributeLayout()
        {
            //Define the layout of the VBO
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, 0);
            GL.EnableVertexAttribArray(0);
        }

        /// <inheritdoc />
        /// <summary>
        /// Generate the VB data for the VBO
        /// </summary>
        protected override float[] VertexBuffer
        {
            get
            {
                const float minX = -1f, minY = -1f, maxX = 1f, maxY = 1f;
                return new[]
                {
                    minX, minY,
                    maxX, minY,
                    maxX, maxY,
                    minX, maxY
                };
            }
        }

        /// <summary>
        /// Should be called once per update to update the position of the snake
        /// </summary>
        public void Update(float DeltaTime)
        {
            PositionFloat += Direction * DeltaTime / Config.TickSpeed;
        }

        /// <summary>
        /// Set the variables for the shader, then draw the component
        /// </summary>
        public override void Draw()
        {
            Shader.SetUniform("transform", ref _transformMatrix);
            Shader.SetUniform("direction", ref _direction);

            base.Draw();
        }

        /// <summary>
        /// Called only if this is the snake head
        /// </summary>
        protected void DrawHead()
        {
            Shader.SetUniform("transform", ref _transformMatrix);
            
            base.Draw();
        }

        /// <inheritdoc />
        /// <summary>
        /// Generate the IB data for the IBO
        /// </summary>
        /// <returns>The newly generated IB</returns>
        protected override uint[] IndexBuffer =>
            new uint[]
            {
                0, 1, 2,
                0, 2, 3
            };
    }
}
