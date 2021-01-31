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
        public float CollisionRange { get; set; }
        public float DetectionRange { get; set; }

        public float IdentificationRange { get; set; }

        public bool IsIdentified { get; set; } = false;

        public bool IsVisible { get; set; } = true;

        public float HitRange { get; set; } = 12;

        public float HitAngle { get; set; } = 30;

        public bool IsDetected { get; set; } = false;

        public virtual float Collision(Vector3 player)
        {
            var d = Math.Sqrt(Math.Pow(Position.X - player.X, 2) + Math.Pow(Position.Y - player.Y, 2) + Math.Pow(Position.Z - player.Z, 2));
            return (float)d;
            //return Math.Abs(Vector3.Dot(player, Position));
        }

        public virtual void Hit(Vector3 playerPosition, Vector3 playerRotation)
        {
            var angle2 = CustomMath.ConvertRadiansToDegrees(CustomMath.SimpleAngleBetweenTwoVectors(playerRotation, this.Position - playerPosition));
            var distance = Math.Sqrt(Math.Pow(Position.X - playerPosition.X, 2) + Math.Pow(Position.Y - playerPosition.Y, 2) + Math.Pow(Position.Z - playerPosition.Z, 2));

            if (distance < HitRange && Math.Abs(angle2) < HitAngle && DateTime.Now - lastHit > TimeSpan.FromMilliseconds(1000))
            {
                Shield -= 0.1f;
                AudioPlaybackEngine.Instance.PlayCachedSound("boom_3");
                lastHit = DateTime.Now;
            }
        }

        public virtual void Move()
        {
        }
    }
}