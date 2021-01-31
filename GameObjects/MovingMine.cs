using ConsoleGameEngine;
using System;
using System.Numerics;

namespace RexMinus1.GameObjects
{
    public class MovingMine : Enemy
    {
        private int dir = 0;

        public MovingMine()
        {
            Mesh = Mesh.LoadFromObj("Assets/obj_mine.obj");
            Shield = 0.2f;
            CollisionAttack = 0.3f;

            CollisionRange = 10;
            DetectionRange = 100;
            IdentificationRange = 50;
            HitRange = 40;
            IsVisible = true;

            Random r = new Random();
            dir = r.Next(-1, 2);
        }

        public override float Collision(Vector3 player)
        {
            var d = base.Collision(player);

            if (d < CollisionRange)
            {
                AudioPlaybackEngine.Instance.PlayCachedSound("boom");
                Shield = 0;
            }

            return d;
        }

        public override void Move()
        {
            RotationY += 0.05f;

            Position = new Vector3(Position.X, Position.Y, Position.Z + dir * 0.1f);

            base.Move();
        }
    }
}