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
        private MoneyParticleAnimatorPresenter _particlePresenter;

        [Inject]
        public void Construct(MoneyParticleAnimatorPresenter particlePresenter) => 
            _particlePresenter = particlePresenter;

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
            int time = (int)value;
            _incomeView.SetSlider(_planet.IncomeProgress);
            _incomeView.SetText(time / 60, time % 60, Format);
        }
    }
}