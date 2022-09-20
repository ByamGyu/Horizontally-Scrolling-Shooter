using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/IntroSound")]
    // Plays a sound when object is enabled.
    public class IntroSound : MonoBehaviour
    {
        [Tooltip("Sound to play when OnEnable is called. Requires AudioChannels.cs and atleast one Audio Source.")]
        public AudioProperties audioProperties;
        void OnEnable()
        {
            _audioProperties = audioProperties;
        }

        void Update()
        {
            if(_audioProperties != null && AudioChannels.channels != null)
            {
                _audioProperties.Play();

                _audioProperties = null;
            }
        }

        AudioProperties _audioProperties;
    }
}