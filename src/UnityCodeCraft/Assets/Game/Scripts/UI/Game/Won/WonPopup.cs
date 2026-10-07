using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class WonPopup : MonoBehaviour
    {
        [field: SerializeField] public Button MenuButton { get; private set; }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);
    }
}