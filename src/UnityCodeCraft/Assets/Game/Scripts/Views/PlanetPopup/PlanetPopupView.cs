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
        
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Widget _priceWidget;
        [SerializeField] private GameObject _commonView;
        [SerializeField] private GameObject _maxLevelView;
        
        private PlanetPopupPresenter _presenter;
        private IDisposable _showDisposables;
        private IDisposable _presentationDisposables;

        [Inject]
        public void Construct(PlanetPopupPresenter presenter)
        {
            _presenter = presenter;
            
            DisposableBuilder disposables = Disposable.CreateBuilder();
            
            _presenter.IsShown
                .Subscribe(gameObject.SetActive)
                .AddTo(ref disposables);
            
            _showDisposables = disposables.Build();
        }

        private void OnDestroy() => 
            _showDisposables.Dispose();

        private void OnEnable()
        {
            PlanetPresenter planet = _presenter.Planet;
            
            if(planet == null)
                return;
            
            _planetName.SetText(planet.Name);
            
            DisposableBuilder disposables = Disposable.CreateBuilder();

            _closeButton
                .OnClickAsObservable()
                .Subscribe(_ => _presenter.Hide())
                .AddTo(ref disposables);
            
            _upgradeButton
                .OnClickAsObservable()
                .Subscribe(_ => planet.UnlockOrUpgrade())
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
            
            planet.Price
                .Subscribe(price => _priceWidget.SetText(price, planet.PriceFormat))
                .AddTo(ref disposables);
            
            planet.IsMaxLevel
                .Subscribe(OnGetMaxLevel)
                .AddTo(ref disposables);
            
            _presentationDisposables = disposables.Build();
        }

        private void OnDisable()
        {
            if(_presenter.Planet != null)
                _presentationDisposables.Dispose();
        }
        
        private void OnGetMaxLevel(bool isMaxLevel)
        {
            _upgradeButton.interactable = !isMaxLevel;
            _commonView.SetActive(!isMaxLevel);
            _maxLevelView.SetActive(isMaxLevel);

            if (isMaxLevel)
                _priceWidget.Hide();
            else
                _priceWidget.Show();
        }
    }
}