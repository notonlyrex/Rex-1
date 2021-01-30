using ConsoleGameEngine;
using System.Numerics;

namespace RexMinus1.GameObjects
{
    public class Mine : Enemy
    {
        public Mine()
        {
            Mesh = Mesh.LoadFromObj("Assets/obj_mine.obj");
            Shield = 0.2f;
            CollisionAttack = 0.3f;
        }

        public override bool Collision(Vector3 player)
        {
            if (base.Collision(player))
            {
                AudioPlaybackEngine.Instance.PlayCachedSound("boom");
                Shield = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Move()
        {
            RotationY += 0.05f;
            base.Move();
        }
    }
}