using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class CharacterViewInstaller : GameEntityInstaller
    {
        private static readonly int TakeDamage = Animator.StringToHash("TakeDamage");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsAiming = Animator.StringToHash("IsAiming");
        private static readonly int AimX = Animator.StringToHash("AimX");
        private static readonly int AimZ = Animator.StringToHash("AimZ");
        
        [SerializeField] private Animator _animator;

        private readonly DisposableComposite _disposables = new();
        
        private IGameEntity _entity;

        public override void Install(IGameEntity entity)
        {
            _entity = entity;
            
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
                .WhenTick(_ => _animator.SetBool(IsMoving, entity.IsMoving()))
                .AddTo(_disposables);

            entity
                .WhenTick(_ =>
                {
                    Vector3 aimDirection = entity.GetValue(GameEntityAPI.AimDirection).Value;
                    
                    _animator.SetBool(IsAiming, entity.IsAiming());
                    _animator.SetFloat(AimX, aimDirection.x);
                    _animator.SetFloat(AimZ, aimDirection.z);
                });
            
            entity
                .GetValue(GameEntityAPI.MoveSpeedMultiplier)
                .Subscribe(speed => _animator.speed = speed)
                .AddTo(_disposables);
        }

        private void OnAnimatorMove()
        {
            transform.parent.position += _animator.deltaPosition;
            transform.parent.rotation *= _animator.deltaRotation;
        }
    }
}