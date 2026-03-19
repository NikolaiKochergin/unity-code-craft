using System;
using Game.Presenters;
using Modules.UI;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Views
{
    public class PlanetView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _lock;
        [SerializeField] private SmartButton _button;
        
        private IDisposable _disposables;
        private PlanetPresenter _presenter;

        public void Show(PlanetPresenter presenter)
        {
            _presenter = presenter;
            
            DisposableBuilder disposables = Disposable.CreateBuilder();
            
            _presenter.Icon
                .Subscribe(sprite => _icon.sprite = sprite)
                .AddTo(ref disposables);
            
            _presenter.IsUnlocked
                .Subscribe(isUnlocked => _lock.SetActive(!isUnlocked))
                .AddTo(ref disposables);

            _disposables = disposables.Build();

            _button.OnClick += _presenter.OnClick;
        }

        public void Hide()
        {
            _disposables.Dispose();
            
            _button.OnClick -= _presenter.OnClick;
        }
    }
}