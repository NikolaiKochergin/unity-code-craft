using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Spider
{
    public class SpiderView : MonoBehaviour
    {
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        
        [SerializeField] private Animator _animator;
        [SerializeField] private TakeDamageColorComponent _damageComponent;
        
        [Space]
        [SerializeField] private MoveAbility _moveComponent;
        [SerializeField] private FallingComponent _fallingComponent;
        
        private HealthComponent _healthComponent;

        private GroundedComponent _groundedComponent;
        
        private void Awake()
        {
            _damageComponent = GetComponent<TakeDamageColorComponent>();
            _healthComponent = GetComponentInParent<HealthComponent>();
            _groundedComponent = GetComponentInParent<GroundedComponent>();
            
            _groundedComponent.OnGrounded += OnGrounded;
            _fallingComponent.OnFalling += OnFalling;
            _healthComponent.OnHealthChanged += OnHealthChanged;
            _healthComponent.OnDied += OnDied;
        }

        private void OnDestroy()
        {
            _groundedComponent.OnGrounded += OnGrounded;
            _fallingComponent.OnFalling += OnFalling;
            _healthComponent.OnHealthChanged -= OnHealthChanged;
            _healthComponent.OnDied -= OnDied;
        }
        
        private void OnGrounded(bool isGrounded) => 
            _animator.SetBool(IsGrounded, _groundedComponent.IsGrounded);

        private void OnFalling(bool isFalling) => 
            _animator.SetBool(IsFalling, isFalling);

        private void Update() => 
            _animator.SetBool(IsMoving, _moveComponent.IsMoving);

        private void OnHealthChanged(float health) => 
            _damageComponent.TakeDamage();

        private void OnDied() => 
            _animator.SetTrigger(Death);
    }
}