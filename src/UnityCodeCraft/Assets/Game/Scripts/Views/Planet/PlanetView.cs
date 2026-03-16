using System;
using Modules.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlanetView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _lock;
        [SerializeField] private SmartButton _button;

        public event Action OnClick
        {
            add => _button.OnClick += value;
            remove => _button.OnClick -= value;
        }
        
        public event Action OnHold
        {
            add => _button.OnHold += value;
            remove => _button.OnHold -= value;
        }

        public void SetIcon(Sprite sprite) => 
            _icon.sprite = sprite;
        
        public void SetLockEnabled(bool isLock) => 
            _lock.SetActive(isLock);
    }
}