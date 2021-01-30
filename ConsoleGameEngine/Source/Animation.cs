namespace ConsoleGameEngine
{
    public abstract class Animation
    {
        public ConsoleEngine Engine { get; set; }

        public bool IsPaused { get; set; }

        public abstract void Render();

        public abstract void Reset();

        public Animation()
        {
            IsPaused = false;
        }
    }
}