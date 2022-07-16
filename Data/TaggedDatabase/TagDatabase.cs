using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// ScriptableObject, выолняющий функции Dictionary - хранит пары строка-значение.
/// Может унаследовать значения из InheritFrom - тогда если какого-то значения нет, оно будет взято оттуда.
/// Также содержит статическое поле CurrentInstance, можно его использовать чтобы например указать какой TagDatabase использовать сейчас глобально.
/// При зименении CurrentInstance будет вызвано событие OnDatabaseChanged. 
/// Чтобы использовать класс, нужно наследовать новый класс от него, и далее создавать ScriptableObject'ы
/// </summary>
public abstract class TagDatabase<T> : ScriptableObject
{
    public static event Action<TagDatabase<T>> OnDatabaseChanged;
    private static TagDatabase<T> _instance;
    public static TagDatabase<T> CurrentInstance
    {
        get => _instance;
        set
        {
            _instance = value;
            if (_instance.InheritFrom != null) _instance.InheritFrom.ReinitializeDictionary();
            _instance.ReinitializeDictionary();
        }
    }

    private Dictionary<string, T> _dictionary;
    /// <summary>
    /// Хранит пары строка-значение
    /// </summary>
    protected abstract TagDatabase<T> InheritFrom { get; }
    /// <summary>
    /// Все элементы.
    /// </summary>
    protected abstract IEnumerable<ITaggedObject<T>> GetAllValues();

    public bool TryGetValue(string label, out T result)
    {
        if (_dictionary == null) ReinitializeDictionary();
        if (_dictionary.TryGetValue(label, out result)) return true;
        return (InheritFrom != null && InheritFrom._dictionary.TryGetValue(label, out result));
    }
    private void ReinitializeDictionary()
    {
        _dictionary = new Dictionary<string, T>();

        var values = GetAllValues();
        if (values == null) return;
        foreach (var entry in values)
        {
            _dictionary.Add(string.Intern(entry.Tag), entry.Value);
        }
    }
}