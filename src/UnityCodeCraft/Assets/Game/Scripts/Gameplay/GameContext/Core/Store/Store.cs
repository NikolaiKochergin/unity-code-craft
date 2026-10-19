using System;
using Fusion;
using UnityEngine;
using Zenject;

namespace Game
{
    public enum PurchaseId
    {
        Mine = 0,
        Turret = 1
    }

    [Serializable]
    public sealed class Purchase
    {
        public PurchaseId Id;
        public int Price;
        public NetworkObjectSupplier Supplier;
    }
    
    public sealed class Store : NetworkBehaviour
    {
        [SerializeField] private Purchase[] _purchases;
        
        private MoneyStorage _moneyStorage;
        
        [Inject]
        public void Construct(MoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
        }

        public void TryBuy(PurchaseId purchaseId, NetworkObject character)
        {
            foreach (Purchase purchase in _purchases)
            {
                if(purchase.Id != purchaseId)
                    continue;
                
                if(_moneyStorage.Money < purchase.Price)
                    return;
                
                _moneyStorage.SpendMoney(purchase.Price);
                purchase.Supplier.Supply(character.InputAuthority, out NetworkObject instance);
                instance.GetBehaviour<NetworkTransform>().Teleport(character.transform.position, character.transform.rotation);
            }
        }
    }
}