using TMPro;
using UnityEngine;

namespace Game.Views
{
    public class Widget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _price;
        
        public void SetText(float value, string format = "{0}") => 
            _price.SetText(format, value);
        
        public void SetText(float arg1, float arg2, string format = "{0}") => 
            _price.SetText(format, arg1, arg2);

        public void Show() => 
            gameObject.SetActive(true);
        
        public void Hide() =>
            gameObject.SetActive(false);
    }
}