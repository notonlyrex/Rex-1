using System.Numerics;

namespace ConsoleGameEngine
{
    public class Model
    {
        public Mesh Mesh { get; set; }
        public Vector3 Position { get; set; }

        public float RotationY { get; set; }
        public float RotationX { get; set; }
    }
}