using System;
using System.Collections;
using Effects.ColorAberration;
using Effects.Digital;
using UnityEngine;

namespace Effects
{
    [DefaultExecutionOrder(-990)]
    public class MainCamera : MonoBehaviour
    {

        [Header("Links")]
        public Camera Camera;

        [SerializeField] private NoPropertyShader _grayscale;
        [SerializeField] private ColorGlitch _color;
        [SerializeField] private DigitalGlitch _digitalA;
        [SerializeField] private DigitalGlitch _digitalB;
        [SerializeField] private float _goToNormalDuration;

        [Header("Settings")]
        [SerializeField] private EffectsValues _grayscaleValues;
        [SerializeField] private EffectsValues _extraGlitchValues;
        private EffectsValues _normalValues;
        
        private void Awake()
        {
            _normalValues = ScriptableObject.CreateInstance<EffectsValues>();
            _normalValues.Init(_color, _digitalA, _digitalB, _goToNormalDuration);
        }

        public void SetNormal()
        {
            _grayscale.enabled = false;

            SetValues(_normalValues);
        }

        public void SetGrayscale()
        {
            _grayscale.enabled = true;
            
            SetValues(_grayscaleValues);
        }

        public void SetExtraGlitch()
        {
            _grayscale.enabled = false;

            SetValues(_extraGlitchValues);
        }

        private void SetValues(EffectsValues values)
        {
            StopAllCoroutines();
            
            StartCoroutine(SetColorValues(values.Color, _color, values.Duration));
            StartCoroutine(SetDigitalValues(values.DigitalA, _digitalA, values.Duration));
            StartCoroutine(SetDigitalValues(values.DigitalB, _digitalB, values.Duration));
        }

        private IEnumerator SetColorValues(ColorGlitch.Values source, ColorGlitch destination, float duration)
        {
            float time = 0;
            while (time < 1)
            {
                time += Time.fixedDeltaTime / duration;

                LerpValues(source, destination, time);

                yield return new WaitForFixedUpdate();
            }

            SetValues(source, destination);
        }

        private static void SetValues(ColorGlitch.Values source, ColorGlitch destination)
        {
            destination.Settings.DriftDirection = source.DriftDirection;
            destination.Settings.R = source.R;
            destination.Settings.G = source.G;
            destination.Settings.B = source.B;
        }

        private static void LerpValues(ColorGlitch.Values source, ColorGlitch destination, float time)
        {
            destination.Settings.ColorDrift = Mathf.Lerp(destination.Settings.ColorDrift, source.ColorDrift, time);
            destination.Settings.Distortion = (int) Mathf.Lerp(destination.Settings.Distortion, source.Distortion, time);
            destination.Settings.DriftSpeed = Mathf.Lerp(destination.Settings.DriftSpeed, source.DriftSpeed, time);
        }

        private IEnumerator SetDigitalValues(DigitalGlitch.Values source, DigitalGlitch destination, float duration)
        {
            float time = 0;
            while (time < 1)
            {
                time += Time.fixedDeltaTime / duration;

                LerpValues(source, destination, time);

                yield return new WaitForFixedUpdate();
            }

            SetValues(source, destination);
        }

        private static void SetValues(DigitalGlitch.Values source, DigitalGlitch destination)
        {
            destination.Settings.R = source.R;
            destination.Settings.G = source.G;
            destination.Settings.B = source.B;
        }

        private static void LerpValues(DigitalGlitch.Values source, DigitalGlitch destination, float time)
        {
            destination.Settings.Intensity = Mathf.Lerp(destination.Settings.Intensity, source.Intensity, time);
            destination.Settings.Shift = Mathf.Lerp(destination.Settings.Shift, source.Shift, time);
            destination.Settings.Size = Mathf.Lerp(destination.Settings.Size, source.Size, time);
            destination.Settings.ColorDrift = Mathf.Lerp(destination.Settings.ColorDrift, source.ColorDrift, time);
            destination.Settings.UpdateSpeed = Mathf.Lerp(destination.Settings.UpdateSpeed, source.UpdateSpeed, time);
            destination.Settings.SafeArea = Mathf.Lerp(destination.Settings.SafeArea, source.SafeArea, time);
        }
    }
}