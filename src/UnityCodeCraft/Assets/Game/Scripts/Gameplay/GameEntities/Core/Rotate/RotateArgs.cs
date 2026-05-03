using UnityEngine;

namespace Game.Gameplay
{
    public struct RotateArgs
    {
        public readonly Vector3 Direction;
        public readonly float DeltaTime;

        public RotateArgs(Vector3 direction, float deltaTime)
        {
            Direction = direction;
            DeltaTime = deltaTime;
        }
    }
}