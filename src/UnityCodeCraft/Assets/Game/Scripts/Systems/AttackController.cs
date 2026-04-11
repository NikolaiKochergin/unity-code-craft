using UnityEngine;

namespace Game
{
    public class AttackController : MonoBehaviour
    {
        [SerializeField] private GameObject _character;
        
        private ITossComponent _tossComponent;
        private IPushComponent _pushComponent;

        private void Awake()
        {
            _tossComponent = _character.GetComponent<ITossComponent>();
            _pushComponent = _character.GetComponent<IPushComponent>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
                _tossComponent.Toss();

            if (Input.GetMouseButton(1))
                _pushComponent.Push();
        }
    }
}
