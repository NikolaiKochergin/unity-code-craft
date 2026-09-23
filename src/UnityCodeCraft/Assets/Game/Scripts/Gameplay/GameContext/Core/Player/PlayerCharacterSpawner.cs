using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerCharacterSpawner : NetworkBehaviour
    {
        [SerializeField] private NetworkPrefabRef _characterPrefab;
        
        private SpawnPointService _spawnPointService;
        private GameFinishController _gameFinishController;

        [Inject]
        public void Construct(
            SpawnPointService spawnPointService,
            GameFinishController gameFinishController)
        {
            _gameFinishController = gameFinishController;
            _spawnPointService = spawnPointService;
        }
        
        public void SpawnCharacter(NetworkObject playerObject)
        {
            PlayerRef player = playerObject.InputAuthority;

            PlayerCharacterProvider characterProvider = playerObject.GetBehaviour<PlayerCharacterProvider>();
            NetworkObject character = characterProvider.Character;
            if (character)
            {
                character.AssignInputAuthority(player);
            }
            else
            {
                Transform spawnPoint = _spawnPointService.GetSpawnPoint(player.AsIndex % _spawnPointService.Count);
                characterProvider.Character = Runner.Spawn(
                    _characterPrefab,
                    spawnPoint.position,
                    spawnPoint.rotation,
                    player
                );
            }
            
            _gameFinishController.AddPlayerTeamUnit(characterProvider.Character);
        }

        public void DespawnCharacter(NetworkObject playerObject)
        {
            PlayerCharacterProvider characterProvider = playerObject.GetBehaviour<PlayerCharacterProvider>();
            NetworkObject character = characterProvider.Character;
            if(!character)
                return;
            
            _gameFinishController.RemovePlayerTeamUnit(character);
            
            characterProvider.Character = null;
            Runner.Despawn(character);
        }
    }
}