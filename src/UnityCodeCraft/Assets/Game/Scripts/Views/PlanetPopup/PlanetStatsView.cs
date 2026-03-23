using UnityEngine;

namespace Game.Views
{
    public class PlanetStatsView : MonoBehaviour
    {
        [SerializeField] private Widget _population;
        [SerializeField] private Widget _level;
        [SerializeField] private Widget _income;

        public void SetPopulation(int count, string Format) => 
            _population.SetText(count, Format);
        
        public void SetLevel(int current, int max, string Format) =>
            _level.SetText(current, max, Format);
        
        public void SetIncome(int value, string Format) =>
            _income.SetText(value, Format);
    }
}