using SampleGame;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class PlayerAuthoring : MonoBehaviour
    {
        [SerializeField] private TeamType _team;
        [SerializeField] private UnitCardsCatalog _catalog;

        [Header("Money")] 
        [SerializeField] private int _money;
        [SerializeField] private int _income;
        [SerializeField] private float _tickCooldown;

        public class PlayerBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                this.Entity()
                    .With<Player>()
                    .WithEnabled<UnitBuyRequest>(false)
                    .WithEnabled<UnitSpawnRequest>(false)
                    .With(new Team { Value = authoring._team })
                    // Money
                    .With(new Money { Value = authoring._money })
                    .With(new MoneyIncome { Value = authoring._income })
                    .With(new IncomeTickCooldown
                        { Time = authoring._tickCooldown, Duration = authoring._tickCooldown });

                foreach (UnitCardConfig card in authoring._catalog.Cards)
                {
                    Entity config = CreateAdditionalEntity(TransformUsageFlags.None);
                    AddComponent(config, new UnitConfig { Name = card.Name });
                    AddComponent(config, new Team { Value = authoring._team });
                    AddComponent(config, new UnitPrice { Value = card.Price });
                    AddComponent(config, new UnitPrefab { Value = GetEntity(card.Prefab, TransformUsageFlags.Dynamic)});
                }
            }
        }
    }
}