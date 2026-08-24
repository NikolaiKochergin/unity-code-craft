using SampleGame;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class UnitSpawnPointsAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject[] _unitSpawnPoints;
        [SerializeField] private TeamType _team;

        public class UnitSpawnPointsBaker : Baker<UnitSpawnPointsAuthoring>
        {
            public override void Bake(UnitSpawnPointsAuthoring authoring)
            {
                foreach (var point in authoring._unitSpawnPoints)
                {
                    Entity pointEntity = CreateAdditionalEntity(TransformUsageFlags.None);
                    AddComponent(pointEntity, new EntityName { value = $"{authoring._team.ToString()} {point.name}" });
                    AddComponent(pointEntity, new UnitSpawnPoint());
                    AddComponent(pointEntity, new Team { Value = authoring._team });
                    AddComponent(pointEntity, new UnitSpawnPointCount { Value = authoring._unitSpawnPoints.Length });
                    AddComponent(pointEntity, new UnitSpawnPointPosition { Value = point.transform.position });
                }
            }
        }
    }
}