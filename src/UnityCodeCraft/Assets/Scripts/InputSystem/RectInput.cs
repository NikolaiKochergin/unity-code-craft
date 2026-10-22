using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SampleGame
{
    [ExecuteAlways]
    public sealed class RectInput : MonoBehaviour
    {
        public event Action OnStarted;
        public event Action OnFinished;

        [SerializeField]
        private MouseInput mouse;

        [SerializeField]
        private float _threshold = 5f; // Минимальное расстояние в пикселях

        private bool isDragging;
        private bool isSelecting;
        private Vector2 startPoint;
        private Vector2 endPoint;

        [ShowInInspector, ReadOnly]
        public bool IsSelecting => this.isSelecting;

        [ShowInInspector, ReadOnly]
        public bool IsDragging => isDragging;

        [ShowInInspector, ReadOnly]
        public Vector2 StartPoint => this.startPoint;

        [ShowInInspector, ReadOnly]
        public Vector2 EndPoint => this.endPoint;

        private void Update()
        {
            var buttonState = this.mouse.LeftButton;

            if (buttonState == MouseInput.ButtonState.DOWN)
            {
                this.isDragging = true;
                this.isSelecting = false;
                this.startPoint = this.mouse.Position;
                this.endPoint = this.startPoint;
            }
            else if (buttonState == MouseInput.ButtonState.PRESS)
            {
                this.endPoint = this.mouse.Position;

                if (!this.isSelecting)
                {
                    float distance = Vector2.Distance(this.startPoint, this.endPoint);
                    if (distance >= _threshold)
                    {
                        this.isSelecting = true;
                        this.OnStarted?.Invoke();
                    }
                }
            }
            else if (buttonState == MouseInput.ButtonState.UP)
            {
                if (this.isSelecting)
                    this.OnFinished?.Invoke();

                this.isDragging = false;
                this.isSelecting = false;
                this.endPoint = this.mouse.Position;
            }
        }
    }
}