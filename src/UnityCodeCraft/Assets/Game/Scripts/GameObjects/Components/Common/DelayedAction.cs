using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public abstract class DelayedAction : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _delay;

        private Coroutine _coroutine;

        protected void InvokeDelayed(Action action) =>
            _coroutine = StartCoroutine(Delay(action));

        private void OnDestroy()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }

        private IEnumerator Delay(Action action)
        {
            yield return new WaitForSeconds(_delay);
            action?.Invoke();
        }
    }
}