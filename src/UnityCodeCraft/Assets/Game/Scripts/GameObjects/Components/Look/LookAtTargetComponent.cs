using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(LookComponent))]
    [RequireComponent(typeof(TargetComponent))]
    public sealed class LookAtTargetComponent : MonoBehaviour
    {
        private LookComponent _lookComponent;
        private TargetComponent _targetComponent;
        
        private Func<bool> _condition;

        private void Awake()
        {
            _lookComponent = GetComponent<LookComponent>();
            _targetComponent = GetComponent<TargetComponent>();
        }
        
        public void SetCondition(Func<bool> condition) => _condition = condition;

        private void FixedUpdate()
        {
            if(_targetComponent.HasTarget &&
               (_condition == null || _condition.Invoke()))
                _lookComponent.Look(_targetComponent.Target);
        }
    }
}