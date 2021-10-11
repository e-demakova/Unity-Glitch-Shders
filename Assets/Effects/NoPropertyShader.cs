using UnityEngine;

namespace Effects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class NoPropertyShader : MonoBehaviour
    {
        [SerializeField] private Shader _shader;

        private Material _material;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_material == null)
                CreateMaterial();

            Graphics.Blit(source, destination, _material);
        }

        private void CreateMaterial()
        {
            _material = new Material(_shader);
            _material.hideFlags = HideFlags.DontSave;
        }
    }
}