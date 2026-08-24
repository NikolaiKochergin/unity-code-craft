using System.Collections;
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
        private EntityManager _entityManager;
        private EntityQuery _playerQuery;
        private Entity _playerEntity;

        private IEnumerator Start()
        {
            foreach (UnitCardConfig config in _unitCardsCatalog.Cards)
            {
                UnitCardView card = _panelView.GetCard();
                _presenters.Add(new UnitCardVewPresenter(card, config));
            }
            
            while (World.DefaultGameObjectInjectionWorld == null)
                yield return null;
            
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            _playerQuery = _entityManager.CreateEntityQuery(
                ComponentType.ReadOnly<Player>(),
                ComponentType.ReadOnly<Money>(),
                ComponentType.ReadOnly<Team>());

            while (_playerEntity == Entity.Null)
            {
                _playerEntity = FindPlayer();
                yield return null;
            }
        }

        private void Update()
        {
            foreach (UnitCardVewPresenter presenter in _presenters) 
                presenter.Update();
            
            if(_playerEntity == Entity.Null)
                return;

            if (!_entityManager.Exists(_playerEntity))
            {
                _playerEntity = Entity.Null;
                return;
            }
            
            _panelView.EnergyAmountText = _entityManager
                .GetComponentData<Money>(_playerEntity).Value.ToString();
        }

        private Entity FindPlayer()
        {
            using NativeArray<Entity> entities = _playerQuery.ToEntityArray(Allocator.Temp);

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