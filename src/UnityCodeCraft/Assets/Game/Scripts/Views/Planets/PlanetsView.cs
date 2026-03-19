using Game.Presenters;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class PlanetsView : MonoBehaviour
    {
        [SerializeField] private PlanetView[] _planets;
        
        private PlanetsPresenter _presenter;

        [Inject]
        public void Construct(PlanetsPresenter presenter) => 
            _presenter = presenter;

        private void Start()
        {
            int index = 0;
            foreach (PlanetPresenter presenter in _presenter.Planets)
                if(index < _planets.Length)
                    _planets[index++].Show(presenter);
        }

        private void OnDestroy()
        {
            foreach (PlanetView planet in _planets) 
                planet.Hide();
        }
    }
}