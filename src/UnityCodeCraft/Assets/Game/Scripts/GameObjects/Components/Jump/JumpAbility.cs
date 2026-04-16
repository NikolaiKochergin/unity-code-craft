using System;
using UnityEngine;

namespace Game
{
    public class JumpAbility : MonoBehaviour,
        AbilityRequestComponent.ICondition,
        AbilityRequestComponent.IAction
    {
        [SerializeField] private ActionComponent _jumpComponent;
        
        private AbilityRequestComponent _jumpRequest;
        private GroundedComponent _groundedComponent;
        private CooldownComponent _cooldown;

        public event Action OnJumped;
        
        private void Awake()
        {
            _groundedComponent = GetComponentInParent<GroundedComponent>();
            _jumpRequest = GetComponent<AbilityRequestComponent>();
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
            _jumpComponent.Apply();
            _cooldown.Reset();
            OnJumped?.Invoke();
        }
    }
}