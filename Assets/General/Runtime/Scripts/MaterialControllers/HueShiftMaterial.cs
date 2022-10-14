using UnityEngine;
using UnityEngine.U2D;

namespace UnityGeneral
{
    public class HueShiftMaterial : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _renderer;
        [SerializeField]
        private SpriteShapeRenderer _spriteShapeRenderer;

        [SerializeField]
        private float _currentHue;

        private int _huePropertyId;
        private MaterialPropertyBlock _materialPropertyBlock;
        public float CurrentHue
        {
            get => _currentHue;
            set
            {
                _materialPropertyBlock.SetFloat(_huePropertyId, _currentHue);
                if (_renderer != null) _renderer.SetPropertyBlock(_materialPropertyBlock);
                if (_spriteShapeRenderer) _spriteShapeRenderer.SetPropertyBlock(_materialPropertyBlock, 1);
                _currentHue = value;
            }
        }
        private void Awake()
        {
            _huePropertyId = Shader.PropertyToID("_Hue");
            _materialPropertyBlock = new();

            if (_renderer != null) _renderer.GetPropertyBlock(_materialPropertyBlock);
            if (_spriteShapeRenderer) _spriteShapeRenderer.GetPropertyBlock(_materialPropertyBlock, 1);
        }
    }
}