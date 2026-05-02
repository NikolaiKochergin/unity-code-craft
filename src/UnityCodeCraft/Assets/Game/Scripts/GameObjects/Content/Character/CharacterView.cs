using UnityEngine;

namespace Game
{
    public class CharacterView : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Death = Animator.StringToHash("Death");
        
        private static readonly int BlowForward = Animator.StringToHash("BlowForward");
        private static readonly int BlowUp = Animator.StringToHash("BlowUp");

        [SerializeField] private GameObject _character;
        [SerializeField] private GameObject _pushAttack;
        [SerializeField] private GameObject _tossAttack;
        [SerializeField] private TakeDamageColorComponent _damageComponent;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _takeDamageSound;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _pushAudioClip;
        [SerializeField] private ParticleSystem _pushVFX;
        [SerializeField] private AudioClip _tossAudioClip;
        [SerializeField] private ParticleSystem _tossVFX;
        
        private GroundedComponent _groundedComponent;
        private JumpComponent _jumpComponent;
        private HealthComponent _healthComponent;
        private MoveComponent _moveComponent;
        private ForceComponent _pushComponent;
        private ForceComponent _tossComponent;

        private void Awake()
        {
            _groundedComponent = _character.GetComponent<GroundedComponent>();
            _jumpComponent = _character.GetComponent<JumpComponent>();
            _healthComponent = _character.GetComponent<HealthComponent>();
            _moveComponent = _character.GetComponent<MoveComponent>();
            _pushComponent = _pushAttack.GetComponent<ForceComponent>();
            _tossComponent = _tossAttack.GetComponent<ForceComponent>();

            _groundedComponent.OnGrounded += OnGrounded;
            _jumpComponent.OnJump += OnJump;
            _healthComponent.OnHealthChanged += OnHealthChanged;
            _healthComponent.OnDied += OnDied;
            _pushComponent.OnFire += OnPush;
            _tossComponent.OnFire += OnToss;
        }

        private void OnDestroy()
        {
            _groundedComponent.OnGrounded -= OnGrounded;
            _jumpComponent.OnJump -= OnJump;
            _healthComponent.OnHealthChanged -= OnHealthChanged;
            _healthComponent.OnDied -= OnDied;
            _pushComponent.OnFire -= OnPush;
            _tossComponent.OnFire -= OnToss;
        }

        private void Update() => 
            OnMove(_moveComponent.IsMoving);

        private void OnGrounded(bool isGrounded) => 
            _animator.SetBool(IsGrounded, isGrounded);

        private void OnMove(bool isMoving) => 
            _animator.SetBool(IsMoving, isMoving);

        private void OnJump()
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
        
        private void OnPush()
        {
            _animator.SetTrigger(BlowForward);
            _pushVFX.Play();
            _audioSource.PlayOneShot(_pushAudioClip);
        }
        
        private void OnToss()
        {
            _animator.SetTrigger(BlowUp);
            _tossVFX.Play();
            _audioSource.PlayOneShot(_tossAudioClip);
        }
    }
}