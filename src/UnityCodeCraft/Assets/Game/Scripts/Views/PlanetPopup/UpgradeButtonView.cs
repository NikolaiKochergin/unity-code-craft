using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Views
{
    public class UpgradeButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Widget _priceWidget;
        [SerializeField] private TMP_Text _buttonText;
        
        public event UnityAction OnClick
        {
            add => _button.onClick.AddListener(value);
            remove => _button.onClick.RemoveListener(value);
        }

        public void SetPrice(int amount, string format) => 
            _priceWidget.SetText(amount, format);

        public void SetMaxLevelView(bool value)
        {
            _buttonText.SetText(value ? "MAX LEVEL" : "Upgrade");
            _button.interactable = !value;
            
            if(value)
                _priceWidget.Hide();
            else
                _priceWidget.Show();
        }
    }
}