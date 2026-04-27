using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MoveTransformComponent))]
    public class WaypointMoveComponent : MonoBehaviour
    {
        [SerializeField] private GameObject[] _waypoints;
        [SerializeField, Min(0)] private float _reachDistance = 0.1f;
        [SerializeField] private float _moveDuration = 0.1f;
        
        private MoveTransformComponent _moveTransformComponent;
        private Func<bool> _condition;
        private int _index;
        private float _moveTime;
        
        public bool IsMoving => Time.time <= _moveTime;
        
        public event Action<Vector2> OnMoved;

        private void Awake() => 
            _moveTransformComponent = GetComponent<MoveTransformComponent>();

        public void SetCondition(Func<bool> condition) => _condition = condition;

        private void FixedUpdate()
        {
            if (_waypoints.Length <= 0 || (_condition != null && !_condition.Invoke())) 
                return;
            
            Vector2 target = _waypoints[_index].transform.position;
            Vector2 current = transform.position;
            Vector2 direction = (target - current).normalized;
            
            _moveTransformComponent.Move(direction);

            if (Vector2.Distance(target, current) <= _reachDistance && ++_index >= _waypoints.Length)
                _index = 0;
            
            _moveTime = Time.time + _moveDuration;
            OnMoved?.Invoke(direction);
        }
    }
}