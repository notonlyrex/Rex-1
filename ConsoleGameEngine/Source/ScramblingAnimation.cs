using System;

namespace ConsoleGameEngine
{
    public class ScramblingAnimation : Animation
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
                var dx = r.Next(-2, 2);
                var dy = r.Next(-2, 2);

                var g = Engine.PixelAt(new Point(x, y));
                Engine.SetPixel(new Point(x + dx, y + dy), g.fg, g.bg, g.c);
            }
        }

        public override void Reset()
        {
            currentFrame = 0;
            IsPaused = false;
        }
    }
}