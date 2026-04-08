using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MoveRequestComponent), typeof(HealthComponent))]
    public class Character : MonoBehaviour,
        MoveRequestComponent.IAction,
        MoveRequestComponent.ICondition,
        JumpRequestComponent.IAction,
        JumpRequestComponent.ICondition
    {
        private HealthComponent _healthComponent;
        private MoveRequestComponent _moveRequestComponent;
        private MoveTransformComponent _moveComponent;
        
        private JumpRequestComponent _jumpRequestComponent;
        private JumpRigidbodyComponent _jumpComponent;
        private GroundedComponent _groundedComponent;

        private void Awake()
        {
            _healthComponent = GetComponent<HealthComponent>();
            _moveRequestComponent = GetComponent<MoveRequestComponent>();
            _moveComponent = GetComponent<MoveTransformComponent>();
            
            _moveRequestComponent.SetAction(this);
            _moveRequestComponent.SetCondition(this);
            
            _jumpRequestComponent = GetComponent<JumpRequestComponent>();
            _jumpComponent = GetComponent<JumpRigidbodyComponent>();
            _groundedComponent = GetComponent<GroundedComponent>();
            
            _jumpRequestComponent.SetAction(this);
            _jumpRequestComponent.SetCondition(this);
        }

        void MoveRequestComponent.IAction.Invoke(Vector2 direction) => 
            _moveComponent.Move(direction);

        bool MoveRequestComponent.ICondition.Evaluate() =>
            _healthComponent.IsAlive;

        void JumpRequestComponent.IAction.Invoke() => 
            _jumpComponent.Jump();

        bool JumpRequestComponent.ICondition.Evaluate() => 
            _healthComponent.IsAlive &&
            _groundedComponent.IsGrounded;
    }
}
