using ConsoleGameEngine;
using System;
using System.Numerics;

namespace RexMinus1.GameObjects
{
    internal class Astronaut : Model, ICollision
    {
        public bool IsCollected { get; set; }

        public float CollisionAttack { get; set; } = 0;

        public bool Collision(Vector3 player)
        {
            if (Math.Abs(Vector3.Dot(player, this.Position)) < 10)
            {
                AudioPlaybackEngine.Instance.PlayCachedSound("beep_3");
                IsCollected = true;

                return true;
            }

            return false;
        }
    }
}