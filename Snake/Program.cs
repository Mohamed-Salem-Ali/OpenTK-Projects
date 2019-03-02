using System;

namespace Snake
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            using (Snake snake = new Snake(500, 500, "snake"))
            {
                snake.Run(60.0f);
            }
        }
    }
}