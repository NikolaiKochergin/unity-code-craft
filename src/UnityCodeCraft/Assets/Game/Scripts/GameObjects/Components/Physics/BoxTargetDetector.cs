using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BoxTargetDetector : TargetDetector
    {
        [SerializeField] private Transform _origin;
        [SerializeField] private ContactFilter2D _contactFilter;
        [SerializeField] private Vector2 _size = new(1.3f, 1.3f);
        [SerializeField] private Vector2 _offset = new(0.65f, 0f);
        [SerializeField, Min(1)] private int _targetLimit = 5;
        
        private readonly List<Transform> _targets = new();
        private Collider2D[] _results;
        
        private void Awake() => 
            _results = new Collider2D[_targetLimit];
        
        public override IReadOnlyList<Transform> GetTargets()
        {
            int count = Physics2D.OverlapBox(
                Center(),
                _size,
                0f,
                _contactFilter,
                _results);
            
            _targets.Clear();

            for (int i = 0; i < count; i++) 
                _targets.Add(_results[i].transform);
            
            return _targets;
        }

        private Vector2 Center() => 
            (Vector2)_origin.position + (Vector2)(_origin.right * _offset.x) + (Vector2)(_origin.up * _offset.y);

#if UNITY_EDITOR
        [SerializeField] private Color _gizmosColor;

        private void OnDrawGizmos()
        {
            if(_origin == null)
                return;
            
            UnityEditor.Handles.color = _gizmosColor;
            UnityEditor.Handles.DrawWireCube(Center(), _size);
        }
#endif
    }
}