using System.Collections;
using UnityEngine;

namespace Game
{
    public class DelayComponent : ActionComponent
    {
        [SerializeField] private ActionComponent _action;
        [SerializeField, Min(0)] private float _delay = 0.15f;
        
        private WaitForSeconds _seconds;
        private Coroutine _routine;

        private void Awake() =>
            _seconds = new WaitForSeconds(_delay);

        private void OnDestroy()
        {
            if (_routine != null) StopCoroutine(_routine);
        }

        public override void Apply() => 
            _routine = StartCoroutine(DelayRoutine());

        private IEnumerator DelayRoutine()
        {
            yield return _seconds;
            _action?.Apply();
        }
    }
}