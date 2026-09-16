using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.UI.Menu
{
    public class PlayButtonPresenter : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        
        private GameSessionClient _sessionClient;

        [Inject]
        public void Construct(GameSessionClient sessionClient) => 
            _sessionClient = sessionClient;

        private void OnEnable() => 
            _playButton.onClick.AddListener(OnPlayButtonClicked);

        private void OnDisable() => 
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);

        private void OnPlayButtonClicked() =>
            _sessionClient
                .StartGame()
                .Forget(Debug.LogError);
    }
}