using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGeneral
{
    public abstract class AEntityDatabase<TEntity> :
        AEntryDatabase,
        IEntityDatabase<TEntity>
    {
        [SerializeField]
        private List<ItemDefinition> _entries = new();

        public override IEnumerable<string> GetAllEntries()
        {
            foreach (var entry in _entries)
            {
                yield return entry.Name;
            }
        }
        public override bool HasEntry(string name)
        {
            foreach (var entry in _entries)
            {
                if (entry.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public TEntity GetEntity(string name)
        {
            foreach (var entry in _entries)
            {
                if (entry.Name == name)
                {
                    return entry.Entity;
                }
            }
            throw new Exception($"Can't find entity {name}");
        }

        public bool TryGetEntity(string name, out TEntity _entity)
        {
            foreach (var entry in _entries)
            {
                if (entry.Name == name)
                {
                    _entity = entry.Entity;
                    return true;
                }
            }
            _entity = default;
            return false;
        }

        [Serializable]
        public class ItemDefinition
        {
            [SerializeField]
            private string _name;
            [SerializeField]
            private TEntity _entity;

            public string Name => _name;
            public TEntity Entity => _entity;
        }
    }
}