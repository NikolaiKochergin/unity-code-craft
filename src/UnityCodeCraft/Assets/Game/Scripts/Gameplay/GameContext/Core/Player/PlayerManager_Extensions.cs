using System.Collections.Generic;
using Fusion;

namespace Game
{
    public sealed partial class PlayerManager
    {
        public IEnumerable<PlayerRef> ActivePlayers => Runner.ActivePlayers;

        public bool TryGetPlayerBehaviour<T>(NetworkObject obj, out T result) where T : Behaviour =>
            TryGetPlayerBehaviour(obj.InputAuthority, out result);

        public bool HasPlayerBehaviour<T>(NetworkObject obj) where T : Behaviour =>
            TryGetPlayerBehaviour(obj.InputAuthority, out T _);

        public T GetPlayerBehaviour<T>(NetworkObject obj) where T : Behaviour =>
            GetPlayerBehaviour<T>(obj.InputAuthority);

        public T GetPlayerBehaviour<T>(PlayerRef player) where T : Behaviour =>
            _players[player].GetBehaviour<T>();

        public bool TryGetLocalPlayerBehaviour<T>(out T result) where T : Behaviour =>
            TryGetPlayerBehaviour(this.Runner.LocalPlayer, out result);

        public bool TryGetPlayerBehaviour<T>(PlayerRef player, out T result) where T : Behaviour
        {
            result = null;
            return _players.TryGet(player, out NetworkObject playerObject) &&
                   playerObject.TryGetBehaviour(out result);
        }
    }
}