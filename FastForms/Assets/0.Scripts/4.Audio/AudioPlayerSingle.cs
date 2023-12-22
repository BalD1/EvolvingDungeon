using System;
using System.Collections;
using UnityEngine;

namespace StdNounou
{
    public class AudioPlayerSingle : MonoBehaviour, IAudioPlayer
    {
        [field: SerializeField] public AudioSource Source { get; private set; }

        public event Action<AudioPlayerSingle> OnClipEnded;

        private Coroutine runningCoroutine;

        public void PlayClip(AudioClip clip = null, float volume = -1, float pitch = -1)
            => PlayClipAt(this.transform.position, clip, volume, pitch);
        public void PlayClipAt(Vector3 position, AudioClip clip = null, float volume = -1, float pitch = -1)
        {
            StopCoroutineIfIsRunning();

            this.transform.position = position;
            if (volume != -1) Source.volume = volume;
            if (pitch != -1) Source.pitch = pitch;
            if (clip != null) Source.clip = clip;
            if (clip == null)
            {
                this.LogError("Source clip was null.");
                return;
            }

            Source.Play();
            runningCoroutine = StartCoroutine(AudioPlayCoroutine(clip.length / pitch));
        }

        public void PlayCue(SO_AudioCue cue)
            => PlayCueAt(this.transform.position, cue);
        public void PlayCueAt(Vector3 position, SO_AudioCue cue)
        {
            if (cue == null || cue.Clips.Length == 0)
            {
                this.LogError("Tried playing null or empty cue.");
                return;
            }
            StopCoroutineIfIsRunning();
            this.transform.position = position;

            Source.volume = cue.VolumeModifier.GetRandomInRange();
            Source.pitch = cue.PitchModifier.GetRandomInRange();
            Source.clip = cue.Clips.RandomElement();
            if (Source.clip == null)
            {
                this.LogError("Source clip was null.");
                return;
            }

            Source.Play();
            runningCoroutine = StartCoroutine(AudioPlayCoroutine(Source.clip.length / Source.pitch));
        }

        public void PlayClipOneShot(AudioClip clip, float volume = -1, float pitch = -1)
            => PlayClipOneShotAt(this.transform.position, clip, volume, pitch);
        public void PlayClipOneShotAt(Vector3 position, AudioClip clip, float volume = -1, float pitch = -1)
        {
            StopCoroutineIfIsRunning();
            this.transform.position = position;
            float finalVol = volume != -1 ? Source.volume = volume : Source.volume;
            if (pitch != -1) Source.pitch = pitch;

            Source.PlayOneShot(clip, finalVol);
            runningCoroutine = StartCoroutine(AudioPlayCoroutine(clip.length /  pitch));
        }

        public void PlayCueOneShot(SO_AudioCue cue)
            => PlayCueOneShotAt(this.transform.position, cue);
        public void PlayCueOneShotAt(Vector3 position, SO_AudioCue cue)
        {
            if (cue == null || cue.Clips.Length == 0)
            {
                this.LogError("Tried playing null or empty cue");
                return;
            }
            StopCoroutineIfIsRunning();
            this.transform.position = position;

            Source.pitch = cue.PitchModifier.GetRandomInRange();
            AudioClip clip = cue.Clips.RandomElement();
            if (clip == null)
            {
                this.LogError("Clip was null.");
                return;
            }

            Source.PlayOneShot(clip, cue.VolumeModifier.GetRandomInRange());
            runningCoroutine = StartCoroutine(AudioPlayCoroutine(clip.length / Source.pitch));
        }

        public void Pause()
        {
            Source.Pause();
        }

        public void Resume()
        {
            Source.UnPause();
        }

        public void Stop()
        {
            Source.Stop();
        }

        private void StopCoroutineIfIsRunning()
        {
            if (runningCoroutine == null) return;
            StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }

        private IEnumerator AudioPlayCoroutine(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);
            OnClipEnded?.Invoke(this);
            runningCoroutine = null;
        }
    } 
}
