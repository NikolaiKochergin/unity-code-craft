using System;
using UnityEngine;

namespace Game
{
    public class PushComponent : DelayedAction
    {
        [SerializeField] private Transform _pushPoint;
        [SerializeField] private Vector2 _pushForce;
        [SerializeField] private ContactFilter2D _contactFilter;
        [SerializeField] private float _angle = 50f;
        [SerializeField] private float _distance = 4.5f;

        private readonly Collider2D[] _results = new Collider2D[5];
        
        public event Action OnPush;
        
        public void Push()
        {
            int count = Physics2D.OverlapCircle(
                _pushPoint.position, 
                _distance, 
                _contactFilter, 
                _results);

            for (int i = 0; i < count; i++)
            {
                Collider2D result = _results[i];
                
                Vector2 dirToTarget = (result.transform.position - _pushPoint.position).normalized;
                
                float angleToTarget = Vector2.Angle(_pushPoint.right, dirToTarget);
                
                if(angleToTarget > _angle / 2f)
                    continue;
                
                if(result.TryGetComponent(out Rigidbody2D rigidBody))
                    rigidBody.AddForce(Force(), ForceMode2D.Impulse);
            }
            
            OnPush?.Invoke();
        }

        private Vector2 Force() => 
            _pushPoint.right * _pushForce.x + _pushPoint.up * _pushForce.y;

#if UNITY_EDITOR
        [SerializeField] private Color _gizmosColor;
        
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = _gizmosColor;

            Vector3 normal = Vector3.forward;
            Vector3 origin = _pushPoint.position;
            Vector3 direction = _pushPoint.right;

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