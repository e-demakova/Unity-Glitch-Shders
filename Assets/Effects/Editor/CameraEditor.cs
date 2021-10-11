using UnityEditor;
using UnityEngine;

namespace Effects
{
    [CustomEditor(typeof(MainCamera))]
    public class CameraEditor : Editor
    {
        private MainCamera _target;

        public override void OnInspectorGUI()
        {
            _target = (MainCamera) target;
            base.OnInspectorGUI();

            if (GUILayout.Button("Go to Normal"))
            {
                _target.SetNormal();
            }

            if (GUILayout.Button("Go to Grayscale"))
            {
                _target.SetGrayscale();
            }

            if (GUILayout.Button("Go to ExtraGlitch"))
            {
                _target.SetExtraGlitch();
            }
        }
    }
}