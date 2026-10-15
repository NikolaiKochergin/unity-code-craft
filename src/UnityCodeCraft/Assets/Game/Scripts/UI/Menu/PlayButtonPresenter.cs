using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI.Menu
{
    public class PlayButtonPresenter : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        
        private SceneNavigator _sceneNavigator;

        [Inject]
        public void Construct(SceneNavigator sceneNavigator) =>
            _sceneNavigator = sceneNavigator;

        private void OnEnable() => 
            _playButton.onClick.AddListener(OnPlayButtonClicked);

        private void OnDisable() => 
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);

        private void OnPlayButtonClicked() => _sceneNavigator.GoToGameplay();
    }
}