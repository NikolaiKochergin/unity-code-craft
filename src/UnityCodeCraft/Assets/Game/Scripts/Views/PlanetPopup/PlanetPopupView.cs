using System;
using Game.Presenters;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Views
{
    public class PlanetPopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _planetName;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _icon;
        
        [SerializeField] private Widget _population;
        [SerializeField] private Widget _level;
        [SerializeField] private Widget _income;
        
        [SerializeField] private UpgradeButtonView _upgradeButton;
        
        private PlanetPopupPresenter _presenter;
        private IDisposable _disposables;
        private IDisposable _planetDisposables;

        [Inject]
        public void Construct(PlanetPopupPresenter presenter)
        {
            _presenter = presenter;
            
            DisposableBuilder disposables = Disposable.CreateBuilder();
            
            _presenter.IsShown
                .Subscribe(gameObject.SetActive)
                .AddTo(ref disposables);
            
            _disposables = disposables.Build();
        }

        private void OnDestroy() => 
            _disposables.Dispose();

        private void OnEnable()
        {
            PlanetPresenter planet = _presenter.Planet;
            
            if(planet == null)
                return;
            
            _planetName.SetText(planet.Name);
            _upgradeButton.Initialize(planet);
            
            DisposableBuilder disposables = Disposable.CreateBuilder();

            _closeButton
                .OnClickAsObservable()
                .Subscribe(_ => _presenter.Hide())
                .AddTo(ref disposables);
            
            planet.Icon
                .Subscribe(sprite => _icon.sprite = sprite)
                .AddTo(ref disposables);


            planet.Population
                .Subscribe(population => _population.SetText(population, planet.PopulationFormat))
                .AddTo(ref disposables);
            
            planet.CurrentLevel
                .Subscribe(currentLevel => _level.SetText(currentLevel, planet.MaxLevel, planet.LevelFormat))
                .AddTo(ref disposables);
            
            planet.MinuteIncome
                .Subscribe(income => _income.SetText(income, planet.IncomeFormat))
                .AddTo(ref disposables);
            
            _planetDisposables = disposables.Build();
        }

        private void OnDisable()
        {
            _upgradeButton.Dispose();
            
            if(_presenter.Planet != null)
                _planetDisposables.Dispose();
        }
    }
}