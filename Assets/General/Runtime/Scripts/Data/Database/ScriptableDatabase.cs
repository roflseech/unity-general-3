using System;
using System.Collections.Generic;
using UnityEngine;

// Для работы необходим https://github.com/herbou/Unity_GenericBinarySerializer

namespace UnityGeneral
{
    /// <summary>
    /// Базовый класс для баз данных, в которых содержатся элементы и их состояние - разблокированы они или нет.
    /// Каждый элемент имеет название и список вариантов(но вариант может быть и один).
    /// Состояние автоматически сохраняется во внешний файл, который задается в Save Filename.
    /// Чтобы использовать класс, нужно наследовать новый класс от него, и далее создавать ScriptableObject'ы
    /// </summary>
    public class ScriptableDatabase<T> : ScriptableObject where T : class
    {
        [SerializeField]
        private string _saveFilename;

        [SerializeField]
        private List<DatabaseEntry> _items = new List<DatabaseEntry>();

        private List<UnlockEntry> _unlockedItems;

        private Dictionary<string, DatabaseEntry> _itemsDictionary;
        private Dictionary<string, UnlockEntry> _unlockedItemsDictionary;

        private Dictionary<string, DatabaseEntry> ItemsDictionary
        {
            get
            {
                if (_itemsDictionary == null)
                {
                    _itemsDictionary = new Dictionary<string, DatabaseEntry>();
                    foreach (var item in _items)
                    {
                        _itemsDictionary.Add(item.Name, item);
                    }
                }
                return _itemsDictionary;
            }
        }
        private Dictionary<string, UnlockEntry> UnlockedItemsDictionary
        {
            get
            {
                if (_unlockedItemsDictionary == null)
                {
                    _unlockedItemsDictionary = new Dictionary<string, UnlockEntry>();
                    foreach (var item in _unlockedItems)
                    {
                        _unlockedItemsDictionary.Add(item.Name, item);
                    }
                }
                return _unlockedItemsDictionary;
            }
        }
        /// <summary>
        /// Все элементы.
        /// </summary>
        public IReadOnlyList<DatabaseEntry> Items => _items;
        /// <summary>
        /// Выбрать элемент по строке.
        /// </summary>
        public DatabaseEntry GetEntry(string name)
        {
            if (_itemsDictionary.TryGetValue(name, out var entry)) return entry;
            return null;
        }
        private void LoadIfNeeded()
        {
            if (_unlockedItems == null) Load();
        }

        /// <summary>
        /// Открывает элемент name вариант variant, если saveResult = true, то сохраняет в файл.
        /// </summary>
        public bool Unlock(string name, int variant, bool saveResult)
        {
            LoadIfNeeded();
            if (ItemsDictionary.TryGetValue(name, out var entry))
            {
                if (entry.GetVariant(variant) != null)
                {
                    if (UnlockedItemsDictionary.TryGetValue(name, out var unlockEntry))
                    {
                        unlockEntry.Unlock(variant);
                    }
                    else
                    {
                        var newUnlockEntry = new UnlockEntry();
                        newUnlockEntry.Name = name;
                        newUnlockEntry.BoughtVariants = new List<int>();
                        newUnlockEntry.BoughtVariants.Add(variant);
                        _unlockedItems.Add(newUnlockEntry);
                        UnlockedItemsDictionary.Add(name, newUnlockEntry);
                    }
                    if (saveResult) Save();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Получается эелмент name, вариант variant.
        /// </summary>
        public T GetItem(string name, int variant)
        {
            if (ItemsDictionary.TryGetValue(name, out var entry))
            {
                return entry.GetVariant(variant);
            }
            return null;
        }
        /// <summary>
        /// Разблокирован ли элемент.
        /// </summary>
        public bool IsUnlocked(string name, int variant)
        {
            LoadIfNeeded();

            if (UnlockedItemsDictionary.TryGetValue(name, out var boughtItem))
            {
                return boughtItem.IsUnlocked(variant);
            }
            return false;
        }
        /// <summary>
        /// Сохранение.
        /// </summary>
        public void Save()
        {
            BinarySerializer.Save(_unlockedItems, _saveFilename);
        }
        /// <summary>
        /// Загрузка.
        /// </summary>
        public void Load()
        {
            if (BinarySerializer.HasSaved(_saveFilename))
            {
                _unlockedItems = BinarySerializer.Load<List<UnlockEntry>>(_saveFilename);
                //при дальнейшем обращении через проперти будет переинициализирован
                _unlockedItemsDictionary = null;
            }
            else
            {
                _unlockedItems = new List<UnlockEntry>();
            }
        }
        public void Reinitialize()
        {
            _unlockedItems = null;
            _itemsDictionary = null;
            _unlockedItemsDictionary = null;
        }

        [Serializable]
        public class DatabaseEntry
        {
            [SerializeField]
            private string _name;
            [SerializeField]
            private Sprite _icon;
            [SerializeField]
            private List<T> _variants;

            public string Name => _name;
            public Sprite Icon => _icon;
            public T GetVariant(int index) => index < _variants.Count ? _variants[index] : null;
            public int VariantsCount => _variants.Count;
        }
        [Serializable]
        public class UnlockEntry
        {
            public string Name;
            public List<int> BoughtVariants;

            public bool IsUnlocked(int variant)
            {
                return BoughtVariants.Contains(variant);
            }
            public void Unlock(int variant)
            {
                if (!IsUnlocked(variant)) BoughtVariants.Add(variant);
            }
        }
    }
}
