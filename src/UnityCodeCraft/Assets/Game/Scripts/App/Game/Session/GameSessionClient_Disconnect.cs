using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Game
{
    public sealed partial class GameSessionClient
    {
        void INetworkRunnerCallbacks.OnDisconnectedFromServer(
            NetworkRunner runner, 
            NetDisconnectReason reason)
        {
            Debug.Log($"Disconnected from server. Reason {reason}");
        }
    }
}