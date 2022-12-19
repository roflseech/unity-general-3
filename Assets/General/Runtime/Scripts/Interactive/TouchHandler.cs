using UniRx;
using UnityEngine;
using System;

namespace UnityGeneral
{
    public abstract class TouchHandler : MonoBehaviour
    {
        public IObservable<Unit> OnTouchDown => _onTouchDown;
        public IObservable<Unit> OnTouchUp => _onTouchUp;

        private Subject<Unit> _onTouchDown = new();
        private Subject<Unit> _onTouchUp = new();

        private bool _touchDown;

        private bool _touchDownThisFrame;
        private bool _touchUpThisFrame;

        public bool GetTouchDown() => _touchDownThisFrame;
        public bool GetTouchUp() => _touchUpThisFrame;
        public bool GetTouch() => _touchDown;

        protected void TouchDown()
        {
            if (!_touchDown)
            {
                _touchDownThisFrame = true;
                _touchDown = true;
                _onTouchDown.OnNext(Unit.Default);
            }
        }
        protected void TouchUp()
        {
            if (_touchDown)
            {
                _touchUpThisFrame = true;
                _touchDown = false;
                _onTouchUp.OnNext(Unit.Default);
            }
        }
        private void LateUpdate()
        {
            _touchDownThisFrame = false;
            _touchUpThisFrame = false;
        }
    }
}