using UnityEngine;

namespace Game.Gameplay
{
    public struct AimArgs
    {
        public readonly Vector3 Direction;
        public readonly float DeltaTime;

        public AimArgs(Vector3 direction, float deltaTime)
        {
            Direction = direction;
            DeltaTime = deltaTime;
        }
    }
}