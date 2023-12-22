using UnityEngine;

namespace StdNounou
{
    public interface IAudioPlayer
    {
        public void PlayClip(AudioClip clip = null, float volume = -1, float pitch = -1);
        public void PlayClipAt(Vector3 position, AudioClip clip = null, float volume = -1, float pitch = -1);

        public void PlayCue(SO_AudioCue cue);
        public void PlayCueAt(Vector3 position, SO_AudioCue cue);

        public void PlayClipOneShot(AudioClip clip, float volume = -1, float pitch = -1);
        public void PlayClipOneShotAt(Vector3 position, AudioClip clip, float volume = -1, float pitch = -1);

        public void PlayCueOneShot(SO_AudioCue cue);
        public void PlayCueOneShotAt(Vector3 position, SO_AudioCue cue);

        public void Pause();
        public void Resume();


        public void Stop();
    } 
}