using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game
{
    public sealed class LosePopupPresenter : NetworkBehaviour
    {
        private GameSessionClient _sessionClient;
        private GameCycle _gameCycle;
        private LosePopup _losePopup;

        [Inject]
        public void Construct(
            LosePopup losePopup,
            GameSessionClient sessionClient, 
            GameCycle gameCycle)
        {
            _losePopup = losePopup;
            _gameCycle = gameCycle;
            _sessionClient = sessionClient;
        }

        public override void Spawned()
        {
            _gameCycle.OnGameFinished += OnGameFinished;
            
            _losePopup.MenuButton.onClick.AddListener(OnMenuButtonClick);
        }

        public override void Despawned(NetworkRunner _, bool __)
        {
            _gameCycle.OnGameFinished -= OnGameFinished;
            
            _losePopup.MenuButton.onClick.RemoveListener(OnMenuButtonClick);
        }

        private void OnGameFinished()
        {
            if(_gameCycle.State == GameState.Lose)
                _losePopup.Show();
        }

        private void OnMenuButtonClick() => StopGameAsync().Forget(Debug.LogError);

        private async UniTask StopGameAsync()
        {
            await _sessionClient.Shutdown(ShutdownReason.Ok);
            await SceneManager.LoadSceneAsync(0).ToUniTask();
        }
    }
}