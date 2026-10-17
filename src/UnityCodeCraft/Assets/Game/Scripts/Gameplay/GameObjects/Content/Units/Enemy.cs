using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class Enemy : NetworkBehaviour,
        MoveComponent.ICondition,
        MoveToTargetComponent.ICondition
    {
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _cooldown = 0.1f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private HealthComponent _healthComponent;
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private MoveToTargetComponent _moveToTargetComponent;
        
        private readonly Collider[] _results = new Collider[10];

        [Networked]
        private TickTimer Timer { get; set; }

        public override void Spawned()
        {
            _moveComponent.SetCondition(this);
            _moveToTargetComponent.SetCondition(this);

            _healthComponent.OnHealthChanged += OnHealthChanged;
        }

        public override void Despawned(NetworkRunner _, bool __) => 
            _healthComponent.OnHealthChanged -= OnHealthChanged;

        private void OnHealthChanged(int _, int __)
        {
            if(_healthComponent.IsDead)
                Runner.Despawn(Object);
        }

        
        //TODO: челики спаунятся в 0 и по этому сразу наносят урон порталу. Либо как то не правильно кастуется бокс
        // нужно как то решить этот вопрос. 
        // Так же возможно куда то вынести эту логику нанесения урона при контакте
        
        public override void FixedUpdateNetwork()
        {
            if(Timer.IsRunning(Runner))
                return;
            
            PhysicsScene physicsScene = Runner.GetPhysicsScene();
            int count = physicsScene
                .OverlapBox(
                    _collider.center,
                    _collider.size / 2,
                    _results,
                    _collider.transform.rotation,
                    _layerMask.value,
                    QueryTriggerInteraction.Ignore);

            for (int i = 0; i < count; i++)
            {
                Collider collider = _results[i];
                NetworkObject other = collider.GetComponent<NetworkObject>() ?? 
                                      collider.GetComponentInParent<NetworkObject>();

                if (other && other.TryGetBehaviour(out HealthComponent health) && health.IsAlive)
                {
                    health.Decrement(_damage);

                    if (other.TryGetBehaviour(out Portal _))
                    {
                        health.Decrement(health.Current);
                        return;
                    }
                    
                    Timer = TickTimer.CreateFromSeconds(
                        Runner,
                        delayInSeconds: _cooldown);
                }
            }
        }


        bool MoveComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;

        bool MoveToTargetComponent.ICondition.IsMet() => 
            _healthComponent.IsAlive;
    }
}