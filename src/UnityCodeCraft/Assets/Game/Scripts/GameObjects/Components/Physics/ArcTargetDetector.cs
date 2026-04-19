using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ArcTargetDetectorComponent : TargetDetector
    {
        [SerializeField] private Transform _origin;
        [SerializeField] private ContactFilter2D _contactFilter;
        [SerializeField] private float _angle = 50f;
        [SerializeField] private float _distance = 4.5f;
        [SerializeField, Min(1)] private int _targetLimit = 5;
        
        private readonly List<Transform> _targets = new();
        private Collider2D[] _results;
        
        private void Awake() => 
            _results = new Collider2D[_targetLimit];

        public override IReadOnlyList<Transform> GetTargets()
        {
            int count = Physics2D.OverlapCircle(
                _origin.position, 
                _distance, 
                _contactFilter, 
                _results);
            
            _targets.Clear();
            
            for (int i = 0; i < count; i++)
            {
                Transform target = _results[i].transform;
                
                Vector2 dirToTarget = (target.position - _origin.position).normalized;
                float angleToTarget = Vector2.Angle(_origin.right, dirToTarget);
                
                if(angleToTarget <= _angle / 2f)
                    _targets.Add(target);
            }
            
            return _targets;
        }
        
#if UNITY_EDITOR
        [SerializeField] private Color _gizmosColor;
        
        private void OnDrawGizmos()
        {
            if(_origin == null)
                return;
            
            UnityEditor.Handles.color = _gizmosColor;

            Vector3 normal = Vector3.forward;
            Vector3 origin = _origin.position;
            Vector3 direction = _origin.right;

            Vector3 startDir = Quaternion.Euler(0, 0, -_angle / 2f) * direction;
            Vector3 dirA = Quaternion.Euler(0, 0, -_angle / 2f) * direction;
            Vector3 dirB = Quaternion.Euler(0, 0, _angle / 2f) * direction;
            
            UnityEditor.Handles.DrawWireArc(origin, normal, startDir, _angle, _distance);
            UnityEditor.Handles.DrawLine(origin, origin + dirA * _distance);
            UnityEditor.Handles.DrawLine(origin, origin + dirB * _distance);
        }
#endif
    }
}