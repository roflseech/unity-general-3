using System.Collections.Generic;
using UnityEngine;

namespace UnityGeneral
{
    [CreateAssetMenu(fileName = "EntryDatabase", menuName = "Data/Shops/EntryDatabase")]
    public class EntryDatabase : AEntryDatabase
    {
        [SerializeField]
        private List<string> _entries = new();

        public override IEnumerable<string> GetAllEntries()
        {
            return _entries;
        }

        public override bool HasEntry(string name)
        {
            return _entries.Contains(name);
        }
    }
}