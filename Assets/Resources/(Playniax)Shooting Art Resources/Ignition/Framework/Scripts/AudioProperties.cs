using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [System.Serializable]
    // AudioProperties datatype.

    // (depends on AudioChannels class)
    public class AudioProperties
    {
        public static bool mute = false;

        [Tooltip("AudioClip to play.")]
        public AudioClip audioClip;
        [Tooltip("Sound volume.")]
        public float volumeScale = 1f;
        public float panStereo = 0;
        [Tooltip("Sound frequency.")]
        public float pitch = 1;
        [Tooltip("Sound on / off.")]
        public bool enabled = true;

        // Copy the properties of one to the other.
        public static void Copy(AudioProperties source, AudioProperties target)
        {
            target.audioClip = source.audioClip;
            target.volumeScale = source.volumeScale;
            target.panStereo = source.panStereo;
            target.pitch = source.pitch;
            target.enabled = source.enabled;
        }

        // Returns a clone with the same properties.
        public AudioProperties Clone()
        {
            var clone = new AudioProperties();

            clone.audioClip = audioClip;
            clone.volumeScale = volumeScale;
            clone.panStereo = panStereo;
            clone.pitch = pitch;
            clone.enabled = enabled;

            return clone;
        }

        // Play AudioClip.
        public AudioSource Play()
        {
            if (mute == false && enabled == true) return AudioChannels.Play(audioClip, volumeScale, panStereo, pitch);

            return null;
        }
    }
}
