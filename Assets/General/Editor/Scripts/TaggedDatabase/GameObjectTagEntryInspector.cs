using UnityEditor;
using UnityEngine;

namespace UnityGeneral
{
    [CustomPropertyDrawer(typeof(GameObjectTagDatabase.GameObjectTagEntry))]
    public class GameObjectTagEntryInspector : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var tagProperty = property.FindPropertyRelative("_tag");
            var valueProperty = property.FindPropertyRelative("_value");

            position.width /= 2;

            tagProperty.stringValue = EditorGUI.TextField(position, tagProperty.stringValue);
            position.x += position.width;
            valueProperty.objectReferenceValue = EditorGUI.ObjectField(
                position,
                GUIContent.none,
                valueProperty.objectReferenceValue,
                typeof(GameObject),
                false);
        }
    }
}