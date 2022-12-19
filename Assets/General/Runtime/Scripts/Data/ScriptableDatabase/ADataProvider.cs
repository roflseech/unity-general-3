using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UnityGeneral
{
    public abstract class ADataProvider<TData> :
        ScriptableObject,
        IDataProvider<TData>
    {
        [SerializeField]
        private AEntryDatabase _database;
        [SerializeField]
        private List<ItemDefinition> _entries = new();

        public AEntryDatabase Database => _database;

        public IEnumerable<string> GetAllEntries()
        {
            return _database.GetAllEntries();
        }
        public bool HasEntry(string name)
        {
            return _database.HasEntry(name);
        }


        public TData GetData(string name)
        {
            foreach (var entry in _entries)
            {
                if (entry.Name == name)
                {
                    return entry.Data;
                }
            }
            throw new Exception($"Can't find entry for {name}");
        }


        public bool TryGetData(string name, out TData data)
        {
            foreach (var entry in _entries)
            {
                if (entry.Name == name)
                {
                    data = entry.Data;
                    return true;
                }
            }

            data = default;
            return false;
        }

        public void UpdateEntries()
        {
            if (_database == null)
            {
                _entries.Clear();
                return;
            }

            var entryNames = GetAllEntries().ToList();
            //remove deleted entries
            for (int i = 0; i < _entries.Count; i++)
            {
                if (!entryNames.Contains(_entries[i].Name))
                {
                    _entries.RemoveAt(i);
                    i--;
                }
            }
            //add new entries
            var oldEntryNames = _entries.Select(entry => entry.Name).ToList();
            var newEntries = entryNames
                .Where(entry => !oldEntryNames.Contains(entry))
                .ToList();

            foreach (var entry in newEntries)
            {
                _entries.Add(new ItemDefinition(entry, default));
            }
        }

        private void OnEnable()
        {
            UpdateEntries();
        }

        [Serializable]
        public class ItemDefinition
        {
            [SerializeField]
            private string _name;
            [SerializeField]
            private TData _data;

            public string Name => _name;
            public TData Data => _data;

            public ItemDefinition()
            {

            }
            public ItemDefinition(string name, TData data)
            {
                _name = name;
                _data = data;
            }
        }
    }
}