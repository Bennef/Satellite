using UnityEngine;

namespace Scripts.Audio
{
    public class SFXManager : MonoBehaviour
    {
        [SerializeField] private AudioClip _crash, _impact, _EMP, _thrust, _reverseThrust, _mine;
        private AudioSource _aSrc, _thrustASrc, _reverseThrustASrc;

        public AudioClip Crash { get => _crash; }
        public AudioClip Hit { get => _impact; }
        public AudioClip EMP { get => _EMP; }
        public AudioClip Thrust { get => _thrust; }
        public AudioClip ReverseThrust { get => _reverseThrust; }
        public AudioClip Mine { get => _mine; }

        public AudioSource ASrc { get => _aSrc; set => _aSrc = value; }
        public AudioSource ThrustASrc { get => _thrustASrc; }
        public AudioSource ReverseThrustASrc { get => _reverseThrustASrc; }

        void Start()
        {
            _aSrc = GetComponent<AudioSource>();
            _thrustASrc = GameObject.Find("SpaceShip").GetComponent<AudioSource>();
            _reverseThrustASrc = GameObject.Find("Model").GetComponent<AudioSource>();
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