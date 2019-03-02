using System;
using OpenTK.Graphics.OpenGL4;

namespace TestProject
{
    internal static class MainClass
    {
        public static void Main(string[] args)
        {
            using (Window window = new Window(500, 500, "Hello world"))
            {
                window.Run(30.0);
            }
        }
    }
}
