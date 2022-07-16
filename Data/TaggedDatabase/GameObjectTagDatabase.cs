using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//Готовый к использованию пример реализации TagDatabase.

[CreateAssetMenu(fileName = "GameObjectTagDatabase", menuName = "Assets/GameObjectTagDatabase")]
public class GameObjectTagDatabase : TagDatabase<GameObject>
{
    [Serializable]
    public class GameObjectTagEntry : ITaggedObject<GameObject>
    {
        [SerializeField]
        private string _tag;
        [SerializeField]
        private GameObject _value;

        public string Tag => _tag;
        public GameObject Value => _value;
    }
    [SerializeField]
    private GameObjectTagDatabase _inheritFrom;
    [SerializeField]
    private List<GameObjectTagEntry> _values = new List<GameObjectTagEntry>();

    protected override TagDatabase<GameObject> InheritFrom => _inheritFrom;
    protected override IEnumerable<ITaggedObject<GameObject>> GetAllValues() => _values;
}
#if UNITY_EDITOR
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
#endif