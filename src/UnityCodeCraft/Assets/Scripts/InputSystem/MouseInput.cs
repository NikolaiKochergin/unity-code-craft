using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SampleGame
{
    [ExecuteAlways]
    public sealed class MouseInput : MonoBehaviour
    {
        private const int LEFT_BUTTON = 0;
        private const int RIGHT_BUTTON = 1;

        public event Action<Vector2> OnLeftClicked;
        public event Action<Vector2> OnRightClicked;

        [ShowInInspector, ReadOnly]
        public Vector2 Position
        {
            get { return this.mousePosition; }
        }

        [ShowInInspector, ReadOnly]
        public ButtonState LeftButton
        {
            get { return this.leftButton; }
        }

        [ShowInInspector, ReadOnly]
        public ButtonState RightButton
        {
            get { return this.rightButton; }
        }

        private Vector2 mousePosition;

        private ButtonState leftButton;
        private ButtonState rightButton;

        private float leftClickTime;
        private float rightClickTime;

        private void Update()
        {
            this.mousePosition = Input.mousePosition;

            this.HandleLeftButton();
            this.HandleRightButton();
        }

        private void HandleLeftButton()
        {
            if (Input.GetMouseButtonDown(LEFT_BUTTON))
            {
                this.leftButton = ButtonState.DOWN;
                return;
            }

            if (Input.GetMouseButton(LEFT_BUTTON))
            {
                this.leftButton = ButtonState.PRESS;
                return;
            }

            if (Input.GetMouseButtonUp(LEFT_BUTTON))
            {
                this.leftButton = ButtonState.UP;
                this.OnLeftClicked?.Invoke(this.mousePosition);
                return;
            }

            this.leftButton = ButtonState.IDLE;
        }

        private void HandleRightButton()
        {
            if (Input.GetMouseButtonDown(RIGHT_BUTTON))
            {
                this.rightButton = ButtonState.DOWN;
                return;
            }

            if (Input.GetMouseButton(RIGHT_BUTTON))
            {
                this.rightButton = ButtonState.PRESS;
                return;
            }

            if (Input.GetMouseButtonUp(RIGHT_BUTTON))
            {
                this.rightButton = ButtonState.UP;
                this.OnRightClicked?.Invoke(this.mousePosition);
                return;
            }

            this.rightButton = ButtonState.IDLE;
        }

        public enum ButtonState
        {
            IDLE,
            DOWN = 0,
            PRESS = 1,
            UP = 2
        }
    }

}