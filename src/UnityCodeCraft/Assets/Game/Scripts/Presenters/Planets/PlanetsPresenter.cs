using System;
using System.Collections.Generic;
using Modules.Planets;
using Zenject;

namespace Game.Presenters
{
    public class PlanetsPresenter : IInitializable, IDisposable
    {
        private readonly IEnumerable<Planet> _planets;
        private readonly PlanetPresenterFactory _factory;
        private readonly List<PlanetPresenter> _planetPresenters = new();

        public PlanetsPresenter(
            IEnumerable<Planet> planets,
            PlanetPresenterFactory factory
            )
        {
            _factory = factory;
            _planets = planets;
        }

        public IEnumerable<PlanetPresenter> Planets => _planetPresenters;

        public void Initialize()
        {
            foreach (Planet planet in _planets)
            {
                PlanetPresenter presenter = _factory.Create(planet);
                presenter.Initialize();
                _planetPresenters.Add(presenter);
            }
        }

        public void Dispose()
        {
            foreach (PlanetPresenter presenter in _planetPresenters) 
                presenter.Dispose();
            
            _planetPresenters.Clear();
        }
    }
}