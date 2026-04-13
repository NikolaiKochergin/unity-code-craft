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
        private PushComponent _pushAction;
        private ArcTargetDetectorComponent _targetDetector;
        private DelayComponent _delay;
        private CooldownComponent _cooldown;

        public event Action OnPush;

        private void Awake()
        {
            _pushRequest = GetComponent<PushRequestComponent>();
            _pushAction = GetComponent<PushComponent>();
            _targetDetector = GetComponent<ArcTargetDetectorComponent>();
            _delay = GetComponent<DelayComponent>();
            _cooldown = GetComponent<CooldownComponent>();

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
            Vector2 direction = new(_targetDetector.PushPoint.right.x, _targetDetector.PushPoint.up.y);

            foreach (Transform target in targets)
                _pushAction.Push(target, direction);
        }
    }
}