using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class TargetDetector : MonoBehaviour
    {
        [SerializeField] private Transform _origin;
        [SerializeField] private ContactFilter2D _contactFilter;
        [SerializeField, Min(1)] private int _targetLimit = 5;
        [SerializeField] private Vector2 _offset = new(0.65f, 0f);
        
        private readonly List<GameObject> _targets = new();
        private Collider2D[] _results;
        
        protected Vector2 Origin => (Vector2)_origin.position + (Vector2)(_origin.right * _offset.x) + (Vector2)(_origin.up * _offset.y);
        protected Vector2 Direction => _origin.right;
        
        private void Awake() => 
            _results = new Collider2D[_targetLimit];

        public IReadOnlyList<GameObject> GetTargets()
        {
            _targets.Clear();
            int count = Cast(_contactFilter, _results);

            for (int i = 0; i < count; i++) 
                _targets.Add(_results[i].gameObject);
            
            return _targets;
        }

        protected abstract int Cast(ContactFilter2D filter, Collider2D[] results);
    }
}