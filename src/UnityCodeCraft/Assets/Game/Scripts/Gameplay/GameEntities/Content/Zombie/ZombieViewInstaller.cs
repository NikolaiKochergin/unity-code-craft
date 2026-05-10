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
    }
}