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
        private Entity _castleEntity;

        private void Update()
        {
            if (_entityManager == default)
            {
                World world = World.DefaultGameObjectInjectionWorld;

                if (world == null)
                    return;

                _entityManager = world.EntityManager;
            }

            if (_castleEntity == Entity.Null)
            {
                FindCastle();

                if (_castleEntity == Entity.Null)
                    return;
            }

            if (!_entityManager.Exists(_castleEntity))
            {
                _castleEntity = Entity.Null;
                return;
            }

            CurrentHealth currentHealth =
                _entityManager.GetComponentData<CurrentHealth>(_castleEntity);

            MaxHealth maxHealth =
                _entityManager.GetComponentData<MaxHealth>(_castleEntity);
            
            _healthView.HealthText = $"{currentHealth.Value}/{maxHealth.Value}";

            _healthView.HealthProgress =
                maxHealth.Value > 0
                    ? (float)currentHealth.Value / maxHealth.Value
                    : 0f;
        }

        private void FindCastle()
        {
            EntityQuery query = _entityManager.CreateEntityQuery(
                ComponentType.ReadOnly<Castle>(),
                ComponentType.ReadOnly<Team>(),
                ComponentType.ReadOnly<CurrentHealth>(),
                ComponentType.ReadOnly<MaxHealth>());

            using NativeArray<Entity> entities =
                query.ToEntityArray(Allocator.Temp);

            foreach (Entity entity in entities)
            {
                Team team = _entityManager.GetComponentData<Team>(entity);

                if (team.Value == _team)
                {
                    _castleEntity = entity;
                    return;
                }
            }
        }
    }
}