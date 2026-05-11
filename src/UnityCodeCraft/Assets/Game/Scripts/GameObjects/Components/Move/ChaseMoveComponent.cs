using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MoveComponent))]
    [RequireComponent(typeof(TargetComponent))]
    public sealed class ChaseMoveComponent : MonoBehaviour
    {
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField, Min(0)] private float _reachDistance = 0.1f;
        
        private TargetComponent _targetComponent;

        public bool IsMoving => _moveComponent.IsMoving;
        
        public event Action<Vector2> OnMoved
        {
            add => _moveComponent.OnMoved += value;
            remove => _moveComponent.OnMoved -= value;
        }

        private void Awake() => 
            _targetComponent = GetComponent<TargetComponent>();

        public void SetCondition(Func<bool> condition) => _moveComponent.SetCondition(condition);
        public void SetAction(Action<Vector2, float> action) => _moveComponent.SetAction(action);

        private void FixedUpdate()
        {
            if(!_targetComponent.HasTarget || 
               Vector2.Distance(_targetComponent.TargetPosition, transform.position) <= _reachDistance)
                return;
            
            Vector2 direction = new Vector2(_targetComponent.TargetPosition.x - transform.position.x, 0).normalized;
            _moveComponent.Move(direction, Time.fixedDeltaTime);
        }

#if UNITY_EDITOR
        private void Reset() => 
            _moveComponent = GetComponent<MoveComponent>();
#endif
    }
}