using System;
using UnityEngine;

namespace Game
{
    public class SnakeView : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Death = Animator.StringToHash("Death");

        [SerializeField] private GameObject _snake;
        [SerializeField] private Animator _animator;
        [SerializeField] private TakeDamageColorComponent _damageComponent;
        
        private HealthComponent _healthComponent;
        private GroundedComponent _groundedComponent;
        private MoveComponent _moveComponent;

        private void Awake()
        {
            _healthComponent = _snake.GetComponent<HealthComponent>();
            _groundedComponent = _snake.GetComponent<GroundedComponent>();
            _moveComponent = _snake.GetComponent<MoveComponent>();

            _healthComponent.OnHealthChanged += OnHealthChanged;
            _healthComponent.OnDied += OnDied;
            _groundedComponent.OnGrounded += OnGrounded;
        }

        private void OnDestroy()
        {
            _healthComponent.OnHealthChanged -= OnHealthChanged;
            _healthComponent.OnDied -= OnDied;
            _groundedComponent.OnGrounded -= OnGrounded;
        }

        private void Update() => 
            OnMove(_moveComponent.IsMoving);

        private void OnGrounded(bool isGrounded) => 
            _animator.SetBool(IsGrounded, isGrounded);
        
        private void OnMove(bool isMoving) => 
            _animator.SetBool(IsMoving, isMoving);

        private void OnHealthChanged(float health)
        {
            if(health > 0)
                _damageComponent.TakeDamage();
        }

        private void OnDied() => 
            _animator.SetTrigger(Death);
    }
}