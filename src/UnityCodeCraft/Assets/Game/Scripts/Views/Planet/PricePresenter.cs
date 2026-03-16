using UnityEngine;

namespace Game.Views
{
    public class PricePresenter : MonoBehaviour
    {
        [SerializeField] private Widget _priceView;

        public void Show(int price, string format)
        {
            _priceView.SetText(price, format);
            _priceView.Show();
        }
        
        public void Hide() =>
            _priceView.Hide();
    }
}