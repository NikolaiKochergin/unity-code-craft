using SampleGame;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class PlayerAuthoring : MonoBehaviour
    {
        [SerializeField] private TeamType _team;

        [Header("Money")] 
        [SerializeField] private int _money;
        [SerializeField] private int _income;
        [SerializeField] private float _tickCooldown;

        public class PlayerBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring) =>
                this.Entity()
                    .With<Player>()
                    .With(new Team { Value = authoring._team })
                    // Money
                    .With(new Money { Value = authoring._money})
                    .With(new MoneyIncome { Value = authoring._income})
                    .With(new IncomeTickCooldown { Time = authoring._tickCooldown, Duration = authoring._tickCooldown})
                ;
        }
    }
}