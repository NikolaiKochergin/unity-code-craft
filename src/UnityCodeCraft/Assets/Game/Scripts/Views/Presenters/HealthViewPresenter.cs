using System.Collections;
using SampleGame;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class HealthViewPresenter : MonoBehaviour
    {
        [SerializeField] private HealthView _healthView;
        [SerializeField] private TeamType _team;

        private EntityManager _entityManager;
        private EntityQuery _castleQuery;
        private Entity _castleEntity;

        private int _lastCurrentHealth = -1;
        private int _lastMaxHealth = -1;

        private IEnumerator Start()
        {
            while (World.DefaultGameObjectInjectionWorld == null)
                yield return null;
            
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            _castleQuery = _entityManager.CreateEntityQuery(
                ComponentType.ReadOnly<Castle>(),
                ComponentType.ReadOnly<Team>(),
                ComponentType.ReadOnly<CurrentHealth>(),
                ComponentType.ReadOnly<MaxHealth>());

            while (_castleEntity == Entity.Null)
            {
                _castleEntity = FindCastle();
                yield return null;
            }
        }

        private void Update()
        {
            if(_castleEntity == Entity.Null)
                return;

            if (!_entityManager.Exists(_castleEntity))
            {
                _castleEntity = Entity.Null;
                return;
            }

            CurrentHealth currentHealth =
                _entityManager.GetComponentData<CurrentHealth>(_castleEntity);

            MaxHealth maxHealth =
                _entityManager.GetComponentData<MaxHealth>(_castleEntity);
            
            if (currentHealth.Value == _lastCurrentHealth &&
                maxHealth.Value == _lastMaxHealth)
                return;
            
            _healthView.HealthText = $"{currentHealth.Value}/{maxHealth.Value}";

            _healthView.HealthProgress =
                maxHealth.Value > 0
                    ? (float)currentHealth.Value / maxHealth.Value
                    : 0f;
        }

        private Entity FindCastle()
        {
            using NativeArray<Entity> entities =
                _castleQuery.ToEntityArray(Allocator.Temp);

            foreach (Entity entity in entities)
            {
                Team team = _entityManager.GetComponentData<Team>(entity);
                if (team.Value == _team)
                    return entity;
            }
            return Entity.Null;
        }
    }
}