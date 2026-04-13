using System;
using UnityEngine;

namespace Game
{
    public class JumpAbility : MonoBehaviour,
        AbilityRequestComponent.ICondition,
        AbilityRequestComponent.IAction
    {
        private AbilityRequestComponent _jumpRequest;
        private JumpComponent _jumpComponent;
        private GroundedComponent _groundedComponent;
        private DelayComponent _delay;
        private CooldownComponent _cooldown;

        public event Action OnJumped;
        
        private void Awake()
        {
            _groundedComponent = GetComponentInParent<GroundedComponent>();
            _jumpRequest = GetComponent<AbilityRequestComponent>();
            _jumpComponent = GetComponent<JumpComponent>();
            _delay = GetComponent<DelayComponent>();
            _cooldown = GetComponent<CooldownComponent>();

            _jumpRequest.SetCondition(this);
            _jumpRequest.SetAction(this);
        }

        public void Use() => 
            _jumpRequest?.Require();
        
        bool AbilityRequestComponent.ICondition.Evaluate() => 
            _cooldown.IsExpired &&
            _groundedComponent.IsGrounded;

        void AbilityRequestComponent.IAction.Invoke()
        {
            _delay.DelayedInvoke(_jumpComponent.Jump);
            _cooldown.Reset();
            OnJumped?.Invoke();
        }
    }
}