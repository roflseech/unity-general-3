using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class DataProviderEditor : Editor
{
    private static readonly Color firstColor = new Color(0.8f, 0.8f, 1.5f);
    private static readonly Color secondColor = new Color(1.5f, 0.8f, 0.8f);

    private ReorderableList _list;
    private SerializedProperty _databaseProperty;
    private UnityEngine.Object _lastDatabase;

    [field: NonSerialized]
    private bool _unfoldProperties;

    public DataProviderEditor()
    {
        _unfoldProperties = false;
    }

    public DataProviderEditor(bool unfoldProperties = false)
    {
        _unfoldProperties = unfoldProperties;
    }

    private void OnEnable()
    {
        Initialize();
    }
    private void Initialize()
    {
        target.GetType().GetMethod("UpdateEntries", new Type[] { }).Invoke(target, new object[] { });

        _databaseProperty = serializedObject.FindProperty("_database");
        _lastDatabase = _databaseProperty.objectReferenceValue;

        if (_lastDatabase == null)
        {
            return;
        }

        var itemDefinitions = serializedObject.FindProperty("_entries");

        _list = new(serializedObject, itemDefinitions, true, true, false, false);

        _list.drawHeaderCallback = (rect) =>
        {
            EditorGUI.LabelField(rect, "Items");
        };
        _list.elementHeightCallback = (index) =>
        {
            return GetElementHeight(itemDefinitions.GetArrayElementAtIndex(index));
        };
        _list.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            DrawElement(itemDefinitions.GetArrayElementAtIndex(index), rect, index % 2 == 0 ? firstColor : secondColor);
        };

    }
    public override void OnInspectorGUI()
    {
        if (_lastDatabase != _databaseProperty.objectReferenceValue)
        {
            Initialize();
        }
        serializedObject.Update();
        EditorGUILayout.PropertyField(_databaseProperty);
        if (_lastDatabase != null)
        {
            _list.DoLayoutList();
        }
        serializedObject.ApplyModifiedProperties();
    }
    protected virtual float GetElementHeight(SerializedProperty property)
    {
        var name = property.FindPropertyRelative("_name");
        var data = property.FindPropertyRelative("_data");

        float result = EditorGUI.GetPropertyHeight(name);

        if (_unfoldProperties)
        {
            foreach (var innerElement in GetChildren(data))
            {
                result += EditorGUI.GetPropertyHeight(innerElement);
            }
        }
        else
        {
            result += EditorGUI.GetPropertyHeight(data);
        }

        return result;
    }
    protected virtual void DrawElement(SerializedProperty property, Rect rect, Color color)
    {
        var prevColor = GUI.color;
        var prevBackgroundColor = GUI.backgroundColor;

        var name = property.FindPropertyRelative("_name");
        var data = property.FindPropertyRelative("_data");

        var nameHeight = EditorGUI.GetPropertyHeight(name);
        var dataHeight = EditorGUI.GetPropertyHeight(data);

        var headerRect = rect;
        headerRect.height = nameHeight;
        GUI.backgroundColor = color;
        GUI.color = color;
        EditorGUI.LabelField(headerRect, name.stringValue);
        GUI.color = prevColor;

        rect.y += headerRect.height;

        if (_unfoldProperties)
        {
            foreach (var innerElement in GetChildren(data))
            {
                rect.height = EditorGUI.GetPropertyHeight(innerElement); ;
                EditorGUI.PropertyField(rect, innerElement, true);
                rect.y += rect.height;
            }
        }
        else
        {

            rect.height = dataHeight;
            EditorGUI.PropertyField(rect, data, true);
        }
        GUI.backgroundColor = prevBackgroundColor;
    }
    private IEnumerable<SerializedProperty> GetChildren(SerializedProperty property)
    {
        property = property.Copy();
        var nextElement = property.Copy();
        bool hasNextElement = nextElement.NextVisible(false);
        if (!hasNextElement)
        {
            nextElement = null;
        }

        property.NextVisible(true);
        while (true)
        {
            if ((SerializedProperty.EqualContents(property, nextElement)))
            {
                yield break;
            }

            yield return property;

            bool hasNext = property.NextVisible(false);
            if (!hasNext)
            {
                break;
            }
        }
    }
}