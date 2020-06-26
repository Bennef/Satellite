using UnityEngine;

namespace Scripts.Audio
{
    public class SFXManager : MonoBehaviour
    {
        [SerializeField] private AudioClip _crash, _impact, _EMP, _thrust;
        private AudioSource _aSrc, _thrustASrc;

        public AudioClip Crash { get => _crash; }
        public AudioClip Hit { get => _impact; }
        public AudioClip EMP { get => _EMP; }
        public AudioSource ASrc { get => _aSrc; set => _aSrc = value; }
        public AudioSource ThrustASrc { get => _thrustASrc; }
        public AudioClip Thrust { get => _thrust; }

        void Start()
        {
            _aSrc = GetComponent<AudioSource>();
            _thrustASrc = GameObject.Find("SpaceShip").GetComponent<AudioSource>();
        }

        public void PlaySound(AudioSource aSrc, AudioClip soundToPlay) => aSrc.PlayOneShot(soundToPlay);

        public void StopSound(AudioSource aSrc) => aSrc.Stop();

        public bool IsPlaying(AudioSource aSrc)
        {
            if (aSrc.isPlaying)
                return true;
            return false;
        }
    }
}