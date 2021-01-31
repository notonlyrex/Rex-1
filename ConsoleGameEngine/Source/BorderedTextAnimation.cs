namespace ConsoleGameEngine
{
    public class BorderedTextAnimation : Animation
    {
        /// <summary>
        /// Jaki tekst
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Ile klatek zanim zniknie
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// Kolor tekstu oraz ramki
        /// </summary>
        public int Color { get; set; }

        /// <summary>
        /// Kolor tła tekstu oraz ramki
        /// </summary>
        public int BackgroundColor { get; set; }

        public Point Origin { get; set; }

        public bool IsCentered { get; set; }

        private int currentFrame = 0;
        private bool isVisible = false;

        public override void Render()
        {
            if (!IsPaused)
            {
                if (isVisible)
                {
                    if (IsCentered)
                        Origin = new Point(Engine.WindowSize.X / 2, Engine.WindowSize.Y / 2);

                    Engine.Frame(new Point(Origin.X - Text.Length / 2 - 1, Origin.Y - 1), new Point(Origin.X + Text.Length / 2 + 1, Origin.Y + 1), Color, BackgroundColor);
                    Engine.WriteText(new Point(Origin.X - Text.Length / 2, Origin.Y), Text, Color, BackgroundColor);
                }

                if (currentFrame > Speed)
                {
                    currentFrame = 0;
                    isVisible = !isVisible;
                }

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
            IsPaused = false;
            isVisible = false;
        }
    }
}