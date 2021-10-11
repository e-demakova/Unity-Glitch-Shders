using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Effects.Digital
{
    [RequireComponent(typeof(Camera))]
    public class DigitalGlitch : MonoBehaviour
    {
        [Serializable]
        public struct Values
        {
            [Range(0, 1)] public float Intensity;
            [Range(0, 1)] public float SafeArea;
            [Range(0, 1)] public float Shift;
            [Range(0, 1)] public float Size;
            [Range(0, 1)] public float UpdateSpeed;

            [Header("Color Drift")]
            [Range(-1, 1)] public float ColorDrift;
            public bool R;
            public bool G;
            public bool B;
        }

        public Values Settings;

        [SerializeField] private Vector2Int _textureSize;
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
            var texture = new Texture2D(_textureSize.x, _textureSize.y, TextureFormat.ARGB32, false);
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
                    if (Random.value > Settings.Size)
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

            _material.SetFloat(IntensityId, Settings.Intensity);
            _material.SetFloat(SafeAreaId, Settings.SafeArea);
            _material.SetFloat(ShiftId, Settings.Shift / 2);
            _material.SetTexture(NoiseTexId, _noiseTexture);
            _material.SetVector(ColorDriftId, Vector2.one * Settings.ColorDrift / 8);
            DefineColors();

            Graphics.Blit(source, destination, _material);
        }

        private void FixedUpdate()
        {
            _material ??= CreateMaterial();
            _noiseTexture ??= CreateNoiseTexture();

            if (Random.value < Settings.UpdateSpeed)
                UpdateNoiseTexture();
        }

        private void DefineColors()
        {
            _material.SetInt(RedId, Settings.R ? 1 : 0);
            _material.SetInt(GreenId, Settings.G ? 1 : 0);
            _material.SetInt(BlueId, Settings.B ? 1 : 0);
        }
    }
}