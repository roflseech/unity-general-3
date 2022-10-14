using System;
using System.Collections.Generic;
using UnityEngine;


namespace UnityGeneral
{
    //Готовый к использованию пример реализации TagDatabase.

    [CreateAssetMenu(fileName = "GameObjectTagDatabase", menuName = "Assets/GameObjectTagDatabase")]
    public class GameObjectTagDatabase : TagDatabase<GameObject>
    {
        [Serializable]
        public class GameObjectTagEntry : ITaggedObject<GameObject>
        {
            [SerializeField]
            private string _tag;
            [SerializeField]
            private GameObject _value;

            public string Tag => _tag;
            public GameObject Value => _value;
        }
        [SerializeField]
        private GameObjectTagDatabase _inheritFrom;
        [SerializeField]
        private List<GameObjectTagEntry> _values = new List<GameObjectTagEntry>();

        protected override TagDatabase<GameObject> InheritFrom => _inheritFrom;
        protected override IEnumerable<ITaggedObject<GameObject>> GetAllValues() => _values;
    }
}
