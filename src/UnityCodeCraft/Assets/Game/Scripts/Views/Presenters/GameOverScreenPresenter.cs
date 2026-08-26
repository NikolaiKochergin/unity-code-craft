using System;
using System.Collections;
using SampleGame;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class GameOverScreenPresenter : MonoBehaviour
    {
        
        
        private EntityManager _entityManager;
        private EntityQuery _playerQuery;
        private Entity _playerEntity;
        private Entity _opponentEntity;

        private IEnumerator Start()
        {
            while (World.DefaultGameObjectInjectionWorld == null)
                yield return null;
            
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            _playerQuery = _entityManager.CreateEntityQuery(
                ComponentType.ReadOnly<Player>(),
                ComponentType.ReadOnly<Team>());

            while (_playerEntity == Entity.Null)
            {
                _playerEntity = FindPlayer(TeamType.Blue);
                yield return null;
            }
            
            while (_opponentEntity == Entity.Null)
            {
                _opponentEntity = FindPlayer(TeamType.Red);
                yield return null;
            }
        }

        private void Update()
        {
            if (!_entityManager.Exists(_opponentEntity))
            {
                Debug.LogWarning("<color=green> Win");
                enabled = false;
                return;
            }

            if (_entityManager.Exists(_playerEntity)) 
                return;
            
            Debug.LogWarning("<color=green> Lose");
            enabled = false;
        }

        private Entity FindPlayer(TeamType team)
        {
            using NativeArray<Entity> entities = _playerQuery.ToEntityArray(Allocator.Temp);

            foreach (Entity entity in entities)
            {
                Team playerTeam = _entityManager.GetComponentData<Team>(entity);
                if (playerTeam.Value == team)
                    return entity;
            }
            return Entity.Null;
        }
    }
}