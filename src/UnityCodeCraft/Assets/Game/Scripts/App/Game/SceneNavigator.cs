using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public sealed class SceneNavigator
    {
        private readonly GameSessionClient _sessionClient;

        public SceneNavigator(GameSessionClient sessionClient) => 
            _sessionClient = sessionClient;

        public void GoToMenu() => GoToMenuAsync().Forget(Debug.LogError);
        public void GoToGameplay() => GoToGameplayAsync().Forget(Debug.LogError);

        public async UniTask GoToMenuAsync()
        {
            await _sessionClient.Shutdown(ShutdownReason.Ok);
            await SceneManager.LoadSceneAsync(0).ToUniTask();
        }

        public async UniTask GoToGameplayAsync()
        {
            await _sessionClient.StartGame();
        }
    }
}