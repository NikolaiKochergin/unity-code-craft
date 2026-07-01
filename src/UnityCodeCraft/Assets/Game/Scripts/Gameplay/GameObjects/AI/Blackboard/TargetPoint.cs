using UnityEngine;

namespace Game
{
    public class TargetPoint : IPoint
    {
        private readonly Transform _transform;

        public TargetPoint(Transform transform, float size = 1f)
        {
            _transform = transform;
            Size = size;
        }

        public GameObject GameObject => _transform.gameObject;
        public Vector3 Position => _transform.position;
        public float Size { get; }
        public bool IsValid => _transform;
    }
}