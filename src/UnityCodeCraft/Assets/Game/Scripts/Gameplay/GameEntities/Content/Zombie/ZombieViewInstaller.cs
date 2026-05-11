using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class ZombieViewInstaller : GameEntityInstaller
    {
        private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        
        [SerializeField] private Animator _animator;
        [SerializeField] private ParticleSystem _takeDamageParticles;
        [SerializeField] private ParticleSystem _dieParticles;
        [SerializeField] private ParticleSystem _fallParticles;
        
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _takeDamageSounds;
        [SerializeField] private AudioClip[] _deathSounds;
        [SerializeField] private AudioClip[] _attackSounds;
        [SerializeField] private AudioClip _fallSound;
        
        private readonly DisposableComposite _disposables = new();
        
        public override void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Animator, _animator);
            
            entity
                .GetValue(GameEntityAPI.DeathEvent)
                .Subscribe(() =>
                {
                    _animator.SetTrigger(Death);
                    _dieParticles.Play();
                    PlayDeathSound();
                }).AddTo(_disposables);
            
            entity
                .GetValue(GameEntityAPI.TakeDamageCommand)
                .Subscribe(_ =>
                {
                    _animator.SetTrigger(TakeDamage);
                    _takeDamageParticles.Play();
                    PlayTakeDamageSound();
                })
                .AddTo(_disposables);
            
            entity
                .WhenTick(_ => _animator.SetBool(IsMoving, entity.IsMoving()));
            
            entity
                .GetValue(GameEntityAPI.FireCommand)
                .AddAction(() => _animator.SetTrigger(Attack));
        }
        
        private void OnAnimatorMove()
        {
            transform.parent.position += _animator.deltaPosition;
            transform.parent.rotation *= _animator.deltaRotation;
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        private void PlayTakeDamageSound()
        {
            AudioClip sound = _takeDamageSounds[Random.Range(0, _takeDamageSounds.Length)];
            _audioSource.PlayOneShot(sound);
        }

        private void PlayDeathSound()
        {
            AudioClip sound = _deathSounds[Random.Range(0, _deathSounds.Length)];
            _audioSource.PlayOneShot(sound);
        }

        private void Handle_BodyFall()
        {
            _audioSource.PlayOneShot(_fallSound);
            _fallParticles.Play();
        }
    }
}