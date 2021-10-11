using UnityEngine;

namespace Effects.Vignette
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class VignetteGlitch : MonoBehaviour
    {
        [Range(0, 1)] public float BlurArea;
        [Range(0, 1)] public float Blur;
        [Range(0, 100)] public int ChromaticAberration;
        [Range(-1, 1)] public float AberrationMultiplier;

        [SerializeField] private Shader _vignette;
        [SerializeField] private Shader _separateBlur;

        private Material _vignetteMaterial;
        private Material _separableBlurMaterial;
        private Texture2D _noiseTexture;

        private static readonly int BlurId = Shader.PropertyToID("_Blur");
        private static readonly int ChromaticAberrationId = Shader.PropertyToID("_ChromaticAberration");
        private static readonly int ChromaticAberrationMultiplierId = Shader.PropertyToID("_ChromaticAberrationMultiplier");
        private static readonly int VignetteTexId = Shader.PropertyToID("_VignetteTex");
        private static readonly int OffsetsId = Shader.PropertyToID("_Offsets");

        private Material CreateMaterial(Shader shader)
        {
            return new Material(shader);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            _vignetteMaterial ??= CreateMaterial(_vignette);
            _separableBlurMaterial ??= CreateMaterial(_separateBlur);

            int width = source.width;
            int height = source.height;

            RenderTexture blurTex = null;

            if (Mathf.Abs(BlurArea) > 0.0f)
            {
                RenderTexture temp = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
                Graphics.Blit(source, temp, _separableBlurMaterial, 0);

                _separableBlurMaterial.SetVector(OffsetsId, new Vector4(Blur / 200f, 0f, 0f, 0f));
                blurTex = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
                Graphics.Blit(temp, blurTex, _separableBlurMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }

            _vignetteMaterial.SetTexture(VignetteTexId, blurTex);
            _vignetteMaterial.SetFloat(BlurId, 1f / (1f - BlurArea) - 1f);

            _vignetteMaterial.SetFloat(ChromaticAberrationId, ChromaticAberration);
            _vignetteMaterial.SetFloat(ChromaticAberrationMultiplierId, AberrationMultiplier);

            Graphics.Blit(source, destination, _vignetteMaterial);
            RenderTexture.ReleaseTemporary(blurTex);
        }
    }
}