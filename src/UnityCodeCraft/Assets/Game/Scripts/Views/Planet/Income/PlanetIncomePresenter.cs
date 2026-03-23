using System;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class PlanetIncomePresenter : MonoBehaviour
    {
        private const string Format = "{0:00}m:{1:00}s";
        
        [SerializeField] private PlanetIncomeView _incomeView;
        
        private Planet _planet;
        private CoinParticleView _particlePresenter;

        [Inject]
        public void Construct(CoinParticleView coinParticle) => 
            _particlePresenter = coinParticle;

        public void Show(Planet planet)
        {
            _planet = planet;
            _incomeView.SetActive(true);

            OnIncomeReady(_planet.IsIncomeReady);
            _planet.OnIncomeReady += OnIncomeReady;
            _planet.OnIncomeTimeChanged += OnIncomeTimeChanged;
        }

        public void Hide()
        {
            if (_planet != null)
            {
                _planet.OnIncomeTimeChanged -= OnIncomeTimeChanged;
                _planet.OnIncomeReady -= OnIncomeReady;
            }
            
            _incomeView.SetActive(false);
        }

        public void Gather()
        {
            if(!_planet.IsIncomeReady)
                return;
            
            _incomeView.SetActive(false);
            _particlePresenter.ShowFrom(
                _incomeView.CoinPosition, 
                () =>
                {
                    _planet.GatherIncome();
                    _incomeView.SetActive(true);
                    _incomeView.SetReady(false);
                });
        }

        private void OnIncomeReady(bool isReady) => 
            _incomeView.SetReady(isReady);

        private void OnIncomeTimeChanged(float value)
        {
            TimeSpan timeRemain = TimeSpan.FromSeconds(value);
            _incomeView.SetSlider(_planet.IncomeProgress);
            _incomeView.SetText(timeRemain.Minutes, timeRemain.Seconds, Format);
        }
    }
}