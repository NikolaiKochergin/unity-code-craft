using Modules.Planets;
using UnityEngine;

namespace Game.Views
{
    public class UpgradeButtonPresenter : MonoBehaviour
    {
        private const string Format = "{0:0}";
        
        [SerializeField] private UpgradeButtonView _buttonView;
        
        private Planet _planet;

        public void Show(Planet planet)
        {
            _planet = planet;
            
            UpdateView();
            _buttonView.OnClick += OnButtonClick;
        }

        public void Hide() => 
            _buttonView.OnClick -= OnButtonClick;

        private void OnButtonClick()
        {
            if(_planet.UnlockOrUpgrade())
                UpdateView();
        }

        private void UpdateView()
        {
            _buttonView.SetMaxLevelView(_planet.IsMaxLevel);
            _buttonView.SetPrice(_planet.Price, Format);
        }
    }
}