using ConsoleGameEngine;
using System;
using System.Numerics;

namespace RexMinus1.GameObjects
{
    public class Enemy : Model, IMoveable, ICollision
    {
        private DateTime lastHit = DateTime.Parse("1970-01-01");

        public float Shield { get; set; }

        public float CollisionAttack { get; set; }

        public bool IsIdentified { get; set; } = false;

        public virtual bool Collision(Vector3 player)
        {
            return Math.Abs(Vector3.Dot(player, Position)) < 10;
        }

        public virtual void Hit(Vector3 player)
        {
            if (Math.Abs(Vector3.Dot(player, this.Position)) < 12 && DateTime.Now - lastHit > TimeSpan.FromMilliseconds(1000))
            {
                Shield -= 0.1f;
                lastHit = DateTime.Now;
            }
        }

        public virtual void Move()
        {
        }
    }
}