namespace ConsoleGameEngine
{
    public class StartingAnimation : Animation
    {
        private int currentFrame = 0;

        public int Speed { get; set; }

        public int ForegroundColor { get; set; }

        public int BackgroundColor { get; set; }

        public ConsoleCharacter Character { get; set; }

        public override void Render()
        {
            if (IsPaused)
                return;

            currentFrame += Speed;
            if (currentFrame > Engine.WindowSize.Y)
                IsPaused = true;

            for (int j = 0; j < currentFrame; j++)
            {
                for (int i = 0; i < Engine.WindowSize.X; i++)
                {
                    Engine.SetPixel(new Point(i, j), ForegroundColor, BackgroundColor, Character);
                }
            }
        }

        public override void Reset()
        {
            currentFrame = 0;
            IsPaused = false;
        }
    }
}