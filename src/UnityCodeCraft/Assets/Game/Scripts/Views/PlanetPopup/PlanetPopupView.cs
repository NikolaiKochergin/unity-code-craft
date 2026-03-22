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
        [SerializeField] private PlanetStatsView _stats;
        [SerializeField] private UpgradeButtonView _upgradeButton;
        
        private PlanetPopupPresenter _presenter;
        private IDisposable _disposables;
        private IDisposable _planetDisposables;

        [Inject]
        public void Construct(PlanetPopupPresenter presenter) => 
            _presenter = presenter;

        private void Start()
        {
            DisposableBuilder disposables = Disposable.CreateBuilder();
            
            _presenter.IsShown
                .Subscribe(isShown => gameObject.SetActive(isShown))
                .AddTo(ref disposables);
            
            _disposables = disposables.Build();
        }

        private void OnDestroy() => 
            _disposables.Dispose();

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(_presenter.Hide);

            PlanetPresenter planet = _presenter.Planet;
            
            if(planet == null)
                return;
            
            _planetName.SetText(planet.Name);
            _stats.Initialize(planet.Stats);
            _upgradeButton.Initialize(planet);
            
            DisposableBuilder disposables = Disposable.CreateBuilder();
            
            planet.Icon
                .Subscribe(sprite => _icon.sprite = sprite)
                .AddTo(ref disposables);
            
            _planetDisposables = disposables.Build();
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(_presenter.Hide);
            _stats.Dispose();
            _upgradeButton.Dispose();
            
            if(_presenter.Planet != null)
                _planetDisposables.Dispose();
        }
    }
}