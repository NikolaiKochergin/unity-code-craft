using System.Collections.Generic;
using SampleGame;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public sealed class PanelViewPresenter : MonoBehaviour
    {
        [SerializeField] private TeamType _team;
        [SerializeField] private PanelView _panelView;
        [SerializeField] private UnitCardsCatalog _unitCardsCatalog;

        private readonly List<UnitCardVewPresenter> _presenters = new();
        private World _world;
        private Entity _playerEntity;
        private EntityManager _entityManager;

        private void Start()
        {
            foreach (UnitCardConfig config in _unitCardsCatalog.Cards)
            {
                UnitCardView card = _panelView.GetCard();
                _presenters.Add(new UnitCardVewPresenter(card, config));
            }
        }

        private void Update()
        {
            foreach (UnitCardVewPresenter presenter in _presenters) 
                presenter.Update();
            
            if (_entityManager == default)
            {
                World world = World.DefaultGameObjectInjectionWorld;

                if (world == null)
                    return;

                _entityManager = world.EntityManager;
            }

            if (_playerEntity == Entity.Null)
            {
                FindPlayerEntity();

                if (_playerEntity == Entity.Null)
                    return;
            }

            if (!_entityManager.Exists(_playerEntity))
            {
                _playerEntity = Entity.Null;
                return;
            }
            
            _panelView.EnergyAmountText = _entityManager.GetComponentData<Money>(_playerEntity).Value.ToString();
        }

        private void FindPlayerEntity()
        {
            EntityQuery query = _entityManager.CreateEntityQuery(
                ComponentType.ReadOnly<Player>(),
                ComponentType.ReadOnly<Money>(),
                ComponentType.ReadOnly<Team>()
            );

            using NativeArray<Entity> entities = query.ToEntityArray(Allocator.Temp);

            foreach (Entity entity in entities)
            {
                Team team = _entityManager.GetComponentData<Team>(entity);
                if (team.Value == _team)
                {
                    _playerEntity = entity;
                    return;
                }
            }
        }
    }
}