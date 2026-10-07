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
            Debug.Log("<color=orange>TryBuyTurretFor()</color>");
        }

        private void SpawnMineFor(NetworkObject character)
        {
            Mine mine = Runner.Spawn(_minePrefab, character.transform.position, Quaternion.identity);
            mine.SetupInstigator(character.Id);
        }
    }
}