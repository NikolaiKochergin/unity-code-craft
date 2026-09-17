using Fusion;
using UnityEngine;

namespace Game
{
    public struct InputData : INetworkInput
    {
        public Vector2 moveDirection;
        public NetworkButtons buttons;
    }
}