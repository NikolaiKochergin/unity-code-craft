using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Presenters;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Views
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField, Min(0)] private float _animationDuration = 0.75f;
        
        private MoneyPresenter _presenter;
        
        private readonly List<Sequence> _animationSequences = new();
        private Coroutine _animationCoroutine;

        private IDisposable _disposables;
        
        [Inject]
        public void Construct(MoneyPresenter presenter) => 
            _presenter = presenter;

        private void OnEnable()
        {
            _moneyText.SetText(_presenter.Format, _presenter.Money.CurrentValue);
            
            ReadOnlyReactiveProperty<int> money = _presenter.Money;

            DisposableBuilder disposables = Disposable.CreateBuilder();
            money
                .Pairwise()
                .Where(x => x.Current > x.Previous)
                .Subscribe(OnMoneyEarned)
                .AddTo(ref disposables);
            
            money
                .Pairwise()
                .Where(x => x.Previous > x.Current)
                .Subscribe(OnMoneySpent)
                .AddTo(ref disposables);
            
            _disposables = disposables.Build();
        }

        private void OnDisable() => 
            _disposables.Dispose();

        private void OnMoneyEarned((int Previous, int Current) x)
        {
            StopAnimations();
            _animationCoroutine = StartCoroutine(AddMoneyAnimation(x.Previous, x.Current - x.Previous));
            BounceAnimation();
        }

        private void OnMoneySpent((int Previous, int Current) x)
        {
            StopAnimations();
            _moneyText.SetText(_presenter.Format, x.Current);
            BounceAnimation();
        }

        private void BounceAnimation()
        {
            Sequence sequence = DOTween.Sequence();

            sequence
                .AppendCallback(() => _animationSequences.Add(sequence))
                .Append(_moneyText.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.2f))
                .Append(_moneyText.transform.DOScale(Vector3.one, 0.4f))
                .OnKill(() => _moneyText.transform.localScale = Vector3.one)
                .OnComplete(() => _animationSequences.Remove(sequence));
        }

        private void StopAnimations()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }
            
            foreach (Sequence sequence in _animationSequences) 
                sequence.Kill();
            
            _animationSequences.Clear();
        }
        
        private IEnumerator AddMoneyAnimation(float startMoney, float range)
        {
            float progress = 0;

            while (progress <= 1)
            {
                yield return null;

                progress = Mathf.Min(1, progress + Time.deltaTime / _animationDuration);

                float currentMoney = startMoney + range * progress;
                _moneyText.SetText(_presenter.Format, currentMoney);
            }
            
            _moneyText.SetText(_presenter.Format, startMoney + range);
        }
    }
}