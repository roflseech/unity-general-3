using UnityEngine;
using System;

using Random = UnityEngine.Random;

namespace UnityGeneral
{

    /// <summary>
    /// Диапазон значений int.
    /// </summary>
    [Serializable]
    public class IntRange
    {
        [SerializeField]
        private int _min;
        [SerializeField]
        private int _max;

        public int Min => _min;
        public int Max => _max;
        public int Randomized => Random.Range(_min, _max);
        public int RandomizedRandomSign => Random.Range(_min, _max) * RandomHelp.Sign;
    }
}