using UnityEngine;

namespace Game.Scripts.GameObjects.Content.Monkey
{
    public class MonkeyView : MonoBehaviour
    {
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");

        [SerializeField] private JumpAbility _jumpAbility;
        [SerializeField] private GroundedComponent _groundedComponent;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private FallingComponent _fallingComponent;
        
        [SerializeField] private TakeDamageColorComponent _damageComponent;
        [SerializeField] private Animator _animator;

        private void OnEnable()
        {
            _jumpAbility.OnJumped += OnJumped;
            _groundedComponent.OnGrounded += OnGrounded;
            _fallingComponent.OnFalling += OnFalling;
            _healthComponent.OnHealthChanged += OnHealthChanged;
            _healthComponent.OnDied += OnDied;
        }

        private void OnDisable()
        {
            _jumpAbility.OnJumped -= OnJumped;
            _groundedComponent.OnGrounded -= OnGrounded;
            _fallingComponent.OnFalling -= OnFalling;
            _healthComponent.OnHealthChanged -= OnHealthChanged;
            _healthComponent.OnDied -= OnDied;
        }

        private void OnGrounded(bool isGrounded) => 
            _animator.SetBool(IsGrounded, _groundedComponent.IsGrounded);

        private void OnFalling(bool isFalling) => 
            _animator.SetBool(IsFalling, isFalling);

        private void OnJumped() => 
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