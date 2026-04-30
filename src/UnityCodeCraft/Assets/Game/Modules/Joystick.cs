using System;
using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
#else
using UnityEngine.UI;
#endif

namespace Game.Modules
{
#if !ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(CanvasRenderer))]
#endif
    public class Joystick :
#if ENABLE_INPUT_SYSTEM
        OnScreenControl,
#else
        Graphic,
#endif
        IPointerDownHandler,
        IDragHandler,
        IPointerUpHandler
    {
        [Header("Rect References")] 
        [SerializeField] private RectTransform _backgroundRect;
        [SerializeField] private RectTransform _handleRect;

        [Header("Settings")] 
        [SerializeField, Min(1f)] private float _range = 100;

        [SerializeField] private bool _hideWhenIdle;
        [SerializeField] private bool _followTouch;
        [SerializeField] private bool _showAtTouch;
        [SerializeField] private bool _invertX;
        [SerializeField] private bool _invertY;

#if ENABLE_INPUT_SYSTEM
        [Header("Input System")] 
        [InputControl(layout = "Vector2")] 
        [SerializeField] private string _controlPath = "<Gamepad>/leftStick";
#endif

        private int _pointerId = int.MinValue;
        private Vector2 _defaultPosition = Vector2.zero;
        private Vector2 _direction = Vector2.zero;
        private RectTransform _rectTransform;

        public Vector2 Direction
        {
            get => _direction;
            private set
            {
                _direction.x = _invertX ? -value.x : value.x;
                _direction.y = _invertY ? -value.y : value.y;

                OnDirectionChanged?.Invoke(_direction);

#if ENABLE_INPUT_SYSTEM
                SendValueToControl(_direction);
#endif
            }
        }

        public event Action<Vector2> OnDirectionChanged;

#if ENABLE_INPUT_SYSTEM
        protected override string controlPathInternal
        {
            get => _controlPath;
            set => _controlPath = value;
        }
#else
        public override void SetMaterialDirty() { }
        public override void SetVerticesDirty() { }
#endif

#if ENABLE_INPUT_SYSTEM
        private void Awake()
        {
#else
        protected override void Awake()
        {
            base.Awake();
#endif
            _rectTransform = (RectTransform)transform;
            _defaultPosition = _backgroundRect.anchoredPosition;
            ResetJoystick();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_pointerId != int.MinValue)
                return;

            _pointerId = eventData.pointerId;

            if (_followTouch || _showAtTouch)
                SetBackgroundPosition(eventData);

            _backgroundRect.gameObject.SetActive(true);
            UpdateHandle(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.pointerId != _pointerId)
                return;

            UpdateHandle(eventData);

            if (_followTouch)
                SetBackgroundPosition(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerId != _pointerId)
                return;

            ResetJoystick();
        }

        private void UpdateHandle(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _backgroundRect,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 handlePosition);

            _handleRect.anchoredPosition = Vector2.ClampMagnitude(handlePosition, _range);
            Direction = _handleRect.anchoredPosition / _range;
        }

        private void SetBackgroundPosition(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 backgroundPosition);

            _backgroundRect.anchoredPosition = backgroundPosition - _handleRect.anchoredPosition;
        }

        private void ResetJoystick()
        {
            _pointerId = int.MinValue;
            _handleRect.anchoredPosition = Vector2.zero;
            Direction = Vector2.zero;
            _backgroundRect.gameObject.SetActive(!_hideWhenIdle);
            _backgroundRect.anchoredPosition = _defaultPosition;
        }
    }
}