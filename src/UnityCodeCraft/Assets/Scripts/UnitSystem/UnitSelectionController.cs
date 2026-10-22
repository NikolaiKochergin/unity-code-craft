using UnityEngine;

namespace SampleGame
{
    public sealed class UnitSelectionController : MonoBehaviour
    {
        private const float MIN_RECT_DIAGONAL = 5f;
        private static Plane GROUND = new(Vector3.up, Vector3.zero);

        [SerializeField]
        private RectInput rectInput;

        [SerializeField]
        private MouseInput mouseInput;

        [SerializeField]
        private new Camera camera;

        [SerializeField]
        private UnitSelectionManager _selectionManager;

        private bool _rectSelctionStarted;

        private void OnEnable()
        {
            this.rectInput.OnStarted += this.OnRectSelectionStarted;
            this.rectInput.OnFinished += this.OnRectSelectionFinished;
            this.mouseInput.OnLeftClicked += this.OnLeftClicked;
            this.mouseInput.OnRightClicked += this.OnRightClicked;
        }

        private void OnDisable()
        {
            this.rectInput.OnStarted -= this.OnRectSelectionStarted;
            this.rectInput.OnFinished -= this.OnRectSelectionFinished;
            this.mouseInput.OnLeftClicked -= this.OnLeftClicked;
            this.mouseInput.OnRightClicked -= this.OnRightClicked;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.LeftCommand))
                _selectionManager.SelectAllUnits();
        }


        private void OnRectSelectionStarted()
        {
            _rectSelctionStarted = true;
        }

        private void OnLeftClicked(Vector2 point)
        {
            if (_rectSelctionStarted)
                return;
            bool additive = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            this.SelectAsPoint(point, additive);
        }

        private void OnRightClicked(Vector2 _)
        {
            _selectionManager.ClearUnits();
        }

        private void OnRectSelectionFinished()
        {
            Vector3 screenStartPoint = this.rectInput.StartPoint;
            Vector3 screenEndPoint = this.rectInput.EndPoint;

            bool additive = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            this.SelectAsRect(screenStartPoint, screenEndPoint, additive);

            _rectSelctionStarted = false;
        }

        private void SelectAsPoint(Vector2 screenPoint, bool extraMode)
        {
            Ray ray = this.camera.ScreenPointToRay(screenPoint);
            _selectionManager.SelectUnit(ray, extraMode);
        }

        private void SelectAsRect(Vector2 screenStartPoint, Vector2 screenEndPoint, bool extraMode)
        {
            Vector3 worldStartPosition = this.RaycastGroundPosition(screenStartPoint);
            Vector3 worldEndPosition = this.RaycastGroundPosition(screenEndPoint);
            _selectionManager.SelectUnits(worldStartPosition, worldEndPosition, extraMode);
        }

        private Vector3 RaycastGroundPosition(Vector2 screenPoint)
        {
            var ray = this.camera.ScreenPointToRay(screenPoint);
            GROUND.Raycast(ray, out var distance);
            return ray.GetPoint(distance);
        }
    }
}