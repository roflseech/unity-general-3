using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Упрощает работу с масками слоев - позволяет комбинировать номера и названия в LayerMask.
/// </summary>
public class CombinedLayerMask : ILayerMask
{
    private int _mask;
    public int Mask => _mask;

    public void AddLayers(params string[] layers)
    {
        foreach (var layer in layers)
        {
            _mask |= 1 << LayerMask.NameToLayer(layer);
        }
    }
    public void AddLayers(IEnumerable<string> layers)
    {
        foreach (var layer in layers)
        {
            _mask |= 1 << LayerMask.NameToLayer(layer);
        }
    }
    public void AddLayers(params int[] layers)
    {
        foreach (var layer in layers)
        {
            _mask |= 1 << layer;
        }
    }
    public void AddLayers(IEnumerable<int> layers)
    {
        foreach (var layer in layers)
        {
            _mask |= 1 << layer;
        }
    }
    public bool HasLayer(int layer)
    {
        return (_mask & layer) != 0;
    }
    public bool HasLayer(string layer)
    {
        int layerId = LayerMask.NameToLayer(layer);
        return (_mask & layerId) != 0;
    }
}