using System;
using System.Collections.Generic;

namespace UnityGeneral
{
    public interface IDataProvider<TData> : IEntryDatabase
    {
        public bool TryGetData(string name, out TData data);
        public TData GetData(string name);
    }
}