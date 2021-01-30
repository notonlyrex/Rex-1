namespace RexMinus1
{
    public class PlayerManager
    {
        public static readonly PlayerManager Instance = new PlayerManager();

        public bool IsMusicEnabled { get; set; }

        /// <summary>
        /// From 0.0 to 1.0
        /// </summary>
        public float Shields { get; internal set; }

        /// <summary>
        /// From 0.0 to 1.0
        /// </summary>
        public float Energy { get; internal set; }

        /// <summary>
        /// From 0.0 to 1.0
        /// </summary>
        public float Heat { get; internal set; }
    }
}