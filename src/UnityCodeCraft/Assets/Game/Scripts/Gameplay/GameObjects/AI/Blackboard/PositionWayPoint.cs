using UnityEngine;

namespace Game
{
    public class PositionWayPoint : IWayPoint
    {
        public PositionWayPoint(Vector3 position)
        {
            Position = position;
        }

        public Vector3 Position { get; }
        public float Size => 0f;
        public bool IsValid => true;
    }
}