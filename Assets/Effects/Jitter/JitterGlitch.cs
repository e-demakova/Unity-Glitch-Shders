using UnityEngine;

namespace Effects.Jitter
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class JitterGlitch : MonoBehaviour
    {
        [SerializeField] private Shader _shader;

        [SerializeField, Range(0, 1)] public float Jitter;
        [SerializeField, Range(0, 1)] public float JitterStep;

        private static readonly int JitterStepId = Shader.PropertyToID("_JitterStep");
        private static readonly int JitterId = Shader.PropertyToID("_ScanLineJitter");

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
            _material.SetFloat(JitterId, Jitter);
            _material.SetFloat(JitterStepId, JitterStep);
        }
    }
}