using Modules.Planets;
using UnityEngine;

namespace Game.Views
{
    public class PlanetStatsPresenter : MonoBehaviour
    {
        private const string PopulationFormat = "Population: {0:0}";
        private const string LevelFormat = "Level: {0:0}/{1:0}";
        private const string IncomeFormat = "Income: {0:0} / sec";
        
        [SerializeField] private PlanetStatsView _planetStatsView;
        
        private Planet _planet;

        public void Show(Planet planet)
        {
            if(planet == null)
                return;
            
            _planet = planet;

            UpdateStats();
            _planet.OnUnlocked += UpdateStats;
            _planet.OnPopulationChanged += OnPopulationChanged;
            _planet.OnUpgraded += OnUpgraded;
            _planet.OnIncomeChanged += OnIncomeChanged;
        }

        public void Hide()
        {
            if(_planet == null)
                return;
            
            _planet.OnUnlocked -= UpdateStats;
            _planet.OnPopulationChanged -= OnPopulationChanged;
            _planet.OnUpgraded -= OnUpgraded;
            _planet.OnIncomeChanged -= OnIncomeChanged;
            
            _planet = null;
        }

        private void UpdateStats()
        {
            OnPopulationChanged(_planet.Population);
            OnUpgraded(_planet.Level);
            OnIncomeChanged(_planet.MinuteIncome);
        }

        private void OnPopulationChanged(int value) => 
            _planetStatsView.SetPopulation(value, PopulationFormat);

        private void OnUpgraded(int currentLevel) => 
            _planetStatsView.SetLevel(currentLevel, _planet.MaxLevel, LevelFormat);

        private void OnIncomeChanged(int value) => 
            _planetStatsView.SetIncome(value/60, IncomeFormat);
    }
}