using OpenTK;

namespace Snake
{
    /// <summary>
    /// Contains the config variables for the application
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// Size of the grid
        /// </summary>
        public static Vector2 GridSize => new Vector2(10, 10) / 2;
        /// <summary>
        /// Start position of the head of the snake
        /// </summary>
        public static Vector2 StartPosition => new Vector2(5, 5);
        /// <summary>
        /// Time between the game ticks in seconds
        /// </summary>
        public static float TickSpeed => .5f;
    }
}