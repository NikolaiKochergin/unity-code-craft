using System;
using Game.Presenters;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private MoneyPanel _moneyPanel;
        
        private MoneyPresenter _presenter;
        
        private IDisposable _disposables;
        
        [Inject]
        public void Construct(MoneyPresenter presenter) => 
            _presenter = presenter;

        private void OnEnable()
        {
            _moneyPanel.SetText(_presenter.Money.CurrentValue, _presenter.Format);
            
            ReadOnlyReactiveProperty<int> money = _presenter.Money;

            DisposableBuilder disposables = Disposable.CreateBuilder();
            money
                .Pairwise()
                .Where(x => x.Current > x.Previous)
                .Subscribe(x => _moneyPanel.AnimateIncrease(x.Previous, x.Current, _presenter.Format))
                .AddTo(ref disposables);
            
            money
                .Pairwise()
                .Where(x => x.Previous > x.Current)
                .Subscribe(x => _moneyPanel.AnimateSet(x.Current, _presenter.Format))
                .AddTo(ref disposables);
            
            _disposables = disposables.Build();
        }

        private void OnDisable() => 
            _disposables.Dispose();


    }
}