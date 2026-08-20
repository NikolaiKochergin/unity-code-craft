using SampleGame;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class UnitSpawnPointAuthoring : MonoBehaviour
    {
        [SerializeField] private TeamType _team;

        public class UnitSpawnPointsBaker : Baker<UnitSpawnPointAuthoring>
        {
            public override void Bake(UnitSpawnPointAuthoring authoring) =>
                this.Entity()
                    .With<UnitSpawnPoint>()
                    .With(new Team{ Value = authoring._team });
        }
    }
}