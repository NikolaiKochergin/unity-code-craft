using R3;

namespace Game.Presenters
{
    public class PlanetPopupPresenter
    {
        private readonly ReactiveProperty<bool> _isShown = new(false);
        
        public PlanetPresenter Planet { get; private set; }
        public ReadOnlyReactiveProperty<bool> IsShown => _isShown;

        public void Show(PlanetPresenter planetPresenter)
        {
            Planet = planetPresenter;
            _isShown.Value = true;
        }

        public void Hide()
        {
            _isShown.Value = false;
            Planet = null;
        }
    }
}