using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game
{
    public sealed class LosePopupPresenter : NetworkBehaviour
    {
        private SceneNavigator _sceneNavigator;
        private GameCycle _gameCycle;
        private LosePopup _losePopup;

        [Inject]
        public void Construct(
            LosePopup losePopup,
            SceneNavigator sceneNavigator,
            GameCycle gameCycle)
        {
            _losePopup = losePopup;
            _gameCycle = gameCycle;
            _sceneNavigator = sceneNavigator;
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

        private void OnMenuButtonClick() => _sceneNavigator.GoToMenu();
    }
}