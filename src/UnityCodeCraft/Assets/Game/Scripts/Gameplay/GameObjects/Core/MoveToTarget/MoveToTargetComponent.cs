using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class MoveToTargetComponent : NetworkBehaviour
    {
        public interface ICondition
        {
            public bool IsMet();
        }
        
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private float _reachDistance = 0.05f;
        [SerializeField] private Transform _target;
        
        private ICondition _condition;
        
        public void SetTarget(Transform target) => _target = target;
        
        public void SetCondition(ICondition condition) => _condition = condition;

        public override void FixedUpdateNetwork()
        {
            if(_condition != null && !_condition.IsMet())
                return;
            
            if(_target == null)
                return;

            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = _target.position;
            Vector3 delta = targetPosition - currentPosition;
            delta.y = 0;
            
            if(delta.sqrMagnitude > _reachDistance * _reachDistance)
                _moveComponent.Move(delta.normalized);
        }
    }
}