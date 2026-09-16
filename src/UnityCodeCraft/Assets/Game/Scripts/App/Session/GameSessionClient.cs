using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Game
{
    public sealed partial class GameSessionClient : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private NetworkRunner _runnerPrefab;
        [SerializeField] private SceneRef _gameScene;
        
        private NetworkRunner _runner;

        public async UniTask<bool> StartGame(
            GameMode gameMode = GameMode.AutoHostOrClient,
            string sessionName = null,
            Dictionary<string, string> sessionProperties = null,
            string lobbyID = null,
            int playerCount = 2)
        {
            await Shutdown(ShutdownReason.Ok);
            _runner = CreateRunner();

            StartGameResult result = await _runner.StartGame(new StartGameArgs
            {
                GameMode = gameMode,
                SessionName = sessionName,
                Scene = _gameScene,
                PlayerCount = playerCount,
                SessionProperties = sessionProperties?.ToDictionary(
                    pair => pair.Key,
                    pair => (SessionProperty) pair.Value
                ),
                CustomLobbyName = lobbyID
            }).AsUniTask();

            bool successful = result.Ok;
            if (successful)
                await _runner.PushHostMigrationSnapshot();

            return successful;
        }


        public async UniTask Shutdown(ShutdownReason reason)
        {
            if (_runner != null)
            {
                await _runner.Shutdown(shutdownReason: reason, destroyGameObject: true);
                _runner = null;
            }
        }

        private NetworkRunner CreateRunner()
        {
            NetworkRunner runner = Instantiate(_runnerPrefab);
            runner.ProvideInput = true;
            runner.AddCallbacks(this);
            return runner;
        }
    }
}