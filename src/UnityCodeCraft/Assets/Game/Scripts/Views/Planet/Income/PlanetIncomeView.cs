using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlanetIncomeView : MonoBehaviour
    {
        [SerializeField] private GameObject _coin;
        [SerializeField] private Image _slider;
        [SerializeField] private TMP_Text _timerText;
        
        public Vector3 CoinPosition => _coin.transform.position;

        public void SetSlider(float value) => 
            _slider.fillAmount = Mathf.Clamp01(value);

        public void SetText(int minutes, int seconds, string format = "{0}m:{1}s") => 
            _timerText.SetText(format, minutes, seconds);

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
            _coin.SetActive(value);
        }

        public void SetReady(bool value)
        {
            _coin.SetActive(value);
            gameObject.SetActive(!value);
        }
    }
}