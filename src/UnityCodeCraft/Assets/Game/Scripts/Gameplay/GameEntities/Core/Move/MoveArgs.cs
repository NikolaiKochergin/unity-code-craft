using UnityEngine;

namespace Game.Gameplay
{
    public struct MoveArgs
    {
        public Vector3 Direction;
        public float DeltaTime;

        public MoveArgs(Vector3 direction, float deltaTime)
        {
            Direction = direction;
            DeltaTime = deltaTime;
        }
    }
}