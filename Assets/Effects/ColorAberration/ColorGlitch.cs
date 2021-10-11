using System;
using UnityEngine;

namespace Effects.ColorAberration
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class ColorGlitch : MonoBehaviour
    {
        public enum Direction
        {
            Horizontal,
            Vertical,
            Diagonal
        }

        [Serializable]
        public struct Values
        {
            [Range(0, 0.5f)] public float ColorDrift;
            [Range(0, 50)] public int Distortion;
            [Range(0, 50)] public float DriftSpeed;
            public Direction DriftDirection;

            [Header("Drifted colors")]
            public bool R;
            public bool G;
            public bool B;
        }

        public Values Settings;
        
        private static readonly int IsVertical = Shader.PropertyToID("_IsVertical");
        private static readonly int IsHorizontal = Shader.PropertyToID("_IsHorizontal");
        private static readonly int ColorDriftId = Shader.PropertyToID("_ColorDrift");
        private static readonly int DistortionId = Shader.PropertyToID("_Distortion");
        private static readonly int RedId = Shader.PropertyToID("_R");
        private static readonly int GreenId = Shader.PropertyToID("_G");
        private static readonly int BlueId = Shader.PropertyToID("_B");

        [SerializeField] private Shader _shader;

        private Material _material;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_material == null)
                CreateMaterial();

            RenderEffects();
            Graphics.Blit(source, destination, _material);
        }

        private void CreateMaterial()
        {
            _material = new Material(_shader);
            _material.hideFlags = HideFlags.DontSave;
        }

        private void RenderEffects()
        {
            DefineDirection();
            DefineColors();

            var colorDriftVector = new Vector2(Settings.ColorDrift * 0.04f, Time.time * Settings.DriftSpeed);
            _material.SetVector(ColorDriftId, colorDriftVector);
            _material.SetInt(DistortionId, Settings.Distortion);
        }

        private void DefineDirection()
        {
            switch (Settings.DriftDirection)
            {
                case Direction.Horizontal:
                    _material.SetInt(IsHorizontal, 1);
                    _material.SetInt(IsVertical, 0);
                    break;
                case Direction.Vertical:
                    _material.SetInt(IsHorizontal, 0);
                    _material.SetInt(IsVertical, 1);
                    break;
                case Direction.Diagonal:
                    _material.SetInt(IsHorizontal, 1);
                    _material.SetInt(IsVertical, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DefineColors()
        {
            _material.SetInt(RedId, Settings.R ? 1 : 0);
            _material.SetInt(GreenId, Settings.G ? 1 : 0);
            _material.SetInt(BlueId, Settings.B ? 1 : 0);
        }
    }
}