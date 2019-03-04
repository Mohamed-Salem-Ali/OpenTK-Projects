using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace Snake
{
    /// <summary>
    /// This class will manage the gameplay and function as the main window class
    /// </summary>
    public class Snake : GameWindow
    {
        /// <summary>
        /// Shader for the snake
        /// </summary>
        private Shader _snakeShader;
        /// <summary>
        /// A list of all the snake components to make the actual snake
        /// </summary>
        private List<SnakeComponent> _snake = new List<SnakeComponent>();

        /// <summary>
        /// Shader for the fruit
        /// For now this is just the same as the snake shader
        /// </summary>
        private Shader _fruitShader;
        /// <summary>
        /// The fruit the snake can pick up to increase its length
        /// </summary>
        private Fruit _fruit;
        /// <summary>
        /// Random generator used to place the fruits
        /// </summary>
        private Random _random;
        /// <summary>
        /// Direction of the head of the snake
        /// </summary>
        private Vector2 _direction = Vector2.Zero;
        /// <summary>
        /// Direction of the head of the snake last time we moved it
        /// </summary>
        private Vector2 _lastDirection;
        /// <summary>
        /// Used to keep track of when the next tick will occour
        /// </summary>
        private float _time;
        
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
            _snakeShader = new Shader("Shaders/snake.vert", "Shaders/basic.frag");
            
            AddSnakePart(4);
            
            _fruitShader = new Shader("Shaders/snake.vert", "Shaders/basic.frag");
            _random = new Random(DateTime.Now.Millisecond + DateTime.Now.Day * 1000 + DateTime.Now.Year * 10000);
            RegenerateFruit();
            
            GL.ClearColor(0.0f, 0.0f, 1.0f, 1.0f);
            base.OnLoad(e);
        }

        /// <summary>
        /// Adds snake components to the snake so it gets longer
        /// </summary>
        /// <param name="count">Amount of snake parts to add</param>
        private void AddSnakePart(int count = 1)
        {
            if (_snake.Count == 0)
            {
                _snake.Add(new SnakeComponent(Config.StartPosition));
                count--;
            }
            for (int i = 0; i < count; i++)
            {
                _snake.Add(new SnakeComponent(_snake.Last().Position));
            }
        }

        /// <summary>
        /// Add a fruit to the game field
        /// </summary>
        private void RegenerateFruit()
        {
            //Generate the new position
            Vector2 position;
            do
            {
                //Get a random position
                position.X = _random.Next(1, (int)Config.GridSize.X * 2);
                position.Y = _random.Next(1, (int)Config.GridSize.Y * 2);
            } while (!ValidPosition(position));
            Console.WriteLine(position);
            
            //If it is the first fruit we generate one
            if (_fruit == null)
            {
                _fruit = new Fruit(position);
            }
            else
            {
                _fruit.Position = position;
            }
        }

        /// <summary>
        /// Fires when a key is pressed
        /// </summary>
        /// <param name="e">event arg passed to us by opentk</param>
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            //Input for the snake
            switch (e.Key)
            {
                case Key.W:
                    _direction = Vector2.UnitY;
                    break;
                case Key.A:
                    _direction = -Vector2.UnitX;
                    break;
                case Key.S:
                    _direction = -Vector2.UnitY;
                    break;
                case Key.D:
                    _direction = Vector2.UnitX;
                    break;
                default:
                    break;
            }

            //Check if we want to move directly backwards
            if (-_direction == _lastDirection)
            {
                _direction = _lastDirection;
            }
            base.OnKeyUp(e);
        }

        /// <summary>
        /// Checks if the position is inside the grid and unique
        /// </summary>
        /// <returns>true if the position is inside the grid and unique, false otherwise</returns>
        private bool ValidPosition(Vector2 position, bool isHead = false)
        {
            //Check if position is inside the grid
            if (position.X < 1 || position.X > Config.GridSize.X * 2 ||
                position.Y < 1 || position.Y > Config.GridSize.Y * 2)
            {
                return false;
            }
            //Check if the position unique
            for (int i = 0; i < _snake.Count; i++)
            {
                if (position == _snake[i].Position && i != 0)
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Update, performs all the game mechanics
        /// </summary>
        /// <param name="e">event arg passed to us by opentk</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _time += (float)e.Time;
            while (_time >= Config.TickSpeed)
            {
                //Move the snake body
                for (int i = _snake.Count - 1; i > 0; i--)
                {
                    _snake[i].Position = _snake[i - 1].Position;
                }
                
                //Move the head
                _snake[0].Position += _direction;
                //Check if the snake head can occupy the new position
                if (!ValidPosition(_snake[0].Position, true))
                {
                    //Dead
                    Console.WriteLine("Dead!");
                }
                
                //Check for the fruit powerup
                if (_snake[0].Position == _fruit.Position)
                {
                    AddSnakePart();
                    RegenerateFruit();
                }

                _lastDirection = _direction;
                _time -= Config.TickSpeed;
            }
            
            base.OnUpdateFrame(e);
        }

        /// <summary>
        /// Renders everything
        /// </summary>
        /// <param name="e">event arg passed to us by opentk</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            
            //TODO: Draw the background here
            
            //Draw everything here
            
            _fruitShader.Bind();
            _fruit.Draw();
            
            _snakeShader.Bind();

            for (int i = 0; i < _snake.Count; i++)
            {
                _snake[i].Draw();
            }
            
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
            _fruit.Dispose();
            _fruitShader.Dispose();
            
            _snakeShader.Dispose();
            for (int i = 0; i < _snake.Count; i++)
            {
                _snake[i].Dispose();
            }
            
            base.OnUnload(e);
        }
    }
}
