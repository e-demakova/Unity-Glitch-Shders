using UnityEngine;

namespace Effects.Digital
{
    [RequireComponent(typeof(Camera))]
    public class DigitalGlitch : MonoBehaviour
    {
        [Range(0, 1)] public float Intensity;
        [Range(0, 1)] public float SafeArea;
        [Range(0, 1f)] public float Shift;
        [Range(0, 1)] public float Size;
        [Range(0, 1)] public float UpdateSpeed;

        public Vector2Int TextureSize = new Vector2Int(64, 32);

        [Header("Color Drift")]
        [Range(-1, 1)] public float ColorDrift;
        public bool R;
        public bool G;
        public bool B;

        [SerializeField] private Shader _shader;

        private Material _material;
        private Texture2D _noiseTexture;

        private static readonly int NoiseTexId = Shader.PropertyToID("_NoiseTex");
        private static readonly int ShiftId = Shader.PropertyToID("_Shift");
        private static readonly int SafeAreaId = Shader.PropertyToID("_SafeArea");
        private static readonly int IntensityId = Shader.PropertyToID("_Intensity");
        private static readonly int ColorDriftId = Shader.PropertyToID("_ColorDrift");
        private static readonly int RedId = Shader.PropertyToID("_R");
        private static readonly int GreenId = Shader.PropertyToID("_G");
        private static readonly int BlueId = Shader.PropertyToID("_B");

        private static Color RandomColor()
        {
            return new Color(Random.value, Random.value, Random.value, Random.value);
        }

        private Material CreateMaterial()
        {
            var material = new Material(_shader);
            material.hideFlags = HideFlags.DontSave;
            return material;
        }

        private Texture2D CreateNoiseTexture()
        {
            var texture = new Texture2D(TextureSize.x, TextureSize.y, TextureFormat.ARGB32, false);
            texture.hideFlags = HideFlags.DontSave;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            return texture;
        }

        private void UpdateNoiseTexture()
        {
            var color = RandomColor();

            for (var y = 0; y < _noiseTexture.height; y++)
            {
                for (var x = 0; x < _noiseTexture.width; x++)
                {
                    if (Random.value > Size)
                        color = RandomColor();
                    _noiseTexture.SetPixel(x, y, color);
                }
            }

            _noiseTexture.Apply();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            _material ??= CreateMaterial();
            _noiseTexture ??= CreateNoiseTexture();

            _material.SetFloat(IntensityId, Intensity);
            _material.SetFloat(SafeAreaId, SafeArea);
            _material.SetFloat(ShiftId, Shift / 2);
            _material.SetTexture(NoiseTexId, _noiseTexture);
            _material.SetVector(ColorDriftId, Vector2.one * ColorDrift / 8);
            DefineColors();

            Graphics.Blit(source, destination, _material);
        }

        private void FixedUpdate()
        {
            _material ??= CreateMaterial();
            _noiseTexture ??= CreateNoiseTexture();

            if (Random.value < UpdateSpeed)
                UpdateNoiseTexture();
        }

        private void DefineColors()
        {
            _material.SetInt(RedId, R ? 1 : 0);
            _material.SetInt(GreenId, G ? 1 : 0);
            _material.SetInt(BlueId, B ? 1 : 0);
        }
    }
}