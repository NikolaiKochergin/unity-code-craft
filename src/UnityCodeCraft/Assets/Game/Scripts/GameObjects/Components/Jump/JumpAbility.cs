using System;
using UnityEngine;

namespace Game
{
    public class JumpAbility : MonoBehaviour
    {
        [SerializeField] private JumpComponent _jumpComponent;
        [SerializeField] private AbilityRequestComponent _jumpRequest;
        [SerializeField] private GroundedComponent _groundedComponent;
        [SerializeField] private CooldownComponent _cooldown;
        [SerializeField] private Optional<DelayComponent> _delay;

        public event Action OnJumped;
        
        private void Awake()
        {
            _jumpRequest.SetCondition(() => _cooldown.IsExpired && _groundedComponent.IsGrounded);
            _jumpRequest.SetAction(JumpInvoke);
        }

        public void Jump() => 
            _jumpRequest.Require();

        private void JumpInvoke()
        {
            if(_delay)
                _delay.Value.DelayedInvoke( _jumpComponent.Jump);
            else
                _jumpComponent.Jump();
            
            _cooldown.Reset();
            OnJumped?.Invoke();
        }
    }
}