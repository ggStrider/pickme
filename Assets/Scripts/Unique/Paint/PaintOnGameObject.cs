using UnityEngine;

namespace Unique.Paint
{
    public class PaintOnGameObject : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private float _rayDistance = 100f;
        [SerializeField] private Texture2D _paintTexture;
        [SerializeField] private Texture2D _patternTexture;
        [SerializeField] private float _paintThreshold = 0.75f;
        [SerializeField] private float _paintSize = 5f;

        private Texture2D _originalTexture;
        private Texture2D _textureCopy;
        private Color32[] _texturePixels;
        private bool[] _paintedPixelsFlags;
        private Color32[] _patternPixels;

        public event System.Action OnPaintCompleted;

        private void Start()
        {
            ValidateConfiguration();
            if (_paintTexture != null)
            {
                _originalTexture = Instantiate(_paintTexture);
            }
            if (_patternTexture != null)
            {
                _patternPixels = _patternTexture.GetPixels32();
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                PerformRaycast();
            }
        }

        private void PerformRaycast()
        {
            var ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, _rayDistance))
            {
                HandlePainting(hit);
            }
        }

        private void HandlePainting(RaycastHit hit)
        {
            var objRenderer = hit.collider.GetComponent<Renderer>();
            if (objRenderer == null) return;

            var material = objRenderer.material;
            if (material.mainTexture == null) return;

            var texture = (Texture2D)material.mainTexture;

            if (_textureCopy == null || _textureCopy.width != texture.width || _textureCopy.height != texture.height)
            {
                PrepareTextureForPainting(texture, material);
            }

            var uv = hit.textureCoord;
            uv.x *= texture.width;
            uv.y *= texture.height;

            PaintRegion(texture, (int)uv.x, (int)uv.y);
        }

        private void PaintRegion(Texture2D texture, int centerX, int centerY)
        {
            var size = Mathf.FloorToInt(_paintSize);
            var startX = Mathf.Max(centerX - size, 0);
            var endX = Mathf.Min(centerX + size, texture.width);
            var startY = Mathf.Max(centerY - size, 0);
            var endY = Mathf.Min(centerY + size, texture.height);

            for (var x = startX; x < endX; x++)
            {
                for (var y = startY; y < endY; y++)
                {
                    if (_patternTexture.GetPixelBilinear((float)x / texture.width, (float)y / texture.height).a > 0)
                    {
                        PaintIfNotTransparent(texture, x, y);
                    }
                }
            }

            _textureCopy.SetPixels32(0, 0, texture.width, texture.height, _texturePixels);
            _textureCopy.Apply();

            CheckForCompletion(texture);
        }

        private void PaintIfNotTransparent(Texture2D texture, int x, int y)
        {
            var pixelIndex = y * texture.width + x;
            var originalColor = _texturePixels[pixelIndex];

            if (_patternTexture.GetPixelBilinear((float)x / texture.width, (float)y / texture.height).a > 0)
            {
                if (originalColor.a > 0)
                {
                    _texturePixels[pixelIndex] = Color.red;
                    _paintedPixelsFlags[pixelIndex] = true;
                }
            }
        }


        private void CheckForCompletion(Texture2D texture)
        {
            var paintedPixels = 0;
            var totalPatternPixels = 0;

            // Ітеруємо лише по пікселях візерунка
            for (var x = 0; x < texture.width; x++)
            {
                for (var y = 0; y < texture.height; y++)
                {
                    var patternColor = _patternPixels[y * texture.width + x];
                    if (patternColor.a > 0)
                    {
                        totalPatternPixels++;
                        if (_paintedPixelsFlags[y * texture.width + x]) paintedPixels++;
                    }
                }
            }

            float paintedPercentage = totalPatternPixels > 0 ? (float)paintedPixels / totalPatternPixels * 100 : 0f;

            Debug.Log($"Paint progress: {paintedPercentage:F2}%");

            if (totalPatternPixels > 0 && (float)paintedPixels / totalPatternPixels >= _paintThreshold)
            {
                OnObjectPainted();
            }
        }


        private void OnObjectPainted()
        {
            Debug.Log("Object painted!");
            OnPaintCompleted?.Invoke();
        }

        private void PrepareTextureForPainting(Texture2D texture, Material material)
        {
            if (_textureCopy == null || _textureCopy.width != texture.width || _textureCopy.height != texture.height)
            {
                _textureCopy = new Texture2D(texture.width, texture.height, texture.format, texture.mipmapCount > 1);
                Graphics.CopyTexture(texture, _textureCopy);
                _texturePixels = _textureCopy.GetPixels32();
                _paintedPixelsFlags = new bool[texture.width * texture.height];
                material.mainTexture = _textureCopy;
            }
        }

        private void ValidateConfiguration()
        {
            if (_patternTexture == null)
            {
                Debug.LogError("Pattern texture is not assigned!");
            }
        }

        private void OnDisable()
        {
            ResetTexture();
        }

        public void ResetTexture()
        {
            if (_paintTexture != null && _originalTexture != null)
            {
                var material = GetComponent<Renderer>().material;
                material.mainTexture = _originalTexture;
            }

            if (_textureCopy != null)
            {
                Destroy(_textureCopy);
                _textureCopy = null;
                _paintedPixelsFlags = null; // Clean up the flag array
            }
        }
    }
}