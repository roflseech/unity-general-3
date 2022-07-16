using UnityEngine;
using System;

using Random = UnityEngine.Random;

/// <summary>
/// диапазон значений float.
/// </summary>
[Serializable]
public class FloatRange
{
    [SerializeField]
    private float _min;
    [SerializeField]
    private float _max;

    public float Min => _min;
    public float Max => _max;
    public float Randomized => Random.Range(_min, _max);
    public float RandomizedRandomSign => Random.Range(_min, _max) * RandomHelp.Sign;
}