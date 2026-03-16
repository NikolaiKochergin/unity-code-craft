using Modules.Planets;
using UnityEngine;

namespace Game.Views
{
    public class IncomePresenter : MonoBehaviour
    {
        private const string Format = "Income: {0:0} / sec";
        
        [SerializeField] private Widget _incomeWidget;
        
        private Planet _planet;

        public void Show(Planet planet)
        {
            _planet = planet;
            OnIncomeChanged(planet.MinuteIncome);
            _planet.OnIncomeChanged += OnIncomeChanged;
        }

        public void Hide() => 
            _planet.OnIncomeChanged -= OnIncomeChanged;

        private void OnIncomeChanged(int value) => 
            _incomeWidget.SetText(value/60, Format);
    }
}