using System;

namespace UnityGeneral
{
    public interface ISaveableDataProvider<TSaveableData> : IEntryDatabase
    {
        public IObservable<string> OnUpdated();
        public bool TryGetSaveableData(string name, out TSaveableData saveableData);
        public bool TryUpdateSaveableData(string name, TSaveableData saveableData, bool save = true);
        public TSaveableData GetSaveableData(string name);
        public void UpdateSaveableData(string name, TSaveableData saveableData, bool save = true);
        public void Save();
        public bool Load();
    }
}