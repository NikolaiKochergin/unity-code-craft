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
                Entity entity = this.Entity()
                    .With(new Team { Value = authoring._team });

                DynamicBuffer<UnitSpawnPoint> spawnPoint = AddBuffer<UnitSpawnPoint>(entity);
                
                foreach (GameObject point in authoring._unitSpawnPoints) 
                    spawnPoint.Add(new UnitSpawnPoint { Position = point.transform.position });
            }
        }
    }
}