using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Game
{
    public sealed partial class GameSessionClient
    {
        void INetworkRunnerCallbacks.OnConnectRequest(
            NetworkRunner runner, 
            NetworkRunnerCallbackArgs.ConnectRequest request, 
            byte[] token)
        {
        }

        void INetworkRunnerCallbacks.OnConnectFailed(
            NetworkRunner runner,
            NetAddress remoteAddress, 
            NetConnectFailedReason reason)
        {
            Debug.Log($"Connect failed: {reason}");
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log($"Connected to server!");
        }
    }
}