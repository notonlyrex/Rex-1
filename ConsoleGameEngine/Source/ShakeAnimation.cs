using System;
using System.Collections.Generic;

namespace ConsoleGameEngine
{
    public class ShakeAnimation : Animation
    {
        public int IntensityX { get; set; }
        public int IntensityY { get; set; }

        public int Smooth { get; set; }

        private int currentFrame = 0;

        private List<int> movementsX;
        private List<int> movementsY;

        public ShakeAnimation(int intensityX, int intensityY, int speed)
        {
            IntensityX = intensityX;
            IntensityY = intensityY;
            Smooth = speed;

            movementsX = new List<int>();
            movementsY = new List<int>();

            for (int i = 0; i < Smooth; i++)
            {
                movementsX.Add((int)(Math.Sin(i) * IntensityX));
            }

            for (int i = 0; i < Smooth; i++)
            {
                movementsY.Add((int)(Math.Sin(i) * IntensityY));
            }
        }

        public override void Render()
        {
            if (!IsPaused)
            {
                for (int i = IntensityX; i < Engine.WindowSize.X - IntensityX; i++)
                {
                    for (int j = IntensityY; j < Engine.WindowSize.Y - IntensityY; j++)
                    {
                        var g = Engine.PixelAt(new Point(i, j));
                        Engine.SetPixel(new Point(i + movementsX[currentFrame], j + movementsY[currentFrame]), g.fg, g.bg, g.c);
                    }
                }
                currentFrame++;

                if (currentFrame >= Smooth)
                    currentFrame = 0;
            }
        }

        public override void Reset()
        {
            currentFrame = 0;
            IsPaused = false;
        }
    }
}