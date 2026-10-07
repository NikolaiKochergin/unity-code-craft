using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game
{
    public sealed class WonPopupPresenter : NetworkBehaviour
    {
        private GameSessionClient _sessionClient;
        private GameCycle _gameCycle;
        private WonPopup _wonPopup;

        [Inject]
        public void Construct(
            WonPopup wonPopup,
            GameSessionClient sessionClient, 
            GameCycle gameCycle)
        {
            _wonPopup = wonPopup;
            _gameCycle = gameCycle;
            _sessionClient = sessionClient;
        }

        public override void Spawned()
        {
            _gameCycle.OnGameFinished += OnGameFinished;
            
            _wonPopup.MenuButton.onClick.AddListener(OnMenuButtonClick);
        }

        public override void Despawned(NetworkRunner _, bool __)
        {
            _gameCycle.OnGameFinished -= OnGameFinished;
            
            _wonPopup.MenuButton.onClick.RemoveListener(OnMenuButtonClick);
        }

        private void OnGameFinished()
        {
            if(_gameCycle.State == GameState.Win)
                _wonPopup.Show();
        }

        private void OnMenuButtonClick() => StopGameAsync().Forget(Debug.LogError);

        private async UniTask StopGameAsync()
        {
            await _sessionClient.Shutdown(ShutdownReason.Ok);
            await SceneManager.LoadSceneAsync(0).ToUniTask();
        }
    }
}