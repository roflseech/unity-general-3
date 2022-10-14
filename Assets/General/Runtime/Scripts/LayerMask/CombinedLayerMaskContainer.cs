using UnityEngine;
using System.Collections.Generic;
using System;

namespace UnityGeneral
{
    /// <summary>
    /// Хранит номера и навзания слоев, позволяет конвертировать их в маску слоев.
    /// </summary>
    [Serializable]
    public class CombinedLayerMaskContainer : ILayerMask
    {
        [SerializeField]
        private List<int> _layerIds = new List<int>();
        [SerializeField]
        private List<string> _layerNames = new List<string>();

        private CombinedLayerMask _combinedLayerMask;
        public int Mask
        {
            get
            {
                if (_combinedLayerMask == null)
                {
                    _combinedLayerMask = new CombinedLayerMask();
                    _combinedLayerMask.AddLayers(_layerIds);
                    _combinedLayerMask.AddLayers(_layerNames);
                }
                return _combinedLayerMask.Mask;
            }
        }
    }
}
