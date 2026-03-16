using Modules.Planets;
using UnityEngine;

namespace Game.Views
{
    public class PopulationPresenter : MonoBehaviour
    {
        private const string Format = "Population: {0:0}";
        
        [SerializeField] private Widget _populationWidget;
        
        private Planet _planet;

        public void Show(Planet planet)
        {
            _planet = planet;
            OnPopulationChanged(planet.Population);
            _planet.OnPopulationChanged += OnPopulationChanged;
        }

        public void Hide() => 
            _planet.OnPopulationChanged -= OnPopulationChanged;

        private void OnPopulationChanged(int amount) => 
            _populationWidget.SetText(amount, Format);
    }
}