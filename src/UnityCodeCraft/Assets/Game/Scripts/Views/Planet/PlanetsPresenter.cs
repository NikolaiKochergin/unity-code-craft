using System.Collections.Generic;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class PlanetsPresenter : MonoBehaviour
    {
        [SerializeField] private List<PlanetPresenter> _planetPresenters;
        
        private IEnumerable<Planet> _planets;

        [Inject]
        public void Construct(IEnumerable<Planet> planets) => 
            _planets = planets;

        private void OnEnable()
        {
            int index = 0;
            foreach (Planet planet in _planets)
            {
                if(index == _planetPresenters.Count)
                    return;
                
                _planetPresenters[index].Show(planet);
                index++;
            }
        }

        private void OnDisable()
        {
            foreach (PlanetPresenter presenter in _planetPresenters) 
                presenter.Hide();
        }
    }
}