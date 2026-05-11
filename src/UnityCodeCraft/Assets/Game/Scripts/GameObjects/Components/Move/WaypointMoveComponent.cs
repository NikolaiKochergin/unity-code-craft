using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MoveComponent))]
    public sealed class WaypointMoveComponent : MonoBehaviour
    {
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private GameObject[] _waypoints;
        [SerializeField, Min(0)] private float _reachDistance = 0.1f;
        
        private int _index;
        
        public bool IsMoving => _moveComponent.IsMoving;
        
        public event Action<Vector2> OnMoved
        {
            add => _moveComponent.OnMoved += value;
            remove => _moveComponent.OnMoved -= value;
        }

        public void SetCondition(Func<bool> condition) => _moveComponent.SetCondition(condition);
        public void SetAction(Action<Vector2, float> action) => _moveComponent.SetAction(action);

        private void FixedUpdate()
        {
            if (_waypoints.Length <= 0) 
                return;
            
            Vector2 target = _waypoints[_index].transform.position;
            Vector2 current = transform.position;
            Vector2 direction = (target - current).normalized;
            
            _moveComponent.Move(direction, Time.fixedDeltaTime);

            if (Vector2.Distance(target, current) <= _reachDistance)
                _index = ++_index % _waypoints.Length;
        }

#if UNITY_EDITOR
        private void Reset() => 
            _moveComponent = GetComponent<MoveComponent>();
#endif
    }
}