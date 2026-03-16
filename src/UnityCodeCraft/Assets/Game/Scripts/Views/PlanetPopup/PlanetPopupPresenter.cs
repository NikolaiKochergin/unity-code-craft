using Modules.Planets;
using UnityEngine;

namespace Game.Views
{
    public class PlanetPopupPresenter : MonoBehaviour
    {
        [SerializeField] private PlanetPopupView _planetPopupView;
        
        [SerializeField] private PopulationPresenter _populationPresenter;
        [SerializeField] private LevelPresenter _levelPresenter;
        [SerializeField] private IncomePresenter _incomePresenter;
        [SerializeField] private UpgradeButtonPresenter _upgradeButtonPresenter;
        
        private Planet _planet;

        public void Show(Planet planet)
        {
            if (planet == null)
            {
                Debug.LogError($"{nameof(planet)} is null");
                return;
            }
            
            if (_planet != planet) 
                _planet = planet;
            
            UpdateName();
            UpdateIcon();

            _planetPopupView.Show();
            _populationPresenter.Show(planet);
            _levelPresenter.Show(planet);
            _incomePresenter.Show(planet);
            _upgradeButtonPresenter.Show(planet);

            _planet.OnUnlocked += UpdateIcon;
            _planetPopupView.OnClose += Hide;
        }

        public void Hide()
        {
            _planet.OnUnlocked += UpdateIcon;
            _planetPopupView.OnClose -= Hide;
            
            _planetPopupView.Hide();
            _populationPresenter.Hide();
            _levelPresenter.Hide();
            _incomePresenter.Hide();
            _upgradeButtonPresenter.Hide();
        }

        private void UpdateName() => 
            _planetPopupView.SetPlanetName(_planet.Name);

        private void UpdateIcon() => 
            _planetPopupView.SetIcon(_planet.GetIcon(_planet.IsUnlocked));
    }
}