using Scripts.Audio;
using Scripts.Environment;
using Scripts.Inputs;
using Scripts.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    public class SpaceShip : MonoBehaviour
    {
        [SerializeField] private Vector3 initialVelocity = new Vector3(1500, 0);
        [SerializeField] private Transform explosionPrefab;
        [SerializeField] private float EMPRadius = 40;
        [SerializeField] private float EMPPower;
        [SerializeField] private bool _shouldAddThrust, _shouldBrake, _isDead, _isDestroyed, _playerTurning, _lockTrajectory;
        [SerializeField] private ParticleSystem[] _thrustParticles;
        [SerializeField] private ParticleSystem _EMPParticles, _impactParticles;
        private float _thrustForce = 1.001f;
        private float yRot;
        private Rigidbody _rb;
        private Transform _spaceShip;
        private SFXManager _sFXManager;
        private InputHandler _inputHandler;
        private Galaxy _galaxy;
        private Light _brakeLight;
        private UIManager _uIManager;
        private Health _health;
        private Quaternion targetRotation;
        private Transform _barycenter;

        public Transform Barycenter { get => _barycenter; set => _barycenter = value; }

        void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _spaceShip = gameObject.GetComponent<Transform>();
            _sFXManager = FindObjectOfType<SFXManager>();
            _inputHandler = FindObjectOfType<InputHandler>();
            _brakeLight = GameObject.Find("Brake Light").GetComponent<Light>();
            _uIManager = FindObjectOfType<UIManager>();
            _health = GetComponent<Health>();
            _galaxy = FindObjectOfType<Galaxy>();

            _rb.AddForce(initialVelocity); // Set initial velocity of ship.

            _lockTrajectory = true;
            UpdateFacingTrajectoryLabel();
            InvokeRepeating("UpdateClosestPlanet", 0, 1); // Update the closest Planet every second.
        }

        public Transform FindNearestPlanet(Transform locatingEntity)
        {
            // Create a temporary List of Planets so we don't reorder the main List.
            List<Transform> tempList = _galaxy.PlanetList;
            // Sort the Planets into ascending order based on distance from SpaceShip.
            tempList.Sort((x, y) => CalculateVectorBetwwenEntities(x, locatingEntity).sqrMagnitude.CompareTo(CalculateVectorBetwwenEntities(y, locatingEntity).sqrMagnitude));
            // Return the first element of the list which is the nearest Planet.
            return tempList[0];
        }

        void UpdateClosestPlanet()
        {
            Barycenter = FindNearestPlanet(_spaceShip);
            _uIManager.SetText(_uIManager.NearestPlanetText, Barycenter.name); // Update UI;
        }

        void Update()
        {
            CheckIfDead();
            UpdateVelocityLabel();
            UpdateHealthLabel();

            if (_isDead)
            {
                if (!_isDestroyed)
                    DestroySpaceShip();
                return;
            }
            else
            {
                if (_inputHandler.GetFaceTrajectoryButtonDown())
                    ToggleFollowingTrajectory();

                // Apply forward thrust to the SpaceShip every frame Space key is pressed.
                if (_inputHandler.GetThrustButtonDown())
                    _shouldAddThrust = true;
                else
                    _shouldAddThrust = false;

                if (_inputHandler.GetBrakeButtonDown())
                    _shouldBrake = true;
                else
                    _shouldBrake = false;

                if (_lockTrajectory)
                { 
                    RotateToVelocityDir();
                    yRot = 0;
                }
                else if (_inputHandler.GetLeftButtonDown())
                    TurnLeft();
                else if (_inputHandler.GetRighttButtonDown())
                    TurnRight();

                if (_inputHandler.GetDeployMineButtonDown())
                    DeployMine();

                if (_inputHandler.GetEMPButtonDown())
                    FireEMP();
            }
        }

        private void UpdateFacingTrajectoryLabel()
        {
            if (_lockTrajectory)
            {
                _uIManager.SetText(_uIManager.FaceTrajectoryText, "On");
                _uIManager.SetColour(_uIManager.FaceTrajectoryText, Color.green);
            }
            else
            {
                _uIManager.SetText(_uIManager.FaceTrajectoryText, "Off");
                _uIManager.SetColour(_uIManager.FaceTrajectoryText, Color.red);
            }
        }

        void FixedUpdate()
        {
            if (!_isDead)
            {
                if (_shouldAddThrust && !_shouldBrake)
                    Thrust();
                else
                {
                    _sFXManager.StopSound(_sFXManager.ThrustASrc);
                    StopThrustParticleEffects();
                }

                if (_shouldBrake && !_shouldAddThrust)
                    Brake();
                else
                {
                    _brakeLight.intensity = 0;
                    _sFXManager.StopSound(_sFXManager.ReverseThrustASrc);
                }
            }
            CapVelocity();
        }

        void Thrust()
        {
            _rb.AddForce(transform.forward * 10, ForceMode.Force);
            foreach (ParticleSystem flames in _thrustParticles)
                flames.Play();
            if (!_sFXManager.IsPlaying(_sFXManager.ThrustASrc))
                _sFXManager.PlaySound(_sFXManager.ThrustASrc, _sFXManager.Thrust);
        }

        private void Brake()
        {
            _brakeLight.intensity = 4;
            _rb.velocity /= _thrustForce;
            if (!_sFXManager.IsPlaying(_sFXManager.ReverseThrustASrc))
                _sFXManager.PlaySound(_sFXManager.ReverseThrustASrc, _sFXManager.ReverseThrust);
        }

        void TurnLeft()
        {
            yRot -= Time.deltaTime * 10;
            ClampYRotation();
            transform.Rotate(0, yRot, 0);
        }

        void TurnRight()
        {
            yRot += Time.deltaTime * 10;
            ClampYRotation();
            transform.Rotate(0, yRot, 0);
        }

        void ClampYRotation() => yRot = Mathf.Clamp(yRot, -1f, 1f);

        public void RotateToVelocityDir()
        {
            targetRotation = Quaternion.LookRotation(_rb.velocity);
            _spaceShip.rotation = Quaternion.Lerp(_spaceShip.rotation, targetRotation, 5 * Time.deltaTime);
        }

        void ToggleFollowingTrajectory()
        {
            _lockTrajectory = !_lockTrajectory;
            UpdateFacingTrajectoryLabel();
        }

        void CapVelocity()
        {
            if (_rb.velocity.magnitude > 100)
                _rb.velocity /= _thrustForce;
        }

        void CheckIfDead()
        {
            if (_health.HealthLeft < 1)
                _isDead = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _health.TakeDamage(25);
            _sFXManager.PlaySound(_sFXManager.ASrc, _sFXManager.Hit);
            _impactParticles.Play();
        }

        private void DestroySpaceShip()
        {
            _isDestroyed = true;
            _sFXManager.PlaySound(_sFXManager.ASrc, _sFXManager.Crash);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        void StopThrustParticleEffects()
        {
            foreach (ParticleSystem flames in _thrustParticles)
                flames.Stop();
        }

        private void FireEMP()
        {
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, EMPRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(EMPPower, explosionPos, EMPRadius, 3.0F);
            }
            _sFXManager.PlaySound(_sFXManager.ASrc, _sFXManager.EMP);
            _EMPParticles.Play();
        }

        public static Vector3 CalculateVectorBetwwenEntities(Transform entity1, Transform entity2)
        {
            return entity2.position - entity1.position;
        }

        public void SetVelocity(Vector3 newVelocity)
        {
            _rb.AddForce(_spaceShip.forward, ForceMode.Force);
            _rb.AddForce(newVelocity, ForceMode.Acceleration);
        }

        public void DeployMine()
        {
            // Instantiate mine
            // Play sound
            print("mine");
        }

        private float CalculateSpeed() => Mathf.Round(_rb.velocity.magnitude);

        void UpdateVelocityLabel() => _uIManager.SetText(_uIManager.VelocityText, CalculateSpeed().ToString());

        void UpdateHealthLabel()
        {
            _uIManager.SetText(_uIManager.HealthText, _health.HealthLeft.ToString());
            switch (_health.HealthLeft)
            {
                case 100:
                    _uIManager.SetColour(_uIManager.HealthText, Color.green);
                    break;
                case 75:
                    _uIManager.SetColour(_uIManager.HealthText, Color.yellow);
                    break;
                case 50:
                    _uIManager.SetColour(_uIManager.HealthText, new Color(255, 165, 0));
                    break;
                case 25:
                    _uIManager.SetColour(_uIManager.HealthText, Color.red);
                    break;
            }
        }
    }
}