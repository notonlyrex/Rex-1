using ConsoleGameEngine;
using System;
using System.Numerics;

namespace RexMinus1
{
    internal class Astronaut : Model, ICollision
    {
        public bool IsCollected { get; set; }

        public void Collision(Vector3 player)
        {
            if (Math.Abs(Vector3.Dot(player, this.Position)) < 10)
            {
                IsCollected = true;
            }
        }
    }
}