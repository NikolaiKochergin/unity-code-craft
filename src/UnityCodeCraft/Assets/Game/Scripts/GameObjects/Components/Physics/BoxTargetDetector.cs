using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BoxTargetDetector : MonoBehaviour, ITargetDetector
    {
        [SerializeField] private Transform _origin;
        [SerializeField] private ContactFilter2D _contactFilter;
        [SerializeField] private float _high = 1.3f;
        [SerializeField] private float _width = 1.3f;
        [SerializeField, Min(1)] private int _targetLimit = 5;
        
        private readonly List<Transform> _targets = new();
        private Collider2D[] _results;
        
        public Transform Origin => _origin;
        
        private void Awake() => 
            _results = new Collider2D[_targetLimit];
        
        public IReadOnlyList<Transform> GetTargets()
        {
            Vector3 center = _origin.position + _origin.right * _width / 2;
            Vector2 size = new(_width, _high);
            
            int count = Physics2D.OverlapBox(
                center,
                size,
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

            Vector3 center = _origin.position + _origin.right * _width / 2;
            Vector3 size = new(_width, _high);

            UnityEditor.Handles.DrawWireCube(center, size);
        }
#endif
    }
}