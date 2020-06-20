using UnityEngine;

namespace Scripts.Audio
{
    /// <summary>
    /// Handles the SFX in game.
    /// </summary>
    public class SFXManager : MonoBehaviour
    {
        private AudioSource aSrc;
        [SerializeField] private AudioClip _thrust, _crash;

        public AudioClip Crash { get => _crash; set => _crash = value; }
        public AudioClip Thrust { get => _thrust; set => _thrust = value; }

        void Start() => aSrc = GetComponent<AudioSource>();

        /// <summary>
        /// Assign the passed AudioClip and play it.
        /// </summary>
        public void PlaySound(AudioClip soundToPlay) => aSrc.PlayOneShot(soundToPlay);

        public void StopSound() => aSrc.Stop();

        public bool IsPlaying()
        {
            if (aSrc.isPlaying)
                return true;
            return false;
        }
    }
}