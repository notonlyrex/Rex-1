using ConsoleGameEngine;

namespace RexMinus1.Animations
{
    internal class Laser : Animation
    {
        private Point start1 = new Point(6, 48);
        private Point start2 = new Point(125, 49);

        private Point end = new Point(57, 32);
        private Point end2 = new Point(74, 32);

        private int currentFrame = 0;

        public override void Render()
        {
            if (!IsPaused && currentFrame < 10)
            {
                Engine.Line(start1, end, 12);
                Engine.Line(start2, end2, 12);
                currentFrame++;
            }
            else
            {
                IsPaused = true;
            }
        }

        public override void Reset()
        {
            currentFrame = 0;
            IsPaused = true;
        }
    }
}