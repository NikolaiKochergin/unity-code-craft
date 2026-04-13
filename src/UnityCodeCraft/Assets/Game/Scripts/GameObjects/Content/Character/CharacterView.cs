using UnityEngine;

namespace Game
{
    public class CharacterView : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");

        [SerializeField] private JumpAbility _jumpComponent;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        
        [SerializeField] private AudioClip _takeDamageSound;
        [SerializeField] private AudioClip _jumpSound;
        
        private TakeDamageColorComponent _damageComponent;
        private HealthComponent _healthComponent;
        private MoveRequestComponent _moveComponent;
        private GroundedComponent _groundedComponent;
        private FallingHandleComponent _fallingComponent;

        private void Awake()
        {
            _damageComponent = GetComponent<TakeDamageColorComponent>();
            _healthComponent = GetComponentInParent<HealthComponent>();
            _moveComponent = GetComponentInParent<MoveRequestComponent>();
            _groundedComponent = GetComponentInParent<GroundedComponent>();
            _fallingComponent = GetComponentInParent<FallingHandleComponent>();
            
            _jumpComponent.OnJumped += OnJumped;
            _groundedComponent.OnGrounded += OnGrounded;
            _fallingComponent.OnFalling += OnFalling;
            _healthComponent.OnHealthChanged += OnHealthChanged;
            _healthComponent.OnDied += OnDied;
        }

        private void OnDestroy()
        {
            _jumpComponent.OnJumped -= OnJumped;
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

        private void OnJumped()
        {
            _animator.SetTrigger(Jump);
            _audioSource.PlayOneShot(_jumpSound);
        }

        private void OnHealthChanged(float health)
        {
            if(health <= 0)
                return;
            
            _damageComponent.TakeDamage();
            _audioSource.PlayOneShot(_takeDamageSound);
        }

        private void OnDied() => 
            _animator.SetTrigger(Death);
    }
}