using UnityEditor;
using UnityEngine;

namespace UnityGeneral
{
    [CustomPropertyDrawer(typeof(FloatRange))]
    public class FloatRangeInspector : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var min = property.FindPropertyRelative("_min");
            var max = property.FindPropertyRelative("_max");

            float space = position.width / 100.0f;

            position.width = position.width / 6 - space * 2;

            var color = GUI.color;
            GUI.color = Color.yellow;
            position.width *= 2;
            EditorGUI.LabelField(position, label);
            position.width /= 2;
            GUI.color = color;

            position.x += position.width * 2 + space;

            EditorGUI.LabelField(position, "Min");
            position.x += position.width;
            min.floatValue = EditorGUI.FloatField(position, min.floatValue);
            position.x += position.width + space;

            EditorGUI.LabelField(position, "Max");
            position.x += position.width;
            max.floatValue = EditorGUI.FloatField(position, max.floatValue);

            if (min.floatValue > max.floatValue) max.floatValue = min.floatValue;
        }
    }
}