using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public class PlayerCharacterSpawner : NetworkBehaviour
    {
        [SerializeField] private NetworkPrefabRef _characterPrefab;
        
        private SpawnPointService _spawnPointService;

        [Inject]
        public void Construct(SpawnPointService spawnPointService)
        {
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
                
                // TODO: дописать тут корректное назначение индекса точки спауна
                Transform spawnPoint = _spawnPointService.GetSpawnPoint(Runner.LocalPlayer.AsIndex);
                characterProvider.Character = Runner.Spawn(
                    _characterPrefab,
                    spawnPoint.position,
                    spawnPoint.rotation,
                    player
                );
            }
        }

        public void DespawnCharacter(NetworkObject playerObject)
        {
            PlayerCharacterProvider characterProvider = playerObject.GetBehaviour<PlayerCharacterProvider>();
            NetworkObject character = characterProvider.Character;
            if(!character)
                return;
            
            characterProvider.Character = null;
            Runner.Despawn(character);
        }
    }
}