using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PushAbility : MonoBehaviour,
        AbilityRequestComponent.ICondition,
        AbilityRequestComponent.IAction
    {
        private AbilityRequestComponent _pushRequest;
        private ForceComponent _force;
        private ITargetDetector _targetDetector;
        private DelayComponent _delay;
        private CooldownComponent _cooldown;

        public event Action OnPush;

        private void Awake()
        {
            _pushRequest = GetComponent<AbilityRequestComponent>();
            _force = GetComponent<ForceComponent>();
            _targetDetector = GetComponent<ArcTargetDetectorComponent>();
            _delay = GetComponent<DelayComponent>();
            _cooldown = GetComponent<CooldownComponent>();

            _pushRequest.SetCondition(this);
            _pushRequest.SetAction(this);
        }

        public void Use() => 
            _pushRequest?.Require();

        bool AbilityRequestComponent.ICondition.Evaluate() => 
            _cooldown.IsExpired;

        void AbilityRequestComponent.IAction.Invoke()
        {
            _delay.DelayedInvoke(PushPossibleTargets);
            _cooldown.Reset();
            OnPush?.Invoke();
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