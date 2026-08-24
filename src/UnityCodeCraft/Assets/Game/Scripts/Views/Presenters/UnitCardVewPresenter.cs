using System;
using SampleGame;
using UnityEngine;

namespace Game
{
    public class UnitCardVewPresenter : IDisposable
    {
        private readonly UnitCardView _card;
        private readonly UnitCardConfig _config;
        
        private int _lastMoneyAmount = -1;

        public UnitCardVewPresenter(UnitCardView card, UnitCardConfig config)
        {
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
            Debug.Log("OnCardClicked");
        }
    }
}