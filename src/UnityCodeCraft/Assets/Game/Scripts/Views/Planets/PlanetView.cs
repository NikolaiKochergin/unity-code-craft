using System;
using Game.Presenters;
using Modules.UI;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Views
{
    public class PlanetView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _lock;
        [SerializeField] private SmartButton _button;
        [SerializeField] private Widget _priceWidget;

        [SerializeField] private GameObject _coin;
        [SerializeField] private Image _timerSlider;
        [SerializeField] private Widget _timer;
        
        private MoneyParticleView _moneyParticle;
        
        private PlanetPresenter _presenter;
        private IDisposable _disposables;
        
        [Inject]
        public void Construct(MoneyParticleView moneyParticle) => 
            _moneyParticle = moneyParticle;

        public void Show(PlanetPresenter presenter)
        {
            _presenter = presenter;
            
            DisposableBuilder disposables = Disposable.CreateBuilder();
            
            _presenter.Icon
                .Subscribe(sprite => _icon.sprite = sprite)
                .AddTo(ref disposables);
            
            _presenter.Price
                .Subscribe(value => _priceWidget.SetText(value, _presenter.PriceFormat))
                .AddTo(ref disposables);
            
            _presenter.IsUnlocked
                .Subscribe(OnUnlockChanged)
                .AddTo(ref disposables);
            
            _presenter.RemainTime
                .Subscribe(OnIncomeTimeChanged)
                .AddTo(ref disposables);
            
            _presenter.IncomeProgress
                .Subscribe(OnIncomeProgressChanged)
                .AddTo(ref disposables);
            
            _presenter.IsIncomeReady
                .Where(_ => _presenter.IsUnlocked.CurrentValue)
                .Subscribe(OnIncomeReady)
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
        }

        private void OnButtonClick()
        {
            if (_presenter.IsUnlocked.CurrentValue)
            {
                if (!_presenter.IsIncomeReady.CurrentValue) 
                    return;
                
                _coin.SetActive(false);
                _moneyParticle.Emit(_coin.transform.position, _presenter.GatherIncome);
            }
            else
            {
                _presenter.Unlock();
            }
        }

        private void OnUnlockChanged(bool isUnlocked)
        {
            _lock.SetActive(!isUnlocked);
            if (isUnlocked)
            {
                _priceWidget.Hide();
                OnIncomeReady(false);
            }
            else
            {
                _coin.SetActive(false);
                _timer.Hide();
                _priceWidget.Show();
            }
        }
        
        private void OnIncomeTimeChanged(TimeSpan time) => 
            _timer.SetText(time.Minutes, time.Seconds, _presenter.RemainTimeFormat);

        private void OnIncomeProgressChanged(float value) => 
            _timerSlider.fillAmount = Mathf.Clamp01(value);

        private void OnIncomeReady(bool value)
        {
            _coin.SetActive(value);
            if(value)
                _timer.Hide();
            else
                _timer.Show();
        }
    }
}