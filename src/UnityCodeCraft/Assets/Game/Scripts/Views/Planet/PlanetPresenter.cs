using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class PlanetPresenter : MonoBehaviour
    {
        private const string PriceFormat = "{0:0}";
        
        [SerializeField] private PlanetView _view;
        [SerializeField] private PlanetIncomePresenter _income;
        [SerializeField] private PricePresenter _price;
        
        private Planet _planet;
        private PlanetPopupPresenter _planetPopup;

        [Inject]
        public void Construct(PlanetPopupPresenter planetPopup) => 
            _planetPopup = planetPopup;

        public void Show(Planet planet)
        {
            if (planet == null)
            {
                Debug.LogError($"{nameof(planet)} is null");
                return;
            }
            
            _planet = planet;
            
            UpdatePlanetView();
            
            _view.OnClick += OnClick;
            _view.OnHold += OnHold;
        }

        public void Hide()
        {
            _view.OnClick -= OnClick;
            _view.OnHold -= OnHold;
        }

        private void OnClick()
        {
            if(_planet.IsUnlocked)
                _income.Gather();
            else
                _planet.Unlock();
        }

        private void UpdatePlanetView()
        {
            bool isLocked = !_planet.IsUnlocked;
            
            _view.SetLockEnabled(isLocked);
            _view.SetIcon(_planet.GetIcon(!isLocked));

            if (isLocked)
            {
                _price.Show(_planet.Price, PriceFormat);
                _income.Hide();
                _planet.OnUnlocked += UpdatePlanetView;
            }
            else
            {
                _price.Hide();
                _income.Show(_planet);
                _planet.OnUnlocked -= UpdatePlanetView;
            }
        }

        private void OnHold() => 
            _planetPopup.Show(_planet);
    }
}