using System.Numerics;

namespace RexMinus1.GameObjects
{
    public interface ICollision
    {
        float Collision(Vector3 player);

        float CollisionAttack { get; set; }

        float CollisionRange { get; set; }

        float DetectionRange { get; set; }

        float IdentificationRange { get; set; }

        bool IsVisible { get; set; }

        bool IsIdentified { get; set; }

        bool IsDetected { get; set; }
    }
}