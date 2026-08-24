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
                GameObject[] points = authoring._unitSpawnPoints;
                
                for (int i = 0; i < points.Length; i++)
                {
                    Entity pointEntity = GetEntity(points[i], TransformUsageFlags.None);
                    AddComponent(pointEntity, 
                        new EntityName { value = $"{authoring._team.ToString()} {points[i].name}" });
                    AddComponent(pointEntity, new UnitSpawnPoint());
                    AddComponent(pointEntity, new Team { Value = authoring._team });
                    AddComponent(pointEntity, new UnitSpawnPointCount { Value = points.Length });
                }
            }
        }
    }
}