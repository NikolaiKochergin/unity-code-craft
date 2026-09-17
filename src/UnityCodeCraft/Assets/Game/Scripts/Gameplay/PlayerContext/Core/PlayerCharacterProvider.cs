using Fusion;

namespace Game
{
    public sealed class PlayerCharacterProvider : NetworkBehaviour
    {
        [Networked]
        public NetworkObject Character { get; set; }
    }
}