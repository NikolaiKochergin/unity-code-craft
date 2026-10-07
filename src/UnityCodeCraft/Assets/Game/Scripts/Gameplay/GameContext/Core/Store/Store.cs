using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public sealed class Store : NetworkBehaviour
    {
        [SerializeField] private int _minePrice = 100;
        [SerializeField] private Mine _minePrefab;
        [SerializeField] private int _turretPrice = 200;
        [SerializeField] private Turret _turretPrefab;
        
        private MoneyStorage _moneyStorage;

        [Inject]
        public void Construct(MoneyStorage moneyStorage) => 
            _moneyStorage = moneyStorage;

        public void TryBuyMineFor(NetworkObject character)
        {
            if(_moneyStorage.Money < _minePrice)
                return;
            
            _moneyStorage.SpendMoney(_minePrice);
            SpawnMineFor(character);
        }

        public void TryBuyTurretFor(NetworkObject character)
        {
            if(_moneyStorage.Money < _turretPrice)
                return;
            
            _moneyStorage.SpendMoney(_turretPrice);
            SpawnTurretFor(character);
        }

        private void SpawnMineFor(NetworkObject character)
        {
            Mine mine = Runner.Spawn(_minePrefab, character.transform.position, Quaternion.identity);
            mine.SetupInstigator(character.Id);
        }

        private void SpawnTurretFor(NetworkObject character)
        {
            Turret turret = Runner.Spawn(_turretPrefab, character.transform.position, Quaternion.identity);
            turret.SetupInstigator(character.Id);
        }
    }
}