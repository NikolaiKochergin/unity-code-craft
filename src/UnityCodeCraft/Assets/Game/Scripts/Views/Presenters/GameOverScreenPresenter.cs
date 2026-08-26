using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class GameOverScreenPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject _winScreen;
        [SerializeField] private GameObject _loseScreen;
        
        private EntityManager _entityManager;
        private EntityQuery _gameResultQuery;

        private bool _isInitialized;

        private IEnumerator Start()
        {
            while (World.DefaultGameObjectInjectionWorld == null)
                yield return null;
            
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            _gameResultQuery = _entityManager.CreateEntityQuery(
                ComponentType.ReadOnly<GameResult>());
            
            _isInitialized = true;
        }

        private void Update()
        {
            if(!_isInitialized)
                return;
            
            if(_gameResultQuery.IsEmpty)
                return;
            
            GameResult result = _gameResultQuery.GetSingleton<GameResult>();

            switch (result.Value)
            {
                case GameResultType.Win:
                    _winScreen.SetActive(true);
                    break;
                case GameResultType.Lose:
                    _loseScreen.SetActive(true);
                    break;
            }
        }
    }
}