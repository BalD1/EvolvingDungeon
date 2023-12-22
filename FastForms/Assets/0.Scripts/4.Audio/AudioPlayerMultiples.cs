using UnityEngine;

namespace StdNounou
{
    public class AudioPlayerMultiples : MonoBehaviour, IAudioPlayer
    {
        [SerializeField] private Transform poolContainer;
        [SerializeField] private int initialCount;

        private PoolableObject<AudioPlayerSingle> sourcesPool;

        private void Awake()
        {
            GameObject audioSourcePF = ResourcesObjectLoader.GetAudioPrefabsHolder().GetAsset("SourcePF");
            sourcesPool = new PoolableObject<AudioPlayerSingle>(_createAction: () => audioSourcePF.Create<AudioPlayerSingle>(), poolContainer, initialCount);
        }

        public void PlayClip(AudioClip clip = null, float volume = -1, float pitch = -1)
            => PlayClipAt(this.transform.position, clip, volume, pitch);
        public void PlayClipAt(Vector3 position, AudioClip clip = null, float volume = -1, float pitch = -1)
        {
            AudioPlayerSingle player = sourcesPool.GetNext(position);
            player.OnClipEnded += OnSourceEndedPlaying;
            player.PlayClipAt(position, clip, volume, pitch);
        }

        public void PlayCue(SO_AudioCue cue)
            => PlayCueAt(this.transform.position, cue);
        public void PlayCueAt(Vector3 position, SO_AudioCue cue)
        {
            if (cue == null)
            {
                this.LogError("Tried playing null cue.");
                return;
            }

            AudioPlayerSingle player = sourcesPool.GetNext(position);
            player.OnClipEnded += OnSourceEndedPlaying;
            player.PlayCueAt(position, cue);
        }

        public void PlayClipOneShot(AudioClip clip, float volume = -1, float pitch = -1)
            => PlayClipOneShotAt(this.transform.position, clip, volume, pitch);
        public void PlayClipOneShotAt(Vector3 position, AudioClip clip, float volume = -1, float pitch = -1)
        {
            AudioPlayerSingle player = sourcesPool.GetNext(position);
            player.OnClipEnded += OnSourceEndedPlaying;
            player.PlayClipOneShotAt(position, clip, volume, pitch);
        }

        public void PlayCueOneShot(SO_AudioCue cue)
            => PlayCueOneShotAt(this.transform.position, cue);
        public void PlayCueOneShotAt(Vector3 position, SO_AudioCue cue)
        {
            if (cue == null)
            {
                this.LogError("Tried playing null cue.");
                return;
            }

            AudioPlayerSingle player = sourcesPool.GetNext(position);
            player.OnClipEnded += OnSourceEndedPlaying;
            player.PlayCueOneShotAt(position, cue);
        }

        public void Pause()
        {
        }

        public void Resume()
        {
        }

        public void Stop()
        {
        }

        private void OnSourceEndedPlaying(AudioPlayerSingle player)
        {
            sourcesPool.Enqueue(player);
        }
    } 
}
