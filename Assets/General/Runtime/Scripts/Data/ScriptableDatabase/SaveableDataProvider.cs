using System;
using System.Collections.Generic;
using UniRx;

namespace UnityGeneral
{
    public class SaveableDataProvider<TSaveableData> : ISaveableDataProvider<TSaveableData> where TSaveableData : struct
    {
        private IEntryDatabase _databae;
        private string _filename;
        private TSaveableData _defaultValue;
        private SaveableDataList _saveableDataList;
        private Dictionary<string, SaveableDataContainer> _saveableDataContainersDictionary;
        private Subject<string> _onUpdated = new();
        private HashSet<string> _availableItems = new();
        private bool _initialized;

        public SaveableDataProvider(
            IEntryDatabase database,
            string filename,
            TSaveableData defaultValue = default)
        {
            _databae = database;
            _filename = filename;
            _defaultValue = defaultValue;
            _saveableDataList = new();
            foreach (var entry in GetAllEntries())
            {
                _availableItems.Add(entry);
            }
        }

        public IEnumerable<string> GetAllEntries()
        {
            return _databae.GetAllEntries();
        }

        public IObservable<string> OnUpdated()
        {
            return _onUpdated;
        }

        public bool Load()
        {
            if (BinarySerializer.HasSaved(_filename))
            {
                _saveableDataList = BinarySerializer.Load<SaveableDataList>(_filename);
            }
            else
            {
                _saveableDataList = new();
                BinarySerializer.Save(_saveableDataList, _filename);
            }
            //false if error?

            _saveableDataContainersDictionary = new();
            foreach (var saveableDataContainer in _saveableDataList.SaveableDataContainers)
            {
                _saveableDataContainersDictionary[saveableDataContainer.Name] = saveableDataContainer;
            }

            _initialized = true;
            return true;
        }

        public void Save()
        {
            BinarySerializer.Save(_saveableDataList, _filename);
        }

        public bool TryGetSaveableData(string name, out TSaveableData saveableData)
        {
            if (!_initialized)
            {
                Load();
            }
            if (!_availableItems.Contains(name))
            {
                saveableData = _defaultValue;
                return false;
            }

            if (_saveableDataContainersDictionary.TryGetValue(name, out var container))
            {
                saveableData = container.SaveableData;
            }
            else
            {
                saveableData = _defaultValue;
            }
            return true;
        }

        public bool TryUpdateSaveableData(string name, TSaveableData saveableData, bool save = true)
        {
            if (!_initialized)
            {
                Load();
            }
            if (!_availableItems.Contains(name))
            {
                return false;
            }

            if (_saveableDataContainersDictionary.TryGetValue(name, out var container))
            {
                container.Update(saveableData);
            }
            else
            {
                var newContainer = new SaveableDataContainer(name, saveableData);
                _saveableDataContainersDictionary[name] = newContainer;
                _saveableDataList.SaveableDataContainers.Add(newContainer);
            }
            if (save)
            {
                Save();
            }
            _onUpdated.OnNext(name);

            return true;
        }

        public TSaveableData GetSaveableData(string name)
        {
            if (!_initialized)
            {
                Load();
            }
            if (!_availableItems.Contains(name))
            {
                throw new Exception($"Can't find entry {name}");
            }

            if (_saveableDataContainersDictionary.TryGetValue(name, out var container))
            {
                return container.SaveableData;
            }
            else
            {
                return _defaultValue;
            }
        }

        public void UpdateSaveableData(string name, TSaveableData saveableData, bool save = true)
        {
            if (!_initialized)
            {
                Load();
            }
            if (!_availableItems.Contains(name))
            {
                throw new Exception($"Can't find entry {name}");
            }

            if (_saveableDataContainersDictionary.TryGetValue(name, out var container))
            {
                container.Update(saveableData);
            }
            else
            {
                var newContainer = new SaveableDataContainer(name, saveableData);
                _saveableDataContainersDictionary[name] = newContainer;
                _saveableDataList.SaveableDataContainers.Add(newContainer);
            }
            if (save)
            {
                Save();
            }
            _onUpdated.OnNext(name);
        }

        public bool HasEntry(string name)
        {
            return _availableItems.Contains(name);
        }

        [Serializable]
        private class SaveableDataContainer
        {
            private string _name;
            private TSaveableData _data;

            public TSaveableData SaveableData => _data;
            public string Name => _name;

            public SaveableDataContainer(string name, TSaveableData data)
            {
                _name = name;
                _data = data;
            }
            public void Update(TSaveableData data)
            {
                _data = data;
            }
        }
        [Serializable]
        private class SaveableDataList
        {
            public readonly List<SaveableDataContainer> SaveableDataContainers = new();
        }
    }
}