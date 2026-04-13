using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TossAbility : MonoBehaviour,
        TossRequestComponent.ICondition,
        TossRequestComponent.IAction
    {
        private TossRequestComponent _tossRequest;
        private ForceComponent _force;
        private ITargetDetector _targetDetector;
        private DelayComponent _delay;
        private CooldownComponent _cooldown;
        
        public event Action OnToss;

        private void Awake()
        {
            _tossRequest = GetComponent<TossRequestComponent>();
            _force = GetComponent<ForceComponent>();
            _targetDetector = GetComponent<BoxTargetDetector>();
            _delay = GetComponent<DelayComponent>();
            _cooldown = GetComponent<CooldownComponent>();
            
            _tossRequest.SetCondition(this);
            _tossRequest.SetAction(this);
        }

        bool TossRequestComponent.ICondition.Evaluate() => 
            _cooldown.IsExpired;

        void TossRequestComponent.IAction.Invoke()
        {
            _delay.DelayedInvoke(PushPossibleTargets);
            _cooldown.Reset();
            OnToss?.Invoke();
        }
        
        private void PushPossibleTargets()
        {
            IReadOnlyList<Transform> targets = _targetDetector.GetTargets();
            Vector2 direction = _targetDetector.Origin.right + _targetDetector.Origin.up;

            foreach (Transform target in targets)
                _force.ApplyTo(target, direction);
        }
    }
}