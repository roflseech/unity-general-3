using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class EntityDatabaseEditor : Editor
{
    private ReorderableList _list;

    private void OnEnable()
    {
        var itemDefinitions = serializedObject.FindProperty("_entries");

        _list = new(serializedObject, itemDefinitions, true, true, true, true);

        _list.drawHeaderCallback = (rect) =>
        {
            EditorGUI.LabelField(rect, "Items");
        };
        _list.elementHeightCallback = (index) =>
        {
            return EditorGUIUtility.singleLineHeight;
        };
        _list.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            DrawElement(itemDefinitions.GetArrayElementAtIndex(index), rect);
        };
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
    protected virtual void DrawElement(SerializedProperty property, Rect rect)
    {
        var name = property.FindPropertyRelative("_name");
        var entity = property.FindPropertyRelative("_entity");

        var headerRect = rect;
        headerRect.width /= 2;
        name.stringValue = EditorGUI.TextField(headerRect, name.stringValue);
        headerRect.x += headerRect.width;
        EditorGUI.PropertyField(headerRect, entity, GUIContent.none);
    }
}