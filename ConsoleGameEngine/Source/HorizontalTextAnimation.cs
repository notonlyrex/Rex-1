namespace ConsoleGameEngine
{
    public class HorizontalTextAnimation : Animation
    {
        /// <summary>
        /// Jaki tekst
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Ile znaków na klatkę
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// Kolor
        /// </summary>
        public int Color { get; set; }

        /// <summary>
        /// Punkt startowy
        /// </summary>
        public Point Origin { get; set; }

        private int currentFrame = 0;

        public override void Render()
        {
            if (!IsPaused && currentFrame + Speed < Text.Length)
            {
                Engine.WriteText(Origin, Text.Substring(0, currentFrame + Speed), Color);
                currentFrame++;
            }
            else
            {
                IsPaused = true;
                Engine.WriteText(Origin, Text, Color);
            }
        }

        public override void Reset()
        {
            currentFrame = 0;
            IsPaused = false;
        }
    }
}