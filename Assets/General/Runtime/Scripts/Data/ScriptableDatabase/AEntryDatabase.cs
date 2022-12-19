using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGeneral
{
    public abstract class AEntryDatabase : ScriptableObject, IEntryDatabase
    {
        public abstract IEnumerable<string> GetAllEntries();

        public abstract bool HasEntry(string name);
    }
}