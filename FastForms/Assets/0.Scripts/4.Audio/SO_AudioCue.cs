using UnityEngine;

namespace StdNounou
{
    [CreateAssetMenu(fileName = "CUE_NewAudioCue", menuName = "Scriptable/Audio/Cue")]
    public class SO_AudioCue : ScriptableObject
    {
        [field: SerializeField] public AudioClip[] Clips { get; private set; }
        [field: SerializeField] public MinMax_Float PitchModifier { get; private set; } = new MinMax_Float(1, 1);
        [field: SerializeField] public MinMax_Float VolumeModifier { get; private set; } = new MinMax_Float(1, 1);
    } 
}