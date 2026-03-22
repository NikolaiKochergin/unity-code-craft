using System;
using Game.Presenters;
using Modules.UI;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlanetView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _lock;
        [SerializeField] private SmartButton _button;
        [SerializeField] private IncomeView _incomeView;
        [SerializeField] private Widget _priceWidget;
        
        private PlanetPresenter _presenter;
        private IDisposable _disposables;

        public void Show(PlanetPresenter presenter)
        {
            _presenter = presenter;
            
            DisposableBuilder disposables = Disposable.CreateBuilder();
            
            _presenter.Icon
                .Subscribe(sprite => _icon.sprite = sprite)
                .AddTo(ref disposables);
            
            _presenter.IsUnlocked
                .Subscribe(OnUnlockChanged)
                .AddTo(ref disposables);

            _disposables = disposables.Build();

            _button.OnClick += OnButtonClick;
            _button.OnHold += _presenter.OnHold;
        }

        public void Hide()
        {
            _disposables?.Dispose();
            
            _button.OnClick -= OnButtonClick;
            _button.OnHold -= _presenter.OnHold;

            _incomeView.Hide();
        }

        private void OnButtonClick()
        {
            if (_presenter.IsUnlocked.CurrentValue)
                _incomeView.TryGatherIncome();
            else
                _presenter.Unlock();
        }

        private void OnUnlockChanged(bool isUnlocked)
        {
            _lock.SetActive(!isUnlocked);
            if (isUnlocked)
            {
                _incomeView.Show(_presenter.Income);
                _priceWidget.Hide();
            }
            else
            {
                _incomeView.Hide();
                _priceWidget.SetText(_presenter.Price.CurrentValue, _presenter.PriceFormat);
                _priceWidget.Show();
            }
        }
    }
}