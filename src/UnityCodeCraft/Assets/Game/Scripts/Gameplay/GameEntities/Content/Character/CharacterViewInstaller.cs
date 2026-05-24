using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class CharacterViewInstaller : GameEntityInstaller
    {
        private const string FootStepEvt = "step";
        private const string DeathEvt = "death";
        
        private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsAiming = Animator.StringToHash("IsAiming");
        private static readonly int AimX = Animator.StringToHash("AimX");
        private static readonly int AimZ = Animator.StringToHash("AimZ");
        
        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationEvents _animationEvents;
        
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _death;
        [SerializeField] private AudioClip[] _moveSteps;

        private readonly DisposableComposite _disposables = new();

        public override void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Animator, _animator);
            
            entity
                .GetValue(GameEntityAPI.DeathEvent)
                .Subscribe(() =>
                {
                    _animator.SetTrigger(Death);
                }).AddTo(_disposables);
            
            entity
                .GetValue(GameEntityAPI.TakeDamageCommand)
                .Subscribe(_ => _animator.SetTrigger(TakeDamage))
                .AddTo(_disposables);

            entity
                .WhenTick(_ => _animator.SetBool(IsMoving, entity.IsMoving()));

            entity
                .WhenTick(_ =>
                {
                    Vector3 aimDirection = entity.GetValue(GameEntityAPI.AimDirection).Value;
                    
                    _animator.SetBool(IsAiming, entity.IsAiming());
                    _animator.SetFloat(AimX, aimDirection.x);
                    _animator.SetFloat(AimZ, aimDirection.z);
                });
            
            entity
                .GetValue(GameEntityAPI.FireCommand)
                .AddAction(() => _animator.SetTrigger(Attack));
            
            _animationEvents.Subscribe(FootStepEvt, PlayMoveStep);
            _animationEvents.Subscribe(DeathEvt, PlayDeathSound);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
            
            _animationEvents.Unsubscribe(FootStepEvt, PlayMoveStep);
            _animationEvents.Unsubscribe(DeathEvt, PlayDeathSound);
        }
        
        private void PlayMoveStep() => 
            _audioSource.PlayOneShot(_moveSteps[Random.Range(0, _moveSteps.Length)]);
        
        private void PlayDeathSound() =>
            _audioSource.PlayOneShot(_death);
    }
}