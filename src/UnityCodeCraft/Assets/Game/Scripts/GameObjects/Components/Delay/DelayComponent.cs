using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class DelayComponent : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _delay = 0.15f;
        
        private WaitForSeconds _seconds;
        private Coroutine _routine;

        private void Awake() =>
            _seconds = new WaitForSeconds(_delay);

        private void OnDestroy()
        {
            if (_routine != null) StopCoroutine(_routine);
        }

        public void DelayedInvoke(Action action) => 
            _routine = StartCoroutine(DelayRoutine(action));

        private IEnumerator DelayRoutine(Action action)
        {
            yield return _seconds;
            action?.Invoke();
        }
    }
}