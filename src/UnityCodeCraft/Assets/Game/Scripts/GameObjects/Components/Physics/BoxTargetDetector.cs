using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BoxTargetDetector : MonoBehaviour, ITargetDetector
    {
        [SerializeField] private Transform _origin;
        [SerializeField] private ContactFilter2D _contactFilter;
        [SerializeField] private Vector2 _size = new(1.3f, 1.3f);
        [SerializeField] private Vector2 _offset = new(0.65f, 0f);
        [SerializeField, Min(1)] private int _targetLimit = 5;
        
        private readonly List<Transform> _targets = new();
        private Collider2D[] _results;
        
        public Transform Origin => _origin;
        
        private void Awake() => 
            _results = new Collider2D[_targetLimit];
        
        public IReadOnlyList<Transform> GetTargets()
        {
            Vector3 center = (Vector2)_origin.position + _offset;
            
            int count = Physics2D.OverlapBox(
                center,
                _size,
                0f,
                _contactFilter,
                _results);
            
            _targets.Clear();

            for (int i = 0; i < count; i++) 
                _targets.Add(_results[i].transform);
            
            return _targets;
        }
        
#if UNITY_EDITOR
        [SerializeField] private Color _gizmosColor;

        private void OnDrawGizmos()
        {
            if(_origin == null)
                return;
            
            UnityEditor.Handles.color = _gizmosColor;

            Vector3 center = (Vector2)_origin.position + _offset;

            UnityEditor.Handles.DrawWireCube(center, _size);
        }
#endif
    }
}