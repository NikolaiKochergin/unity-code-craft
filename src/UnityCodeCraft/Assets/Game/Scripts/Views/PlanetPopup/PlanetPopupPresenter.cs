using Modules.Planets;
using UnityEngine;

namespace Game.Views
{
    public class PlanetPopupPresenter : MonoBehaviour
    {
        [SerializeField] private PlanetPopupView _planetPopupView;

        [SerializeField] private PlanetStatsPresenter _planetStatsPresenter;
        [SerializeField] private UpgradeButtonPresenter _upgradeButtonPresenter;
        
        private Planet _planet;

        public void Show(Planet planet)
        {
            if (planet == null)
            {
                Debug.LogError($"{nameof(planet)} is null");
                return;
            }
            
            _planet = planet;
            
            UpdateName();
            UpdateIcon();

            _planetPopupView.Show();
            _planetStatsPresenter.Show(planet);
            _upgradeButtonPresenter.Show(planet);

            _planet.OnUnlocked += UpdateIcon;
            _planetPopupView.OnClose += Hide;
        }

        public void Hide()
        {
            _planet.OnUnlocked -= UpdateIcon;
            _planetPopupView.OnClose -= Hide;
            
            _planetPopupView.Hide();
            _planetStatsPresenter.Hide();
            _upgradeButtonPresenter.Hide();

            _planet = null;
        }

        private void UpdateName() => 
            _planetPopupView.SetPlanetName(_planet.Name);

        private void UpdateIcon() => 
            _planetPopupView.SetIcon(_planet.GetIcon(_planet.IsUnlocked));
    }
}