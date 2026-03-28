using System;
using Game.Presenters;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class IncomeView : MonoBehaviour
    {
        [SerializeField] private CoinView _coin;
        [SerializeField] private Image _slider;
        [SerializeField] private TMP_Text _timerText;
        
        private PlanetPresenter _presenter;
        private IDisposable _disposables;

        public void Show(PlanetPresenter presenter)
        {
            _presenter = presenter;
            
            DisposableBuilder disposables = Disposable.CreateBuilder();

            presenter.RemainTime
                .Subscribe(OnIncomeTimeChanged)
                .AddTo(ref disposables);
            
            presenter.IncomeProgress
                .Subscribe(OnIncomeProgressChanged)
                .AddTo(ref disposables);
            
            presenter.IsIncomeReady
                .Subscribe(OnIncomeReady)
                .AddTo(ref disposables);

            _disposables = disposables.Build();
        }

        public void Hide()
        {
            _disposables?.Dispose();
            _coin.Hide();
            gameObject.SetActive(false);
        }

        public void TryGatherIncome()
        {
            if(_presenter.IsIncomeReady.CurrentValue)
                _coin.ShowGatherEffect(onFinished: _presenter.GatherIncome);
        }

        private void OnIncomeTimeChanged(TimeSpan time) => 
            _timerText.SetText(_presenter.RemainTimeFormat, time.Minutes, time.Seconds);

        private void OnIncomeProgressChanged(float value) => 
            _slider.fillAmount = Mathf.Clamp01(value);

        private void OnIncomeReady(bool value)
        {
            gameObject.SetActive(!value);
            if(value)
                _coin.Show();
            else
                _coin.Hide();
        }
    }
}