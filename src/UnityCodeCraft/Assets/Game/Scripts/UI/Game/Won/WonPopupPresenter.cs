using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game
{
    public sealed class WonPopupPresenter : NetworkBehaviour
    {
        private SceneNavigator _sceneNavigator;
        private GameCycle _gameCycle;
        private WonPopup _wonPopup;

        [Inject]
        public void Construct(
            WonPopup wonPopup,
            SceneNavigator sceneNavigator,
            GameCycle gameCycle)
        {
            _wonPopup = wonPopup;
            _gameCycle = gameCycle;
            _sceneNavigator = sceneNavigator;
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

        private void OnMenuButtonClick() => _sceneNavigator.GoToMenu();
    }
}