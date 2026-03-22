using System;
using Game.Presenters;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class UpgradeButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Widget _priceWidget;
        [SerializeField] private GameObject _commonView;
        [SerializeField] private GameObject _maxLevelView;
        
        private PlanetPresenter _planet;
        private IDisposable _disposables;

        public void Initialize(PlanetPresenter planet)
        {
            _planet = planet;
            
            if(_planet == null)
                return;
            
            _button.onClick.AddListener(planet.UnlockOrUpgrade);
            
            DisposableBuilder disposables = Disposable.CreateBuilder();

            planet.Price
                .Subscribe(price => _priceWidget.SetText(price, planet.PriceFormat))
                .AddTo(ref disposables);
            
            planet.IsMaxLevel
                .Subscribe(OnGetMaxLevel)
                .AddTo(ref disposables);
            
            _disposables = disposables.Build();
        }

        public void Dispose()
        {
            if(_planet == null)
                return;
            
            _disposables.Dispose();
            _button.onClick.RemoveListener(_planet.UnlockOrUpgrade);
            _planet = null;
        }

        private void OnGetMaxLevel(bool isMaxLevel)
        {
            _button.interactable = !isMaxLevel;
            _commonView.SetActive(!isMaxLevel);
            _maxLevelView.SetActive(isMaxLevel);

            if (isMaxLevel)
                _priceWidget.Hide();
            else
                _priceWidget.Show();
        }
    }
}