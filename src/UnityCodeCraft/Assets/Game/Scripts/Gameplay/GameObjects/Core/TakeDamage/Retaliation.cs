using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class Retaliation : NetworkBehaviour
    {
        [SerializeField] private TakeDamageComponent _takeDamageComponent;

        public override void Spawned()
        {
            _takeDamageComponent.OnDamageTaken += OnDamageTaken;
        }

        public override void Despawned(NetworkRunner _, bool __)
        {
            _takeDamageComponent.OnDamageTaken -= OnDamageTaken;
        }

        private void OnDamageTaken(TakeDamageArgs args)
        {
            if(!Runner.TryFindObject(args.Instigator, out NetworkObject instigator))
                return;
            
            if(instigator.TryGetBehaviour(out HealthComponent health) &&
               instigator.TryGetBehaviour(out TakeDamageComponent takeDamage))
                takeDamage.TakeDamage(new TakeDamageArgs(Object.Id, health.Current));
        }
    }
}