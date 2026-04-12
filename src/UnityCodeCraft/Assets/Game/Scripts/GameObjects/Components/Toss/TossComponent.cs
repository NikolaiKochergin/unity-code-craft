using System;
using UnityEngine;

namespace Game
{
    public class TossComponent : MonoBehaviour
    {
        [SerializeField] private Transform _tossPoint;
        [SerializeField] private Vector2 _tossForce = new(1, 10);
        [SerializeField] private ContactFilter2D _contactFilter;
        [SerializeField] private float _high = 1.3f;
        [SerializeField] private float _width = 1.3f;

        private readonly Collider2D[] _results = new Collider2D[5];

        public event Action OnToss;

        public void Toss()
        {
            Vector3 center = _tossPoint.position + _tossPoint.right * _width / 2;
            Vector2 size = new(_width, _high);
            
            int count = Physics2D.OverlapBox(
                center,
                size,
                0f,
                _contactFilter,
                _results);
            
            for (int i = 0; i < count; i++)
            {
                Collider2D result = _results[i];
            
                if (result.TryGetComponent(out Rigidbody2D rigidBody))
                    rigidBody.AddForce(Force(), ForceMode2D.Impulse);
            }

            OnToss?.Invoke();
        }

        private Vector2 Force() =>
            _tossPoint.right * _tossForce.x + _tossPoint.up * _tossForce.y;

#if UNITY_EDITOR
        [SerializeField] private Color _gizmosColor;

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = _gizmosColor;

            Vector3 center = _tossPoint.position + _tossPoint.right * _width / 2;
            Vector3 size = new(_width, _high);

            UnityEditor.Handles.DrawWireCube(center, size);
        }
#endif
    }
}