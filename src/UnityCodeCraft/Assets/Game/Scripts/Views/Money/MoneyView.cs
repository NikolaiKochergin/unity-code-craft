using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Views
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField, Min(0)] private float _animationDuration = 0.75f;

        private readonly List<Sequence> _animationSequences = new();
        private Coroutine _animationCoroutine;
        
        private string _format;

        public void Setup(float value, string format = "{0}")
        {
            _format = format;
            _moneyText.SetText(format, value);
        }

        public void Spend(int newValue, int _)
        {
            StopAnimations();
            _moneyText.SetText(_format, newValue);
            BounceAnimation();
        }

        public void Earn(int newValue, int prevValue)
        {
            StopAnimations();
            _animationCoroutine = StartCoroutine(AddMoneyAnimation(prevValue, newValue - prevValue));
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
                _moneyText.SetText(_format, currentMoney);
            }
            
            _moneyText.SetText(_format, startMoney + range);
        }
    }
}