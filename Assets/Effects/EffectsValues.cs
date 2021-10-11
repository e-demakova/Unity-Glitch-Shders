using Effects.ColorAberration;
using Effects.Digital;
using UnityEngine;

namespace Effects
{
    [CreateAssetMenu(fileName = "NewEffectsValues", menuName = "Effects Values")]
    public class EffectsValues : ScriptableObject
    {
        public ColorGlitch.Values Color;
        public DigitalGlitch.Values DigitalA;
        public DigitalGlitch.Values DigitalB;
        public float Duration;

        public void Init(ColorGlitch color, DigitalGlitch digitalA, DigitalGlitch digitalB, float duration)
        {
            Color = color.Settings;
            DigitalA = digitalA.Settings;
            DigitalB = digitalB.Settings;
            Duration = duration;
        }
    }
}