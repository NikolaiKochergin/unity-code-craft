using System;
using System.Collections.Generic;
using Fusion;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public sealed partial class PlayerManager
    {
        [Serializable]
        private struct DisconnectedPlayer
        {
            public PlayerRef Player;
            public TickTimer Timeout;
        }

        [Header("Disconnection")] 
        [SerializeField] private float _disconnectTimeout = 70f;
        
        [ShowInInspector, Sirenix.OdinInspector.ReadOnly]
        private List<DisconnectedPlayer> _disconnectedPlayers = new();
        
        private void PutDisconnectedPlayer(PlayerRef player)
        {
            DisconnectedPlayer disconnectedPlayer = new()
            {
                Player = player,
                Timeout = TickTimer.CreateFromSeconds(Runner, _disconnectTimeout)
            };

            for (int i = _disconnectedPlayers.Count - 1; i >= 0; i--)
            {
                if (_disconnectedPlayers[i].Player == player)
                {
                    _disconnectedPlayers[i] = disconnectedPlayer;
                    return;
                }
            }
            
            _disconnectedPlayers.Add(disconnectedPlayer);
        }

        private void RemoveDisconnectedPlayer(PlayerRef player)
        {
            for (int i = _disconnectedPlayers.Count - 1; i >= 0; i--)
                if (_disconnectedPlayers[i].Player == player)
                    _disconnectedPlayers.RemoveAt(i);
        }

        private void ProcessDisconnectedPlayers()
        {
            for (int i = _disconnectedPlayers.Count - 1; i >= 0; i--)
                if (_disconnectedPlayers[i].Timeout.Expired(Runner))
                    DespawnDisconnectedPlayerAt(i);
        }

        private void DespawnDisconnectedPlayerAt(int index)
        {
            DisconnectedPlayer info = _disconnectedPlayers[index];
            _disconnectedPlayers.RemoveAt(index);
            DespawnPlayerInternal(info.Player);
        }

        public void DespawnPlayerPreventively(PlayerRef player)
        {
            for (int i = _disconnectedPlayers.Count - 1; i >= 0; i--)
                if (_disconnectedPlayers[i].Player == player)
                    _disconnectedPlayers.RemoveAt(i);

            DespawnPlayerInternal(player);
        }
        
        private void DespawnPlayerInternal(PlayerRef player)
        {
            if (!_players.Remove(player, out NetworkObject playerObject) || playerObject == null ||
                !playerObject.IsValid)
                return;

            _characterSpawner.DespawnCharacter(playerObject);
            Runner.Despawn(playerObject);
            Runner.PushHostMigrationSnapshot();
        }
    }
}