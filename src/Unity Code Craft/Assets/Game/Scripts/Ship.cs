using UnityEngine;

namespace Game
{
    // +
    public sealed class Ship : MonoBehaviour, IDamageable
    {
        [SerializeField] private ShipConfig config;

        [field: SerializeField] public HealthComponent Health { get; private set; }
        [field: SerializeField] public MoveComponent Mover { get; private set; }
        [field: SerializeField] public AttackComponent Attack { get; private set; }
        
        private void Awake()
        {
            Health.Setup(config.Health.Max);
            Mover.Setup(config.Move.Speed);
            Attack.Setup(config.Attack, () => Health.IsAlive);
        }

        private void FixedUpdate() => 
            Mover.Update(Time.fixedDeltaTime);
    }
}