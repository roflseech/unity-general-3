using System;
using System.Collections.Generic;

namespace UnityGeneral
{
    public interface IEntryDatabase
    {
        public IEnumerable<string> GetAllEntries();
        public bool HasEntry(string name);
    }
}