using ConsoleGameEngine;
using System;
using System.Numerics;

namespace RexMinus1.GameObjects
{
    public class Enemy : Model, IMoveable
    {
        private DateTime lastHit = DateTime.Parse("1970-01-01");

        public float Shield { get; set; }

        public bool IsIdentified { get; set; } = false;

        public void Hit(Vector3 player)
        {
            if (Math.Abs(Vector3.Dot(player, this.Position)) < 12 && DateTime.Now - lastHit > TimeSpan.FromMilliseconds(1000))
            {
                Shield -= 0.5f;
                lastHit = DateTime.Now;
            }
        }

        public void Move()
        {
        }
    }
}