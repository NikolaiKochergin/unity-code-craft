using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TossAbility : MonoBehaviour,
        AbilityRequestComponent.ICondition,
        AbilityRequestComponent.IAction
    {
        private AbilityRequestComponent _tossRequest;
        private ForceComponent _force;
        private ITargetDetector _targetDetector;
        private DelayComponent _delay;
        private CooldownComponent _cooldown;
        
        public event Action OnToss;

        private void Awake()
        {
            _tossRequest = GetComponent<AbilityRequestComponent>();
            _force = GetComponent<ForceComponent>();
            _targetDetector = GetComponent<BoxTargetDetector>();
            _delay = GetComponent<DelayComponent>();
            _cooldown = GetComponent<CooldownComponent>();
            
            _tossRequest.SetCondition(this);
            _tossRequest.SetAction(this);
        }

        public void Use() => 
            _tossRequest.Require();

        bool AbilityRequestComponent.ICondition.Evaluate() => 
            _cooldown.IsExpired;

        void AbilityRequestComponent.IAction.Invoke()
        {
            _delay.DelayedInvoke(TossPossibleTargets);
            _cooldown.Reset();
            OnToss?.Invoke();
        }

        private void TossPossibleTargets()
        {
            IReadOnlyList<Transform> targets = _targetDetector.GetTargets();
            Vector2 direction = _targetDetector.Origin.right + _targetDetector.Origin.up;

            foreach (Transform target in targets)
                _force.ApplyTo(target, direction);
        }
    }
}