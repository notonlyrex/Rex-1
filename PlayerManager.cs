namespace RexMinus1
{
    public class PlayerManager
    {
        public static readonly PlayerManager Instance = new PlayerManager();

        public bool IsMusicEnabled { get; set; }

        /// <summary>
        /// From 0.0 to 1.0
        /// </summary>
        public float Shield { get; internal set; }

        /// <summary>
        /// From 0.0 to 1.0
        /// </summary>
        public float Energy { get; internal set; }

        /// <summary>
        /// From 0.0 to 1.0
        /// </summary>
        public float Heat { get; internal set; }

        public float EnergyToAccelerate { get; set; }
        public float EnergyToDecelerate { get; set; }

        public float Acceleration { get; set; }
        public float Deceleration { get; set; }

        public float HeatToAccelerate { get; set; }
        public float HeatToShoot { get; set; }
        public float EnergyToShoot { get; set; }
    }
}