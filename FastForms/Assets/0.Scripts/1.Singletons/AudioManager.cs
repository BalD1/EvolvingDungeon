using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;

namespace StdNounou
{
    public class AudioManager : Singleton<AudioManager>
    {
        [field: SerializeField] public SerializedDictionary<E_SFXIDs, SO_AudioCue> SFXAudioCues {  get; private set; }
        [field: SerializeField] public SerializedDictionary<E_MusicIDs, SO_AudioCue> MusicAudioCues {  get; private set; }

        [field: SerializeField] public AudioMixer MainMixer { get; private set; }

        [SerializeField] private GameObject audioSourcePF;
        [SerializeField] private Transform audioSourcesPoolContainer;

        private int sourcesPoolInitialCount;

        #region MixerParams

        public const string MASTER_PITCH_ID = "MasterPitch";
        public const string MASTER_VOLUME_ID = "MasterVolume";

        public const string MUSIC_PITCH_ID = "MusicPitch";
        public const string MUSIC_VOLUME_ID = "MusicVolume";

        public const string SFX_PITCH_ID = "SFXPitch";
        public const string SFX_VOLUME_ID = "SFXVolume"; 

        #endregion

        public enum E_SFXIDs
        {

        }

        public enum E_MusicIDs
        {

        }

        protected override void EventsSubscriber()
        {
        }

        protected override void EventsUnSubscriber()
        {
        }

        #region SetParam

        public void SetMixerParam(string paramID, float value)
            => MainMixer.SetFloat(paramID, value);

        public void SetMasterVolume(float volume)
            => SetMixerParam(MASTER_VOLUME_ID, volume);
        public void SetMasterPitch(float pitch)
            => SetMixerParam(MASTER_PITCH_ID, pitch);

        public void SetSFXVolume(float volume)
            => SetMixerParam(SFX_VOLUME_ID, volume);
        public void SetSFXPitch(float pitch)
            => SetMixerParam(SFX_PITCH_ID, pitch);

        public void SetMusicVolume(float volume)
            => SetMixerParam(MUSIC_VOLUME_ID, volume);
        public void SetMusicPitch(float volume)
            => SetMixerParam(MUSIC_PITCH_ID, volume); 

        #endregion
    }
}