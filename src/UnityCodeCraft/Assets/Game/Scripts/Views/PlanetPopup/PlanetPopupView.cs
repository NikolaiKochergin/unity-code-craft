using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlanetPopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _planetName;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _closeButton;
        
        public event UnityAction OnClose
        {
            add => _closeButton.onClick.AddListener(value);
            remove => _closeButton.onClick.RemoveListener(value);
        }

        private void Awake() => 
            Hide();

        public void Show() => 
            gameObject.SetActive(true);

        public void Hide() => 
            gameObject.SetActive(false);
        
        public void SetPlanetName(string planetName) =>
            _planetName.SetText(planetName);
        
        public void SetIcon(Sprite sprite) =>
            _icon.sprite = sprite;
    }
}