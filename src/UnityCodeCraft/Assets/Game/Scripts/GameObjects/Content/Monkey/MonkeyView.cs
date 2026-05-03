using UnityEngine;

namespace Game
{
    public sealed class MonkeyView : MonoBehaviour
    {
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Death = Animator.StringToHash("Death");
        
        [SerializeField] private GameObject _monkey;
        [SerializeField] private TakeDamageColorComponent _damageComponent;
        [SerializeField] private Animator _animator;
        
        private HealthComponent _healthComponent;
        private GroundedComponent _groundedComponent;
        private JumpComponent _jumpComponent;

        private void Awake()
        {
            _healthComponent = _monkey.GetComponent<HealthComponent>();
            _groundedComponent = _monkey.GetComponent<GroundedComponent>();
            _jumpComponent = _monkey.GetComponent<JumpComponent>();

            _healthComponent.OnHealthChanged += OnHealthChanged;
            _healthComponent.OnDied += OnDied;
            _groundedComponent.OnGrounded += OnGrounded;
            _jumpComponent.OnJump += OnJump;
        }

        private void OnDestroy()
        {
            _healthComponent.OnHealthChanged -= OnHealthChanged;
            _healthComponent.OnDied -= OnDied;
            _groundedComponent.OnGrounded -= OnGrounded;
            _jumpComponent.OnJump -= OnJump;
        }

        private void OnGrounded(bool isGrounded) => 
            _animator.SetBool(IsGrounded, isGrounded);

        private void OnJump() => 
            _animator.SetTrigger(Jump);

        private void OnHealthChanged(float health)
        {
            if(health > 0)
                _damageComponent.TakeDamage();
        }

        private void OnDied() => 
            _animator.SetTrigger(Death);
    }
}