using System.Collections;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed partial class PlayerManager : NetworkBehaviour, IPlayerLeft, IPlayerJoined,
        IEnumerable<KeyValuePair<PlayerRef, NetworkObject>>
    {
        [SerializeField] private NetworkObject _playerPrefab;
        
        [Networked, Capacity(16)]
        private NetworkDictionary<PlayerRef, NetworkObject> _players { get; }
        
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        public int PlayerCount => StateBufferIsValid ? _players.Count : 0;

        [Inject]
        private PlayerCharacterSpawner _characterSpawner;
        
        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if(!HasStateAuthority)
                return;

            RemoveDisconnectedPlayer(player);

            if (_players.TryGet(player, out NetworkObject playerObject))
            {
                playerObject.AssignInputAuthority(player);
            }
            else
            {
                playerObject = Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, player);
                _players.Add(player, playerObject);
            }
            
            Runner.SetPlayerObject(player, playerObject);

            _characterSpawner.SpawnCharacter(playerObject);
            Runner.PushHostMigrationSnapshot();
        }

        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if(!Runner.IsServer)
                return;

            if (Runner.LocalPlayer != player)
                PutDisconnectedPlayer(player);
        }

        public override void FixedUpdateNetwork()
        {
            if(Runner.IsServer)
                ProcessDisconnectedPlayers();
        }

        public IEnumerator<KeyValuePair<PlayerRef, NetworkObject>> GetEnumerator() => _players.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}