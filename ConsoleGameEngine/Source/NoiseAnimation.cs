using System;

namespace ConsoleGameEngine
{
    public class NoiseAnimation : Animation
    {
        private Random r = new Random();

        public int Intensity { get; set; }
        private int currentFrame = 0;

        public override void Render()
        {
            for (int i = 0; i < Intensity; i++)
            {
                var x = r.Next(Engine.WindowSize.X - 2) + 1;
                var y = r.Next(Engine.WindowSize.Y - 2) + 1;
                var c0 = r.Next(16);

                Engine.SetPixel(new Point(x, y), c0, '.');
            }
        }

        public override void Reset()
        {
            currentFrame = 0;
            IsPaused = false;
        }
    }
}