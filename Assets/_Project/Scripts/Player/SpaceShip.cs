using Scripts.Audio;
using Scripts.Inputs;
using UnityEngine;

namespace Scripts.Player
{
    public class SpaceShip : MonoBehaviour
    {
        [SerializeField] private Vector3 initialVelocity;
        [SerializeField] private Transform explosionPrefab;
        [SerializeField] private float EMPRadius;
        [SerializeField] private float EMPPower;
        [SerializeField] private bool shouldAddThrust, shouldBrake, isDead;
        //[SerializeField] private Transform _barycenter; 
        private float _thrustForce = 1.001f;
        private Rigidbody _rb;
        private Transform _spaceShip;
        private ParticleSystem[] _thrustParticles;
        private MeshRenderer _shipMeshRenderer;
        private SFXManager _sFXManager;
        private InputHandler _inputHandler;

        void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _spaceShip = gameObject.GetComponent<Transform>();
            _thrustParticles = gameObject.GetComponentsInChildren<ParticleSystem>();
            _shipMeshRenderer = GetComponentInChildren<MeshRenderer>();
            _sFXManager = FindObjectOfType<SFXManager>();
            _inputHandler = FindObjectOfType<InputHandler>();
            _rb.AddForce(initialVelocity); // Set initial velocity of ship.
        }

        void Update()
        {
            if (!isDead)
            {
                // Apply forward thrust to the SpaceShip every frame Space key is pressed.
                if (_inputHandler.GetThrustButtonDown())
                    shouldAddThrust = true;
                else
                    shouldAddThrust = false;

                if (_inputHandler.GetBrakeButtonDown())
                    shouldBrake = true;
                else
                    shouldBrake = false;

                RotateToVelocityDir();

                // If player presses F, launch a satellite.
                if (_inputHandler.GetDeploySatelliteButtonDown())
                    LaunchSatellite();

                // If player presses E, cause EMP.
                if (_inputHandler.GetEMPButtonDown())
                    EMP();
            }
        }
        
        void FixedUpdate()
        {
            if (!isDead)
            {
                // Apply forward thrust to the SpaceShip every frame Space key is pressed.
                if (shouldAddThrust)
                {
                    _rb.velocity *= _thrustForce;
                    foreach (ParticleSystem flames in _thrustParticles)
                        flames.Play();
                    if (!_sFXManager.IsPlaying())
                        _sFXManager.PlaySound(_sFXManager.Thrust);
                }
                else 
                {
                    _sFXManager.StopSound();
                    foreach (ParticleSystem flames in _thrustParticles)
                        flames.Stop();
                }

                if (shouldBrake)
                {
                    _rb.velocity /= _thrustForce;
                }
            }
        }

        private void OnCollisionEnter(Collision collision) => DestroySpaceShip();

        private void DestroySpaceShip()
        {
            isDead = true;
            _sFXManager.PlaySound(_sFXManager.Crash);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            _rb.isKinematic = true;
            _shipMeshRenderer.enabled = false;
        }

        private void EMP()
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, EMPRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(EMPPower, explosionPos, EMPRadius, 3.0F);
            }
            Debug.Log("EMP");
        }

        public static Vector3 CalculateVectorBetwwenEntities(Transform entity1, Transform entity2)
        {
            return entity2.position - entity1.position;
        }

        public void RotateToVelocityDir() => _spaceShip.rotation = Quaternion.LookRotation(_rb.velocity, Vector3.up);

        public void SetVelocity(Vector3 newVelocity)
        {
            _rb.AddForce(_spaceShip.forward, ForceMode.Force);
            _rb.AddForce(newVelocity, ForceMode.Acceleration);
        }

        public void LaunchSatellite()
        {
            Debug.Log("launch");
        }
    }
}