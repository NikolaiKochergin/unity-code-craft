using System;
using Modules.Planets;
using R3;

namespace Game.Presenters
{
    public class IncomePresenter : IDisposable
    {
        private readonly Planet _planet;
        private readonly ReactiveProperty<bool> _isIncomeReady;
        private readonly ReactiveProperty<float> _incomeProgress;
        private readonly ReactiveProperty<TimeSpan> _remainTime;

        public IncomePresenter(Planet planet)
        {
            _planet = planet;
            _isIncomeReady = new ReactiveProperty<bool>(planet.IsIncomeReady);
            _incomeProgress = new ReactiveProperty<float>(planet.IncomeProgress);
            _remainTime = new ReactiveProperty<TimeSpan>();
        }
        
        public string Format => "{0:00}m:{1:00}s";
        public ReadOnlyReactiveProperty<bool> IsIncomeReady => _isIncomeReady;
        public ReadOnlyReactiveProperty<float> IncomeProgress => _incomeProgress;
        public ReadOnlyReactiveProperty<TimeSpan> RemainTime => _remainTime;

        public void Initialize()
        {
            _planet.OnIncomeReady += OnIncomeReady;
            _planet.OnIncomeTimeChanged += OnIncomeTimeChanged;
        }

        public void Dispose()
        {
            _planet.OnIncomeReady -= OnIncomeReady;
            _planet.OnIncomeTimeChanged -= OnIncomeTimeChanged;
        }

        private void OnIncomeReady(bool isReady)
        {
            _isIncomeReady.Value = isReady;
            _remainTime.Value = TimeSpan.Zero;
            _incomeProgress.Value = _planet.IncomeProgress;
        }

        private void OnIncomeTimeChanged(float value)
        {
            _remainTime.Value = TimeSpan.FromSeconds(value);
            _incomeProgress.Value = _planet.IncomeProgress;
        }

        public void GatherIncome() => 
            _planet.GatherIncome();
    }
}