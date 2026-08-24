using System;
using SampleGame;
using Unity.Entities;

namespace Game
{
    public class UnitCardVewPresenter : IDisposable
    {
        private readonly UnitCardView _card;
        private readonly UnitCardConfig _config;
        private readonly Entity _playerEntity;
        
        private int _lastMoneyAmount = -1;
        private EntityManager _entityManager;

        public UnitCardVewPresenter(
            UnitCardView card, 
            UnitCardConfig config, 
            Entity playerEntity,
            EntityManager entityManager)
        {
            _entityManager = entityManager;
            _playerEntity = playerEntity;
            _card = card;
            _config = config;
            Setup();
        }

        private void Setup()
        {
            _card.SetIcon(_config.Icon);
            _card.SetName(_config.Name);
            _card.OnClicked += OnCardClicked;
        }
        
        public void Dispose()
        {
            _card.OnClicked -= OnCardClicked;
        }

        public void Update(int moneyAmount)
        {
            if(_lastMoneyAmount == moneyAmount)
                return;
            
            _lastMoneyAmount = moneyAmount;
            
            _card.SetProgress((float)moneyAmount/_config.Price);
            _card.SetProgressCaption($"{moneyAmount}/{_config.Price}");
        }

        private void OnCardClicked()
        {
            _entityManager.SetComponentData(_playerEntity, new UnitBuyRequest { PrefabName = _config.Name });
            _entityManager.SetComponentEnabled<UnitBuyRequest>(_playerEntity, true);
        }
    }
}