using System;
using System.Collections.Generic;

namespace UnityGeneral
{
    public interface IEntityDatabase<TEntity> : IEntryDatabase
    {
        public bool TryGetEntity(string name, out TEntity entity);
        public TEntity GetEntity(string name);
    }
}