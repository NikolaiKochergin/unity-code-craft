using UnityEngine;

namespace Game
{
    public class TargetWayPoint : IWayPoint
    {
        private readonly Transform _transform;

        public TargetWayPoint(Transform transform, float size = 1f)
        {
            _transform = transform;
            Size = size;
        }

        public Vector3 Position => _transform.position;
        public float Size { get; }
        public bool IsValid => _transform;
    }
}