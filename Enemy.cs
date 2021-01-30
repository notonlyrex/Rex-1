using ConsoleGameEngine;
using System;
using System.Numerics;

namespace RexMinus1
{
    public class Enemy : Model
    {
        private DateTime lastHit = DateTime.Parse("1970-01-01");

        public float Shield { get; set; }

        public void Hit(Vector3 player)
        {
            if (Math.Abs(Vector3.Dot(player, this.Position)) < 12 && DateTime.Now - lastHit > TimeSpan.FromMilliseconds(1000))
            {
                Shield -= 0.5f;
                lastHit = DateTime.Now;
            }
        }
    }
}