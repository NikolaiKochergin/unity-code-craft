using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Enemy : NetworkBehaviour,
        MoveToTargetComponent.ICondition
    {
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _cooldown = 0.5f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveToTargetComponent _moveToTargetComponent;
        
        private readonly Collider[] _results = new Collider[10];
        private VfxBus _vfxBus;
        private bool _isDestroyed;

        [Networked]
        private TickTimer Timer { get; set; }

        [Inject]
        public void Construct(VfxBus vfxBus)
        {
            _vfxBus = vfxBus;
        }

        public override void Spawned()
        {
            _moveToTargetComponent.SetCondition(this);
        }
        
        public override void FixedUpdateNetwork()
        {
            if(Timer.IsRunning(Runner))
                return;
            
            if(_isDestroyed)
                return;

            if (_healthComponent.IsDead)
            {
                Runner.Despawn(Object);
                _vfxBus.PlayVfx(VfxType.EnemyDie, transform);
                _isDestroyed = true;
                return;
            }
            
            HandleCollision();
        }
        
        private void HandleCollision()
        {
            PhysicsScene physicsScene = Runner.GetPhysicsScene();
            int count = physicsScene
                .OverlapBox(
                    transform.position + _collider.center,
                    _collider.size * 0.2f,
                    _results,
                    _collider.transform.rotation,
                    _layerMask.value,
                    QueryTriggerInteraction.Ignore);

            for (int i = 0; i < count; i++)
            {
                Collider coll = _results[i];
                NetworkObject other = coll.GetComponent<NetworkObject>() ?? 
                                      coll.GetComponentInParent<NetworkObject>();

                if (other && 
                    other.TryGetBehaviour(out HealthComponent health) && health.IsAlive &&
                    other.TryGetBehaviour(out TakeDamageComponent takeDamage))
                {
                    takeDamage.TakeDamage(new TakeDamageArgs(Object.InputAuthority, _damage));

                    if (other.TryGetBehaviour(out Portal _))
                    {
                        GetBehaviour<TakeDamageComponent>()
                            .TakeDamage(new TakeDamageArgs(Object.InputAuthority, _healthComponent.Current));
                        return;
                    }
                    
                    Timer = TickTimer.CreateFromSeconds(
                        Runner,
                        delayInSeconds: _cooldown);
                }
            }
        }

        bool MoveToTargetComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;
    }
}