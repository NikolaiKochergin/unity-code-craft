using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Views
{
    public class UpgradeButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Widget _priceWidget;
        [SerializeField] private GameObject _commonView;
        [SerializeField] private GameObject _maxLevelView;
        
        public event UnityAction OnClick
        {
            add => _button.onClick.AddListener(value);
            remove => _button.onClick.RemoveListener(value);
        }

        public void SetPrice(int amount, string format) => 
            _priceWidget.SetText(amount, format);

        public void SetMaxLevelView(bool value)
        {
            _button.interactable = !value;
            _commonView.SetActive(!value);
            _maxLevelView.SetActive(value);
            
            if(value)
                _priceWidget.Hide();
            else
                _priceWidget.Show();
        }
    }
}