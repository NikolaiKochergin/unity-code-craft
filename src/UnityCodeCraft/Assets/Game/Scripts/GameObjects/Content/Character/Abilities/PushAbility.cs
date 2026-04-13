using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PushAbility : MonoBehaviour,
        PushRequestComponent.ICondition,
        PushRequestComponent.IAction
    {
        private PushRequestComponent _pushRequest;
        private ForceComponent _force;
        private ITargetDetector _targetDetector;
        private DelayComponent _delay;
        private CooldownComponent _cooldown;

        public event Action OnPush;

        private void Awake()
        {
            _pushRequest = GetComponent<PushRequestComponent>();
            _force = GetComponent<ForceComponent>();
            _targetDetector = GetComponent<ArcTargetDetectorComponent>();
            _delay = GetComponent<DelayComponent>();
            _cooldown = GetComponent<CooldownComponent>();

            _pushRequest.SetCondition(this);
            _pushRequest.SetAction(this);
        }
        
        bool PushRequestComponent.ICondition.Evaluate() => 
            _cooldown.IsExpired;

        void PushRequestComponent.IAction.Invoke()
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