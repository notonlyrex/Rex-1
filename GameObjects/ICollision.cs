using System.Numerics;

namespace RexMinus1.GameObjects
{
    public interface ICollision
    {
        bool Collision(Vector3 player);

        float CollisionAttack { get; set; }
    }
}