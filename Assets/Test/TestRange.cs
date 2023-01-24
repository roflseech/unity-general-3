using System.Collections.Generic;
using UnityEngine;
using UnityGeneral;

[CreateAssetMenu(fileName = "TestRange", menuName = "Test/TestRange")]
public class TestRange : ScriptableObject
{
    [SerializeField]
    private List<IntRange> _values = new();
}
