using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UnityGeneral
{
    public class TouchPanel : TouchHandler
    {
        [SerializeField]
        private RectTransform _targetPanel;

        public RectTransform TargetPanel { get => _targetPanel; set => _targetPanel = value; }

        public Vector2 ScreenSpaceToRelative(Vector2 pos)
        {
            var rect = RectTransformToScreenSpace(_targetPanel);
            return (pos - rect.center) / rect.size + Vector2.one * 0.5f;
        }

        private void Start()
        {
            if (!_targetPanel.TryGetComponent<EventTrigger>(out var eventTrigger))
            {
                eventTrigger = _targetPanel.gameObject.AddComponent<EventTrigger>();
            }

            AddTriger(eventTrigger, EventTriggerType.PointerDown, data => TouchDown());
            AddTriger(eventTrigger, EventTriggerType.PointerUp, data => TouchUp());
            AddTriger(eventTrigger, EventTriggerType.PointerExit, data => TouchUp());
        }
        private void AddTriger(EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> callback)
        {
            var entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }

        private void OnDestroy()
        {
            if (_targetPanel != null && _targetPanel.TryGetComponent<EventTrigger>(out var eventTrigger))
            {
                eventTrigger.triggers.Clear();
            }
        }
        private Rect RectTransformToScreenSpace(RectTransform rectTransform)
        {
            Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale);
            Rect rect = new Rect(rectTransform.position.x, Screen.height - rectTransform.position.y, size.x, size.y);
            rect.x -= rectTransform.pivot.x * size.x;
            rect.y -= (1.0f - rectTransform.pivot.y) * size.y;
            return rect;
        }
    }
}