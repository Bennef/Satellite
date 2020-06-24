using UnityEngine;

namespace Scripts.Audio
{
    public class SFXManager : MonoBehaviour
    {
        private AudioSource _aSrc;
        [SerializeField] private AudioClip thrust, crash;

        public AudioClip Crash { get => crash; set => crash = value; }
        public AudioClip Thrust { get => thrust; set => thrust = value; }

        void Start() => _aSrc = GetComponent<AudioSource>();

        public void PlaySound(AudioClip soundToPlay) => _aSrc.PlayOneShot(soundToPlay);

        public void StopSound() => _aSrc.Stop();

        public bool IsPlaying()
        {
            if (_aSrc.isPlaying)
                return true;
            return false;
        }
    }
}