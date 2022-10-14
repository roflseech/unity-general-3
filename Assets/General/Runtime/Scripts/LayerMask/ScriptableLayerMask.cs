using System.Collections.Generic;
using UnityEngine;

namespace UnityGeneral
{
    [CreateAssetMenu(fileName = "ScriptableMask", menuName = "Assets/ScriptableMask")]
    public class ScriptableLayerMask : ScriptableObject, ILayerMask
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
                    UpdateMask();
                }
                return _combinedLayerMask.Mask;
            }
        }
        private void UpdateMask()
        {
            _combinedLayerMask = new CombinedLayerMask();
            _combinedLayerMask.AddLayers(_layerIds);
            _combinedLayerMask.AddLayers(_layerNames);
        }
        private void OnValidate()
        {
            UpdateMask();
        }
    }
}
