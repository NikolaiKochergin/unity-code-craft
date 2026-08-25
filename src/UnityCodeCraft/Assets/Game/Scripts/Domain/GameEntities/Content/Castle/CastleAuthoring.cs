using SampleGame;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class CastleAuthoring : MonoBehaviour
    {
        [SerializeField] private TeamType _team;
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth;

        public class CastleBaker : Baker<CastleAuthoring>
        {
            public override void Bake(CastleAuthoring authoring) =>
                this.Entity(TransformUsageFlags.Dynamic)
                    .With<Castle>()
                    .With<Unit>()
                    .With(new Team { Value = authoring._team })
                    // Health
                    .With(new CurrentHealth { Value = authoring._currentHealth })
                    .With(new MaxHealth { Value = authoring._maxHealth })
                    // Take Damage
                    .WithBuffer<TakeDamageRequest>()
                    .WithBuffer<TakeDamageEvent>()
                    // Heal
                    .WithBuffer<TakeHealRequest>()
                    .WithBuffer<TakeHealEvent>()
                ;
        }
    }
}