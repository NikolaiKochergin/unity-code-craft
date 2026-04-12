using UnityEngine;

namespace Game
{
    public class TossAbility : MonoBehaviour, TossRequestComponent.IAction
    {
        private TossRequestComponent _tossRequest;
        private TossComponent _tossComponent;

        private void Awake()
        {
            _tossRequest = GetComponent<TossRequestComponent>();
            _tossComponent = GetComponent<TossComponent>();
            
            _tossRequest.SetAction(this);
        }

        void TossRequestComponent.IAction.Invoke() => _tossComponent.Toss();
    }
}