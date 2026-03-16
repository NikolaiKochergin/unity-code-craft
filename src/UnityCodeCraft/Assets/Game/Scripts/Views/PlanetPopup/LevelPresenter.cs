using Modules.Planets;
using UnityEngine;

namespace Game.Views
{
    public class LevelPresenter : MonoBehaviour
    {
        private const string Format = "Level: {0:0}/{1:0}";
        
        [SerializeField] private Widget _levelWidget;
        
        private Planet _planet;

        public void Show(Planet planet)
        {
            _planet = planet;
            OnUpgraded(planet.Level);
            _planet.OnUpgraded += OnUpgraded;
        }

        public void Hide() => 
            _planet.OnUpgraded -= OnUpgraded;

        private void OnUpgraded(int currentLevel) => 
            _levelWidget.SetText(currentLevel, _planet.MaxLevel, Format);
    }
}