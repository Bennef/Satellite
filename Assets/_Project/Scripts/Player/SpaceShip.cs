﻿using Scripts.Audio;
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
        [SerializeField] private float EMPRadius;
        [SerializeField] private float EMPPower;
        [SerializeField] private bool shouldAddThrust, shouldBrake, isDead, isDestroyed;
        [SerializeField] private Transform _barycenter; 
        [SerializeField] private List<Transform> _planetList;
        [SerializeField] private ParticleSystem[] _thrustParticles;
        [SerializeField] private ParticleSystem _EMPParticles, _impactParticles;
        private float _thrustForce = 1.001f;
        private Rigidbody _rb;
        private Transform _spaceShip;
        private SFXManager _sFXManager;
        private InputHandler _inputHandler;
        private Light _brakeLight;
        private UIManager _uIManager;
        private Health _health;
        
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

            _rb.AddForce(initialVelocity); // Set initial velocity of ship.

            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Planet"))
                _planetList.Add(go.GetComponent<Transform>());

            InvokeRepeating("UpdateClosestPlanet", 0, 1); // Update the closest Planet every second.
        }

        public Transform FindNearestPlanet(Transform locatingEntity)
        {
            // Create a temporary List of Planets so we don't reorder the main List.
            List<Transform> tempList = _planetList;

            // Sort the Planets into ascending order based on distance from SpaceShip.
            tempList.Sort((x, y) => CalculateVectorBetwwenEntities(x, locatingEntity).magnitude.CompareTo(CalculateVectorBetwwenEntities(y, locatingEntity).magnitude));

            // Return the first element of the list which is the nearest Planet.
            return tempList[0];
        }

        void UpdateClosestPlanet()
        {
            _barycenter = FindNearestPlanet(_spaceShip);
            _uIManager.SetText(_uIManager.NearestPlanetText, _barycenter.name); // Update UI;
        }

        void Update()
        {
            CheckIfDead();

            if (isDead)
            {
                if (!isDestroyed)
                    DestroySpaceShip();
                return;
            }
            else
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

                // If player presses F, launch a mine.
                if (_inputHandler.GetDeploySatelliteButtonDown())
                    DeployMine();

                // If player presses E or clicks mouse, cause EMP.
                if (_inputHandler.GetEMPButtonDown())
                    EMP();
            }
        }

        void CheckIfDead()
        {
            if (_health.HealthLeft < 1)
                isDead = true;
        }

        void FixedUpdate()
        {
            if (!isDead)
            {
                // Apply forward thrust to the SpaceShip every frame Space key is pressed.
                if (shouldAddThrust && !shouldBrake)
                {
                    _rb.velocity *= _thrustForce;
                    foreach (ParticleSystem flames in _thrustParticles)
                        flames.Play();
                    if (!_sFXManager.IsPlaying(_sFXManager.ThrustASrc))
                        _sFXManager.PlaySound(_sFXManager.ThrustASrc, _sFXManager.Thrust);
                }
                else
                {
                    _sFXManager.StopSound(_sFXManager.ThrustASrc);
                    StopParticleEffects();
                }

                if (shouldBrake && !shouldAddThrust)
                {
                    _brakeLight.intensity = 4;
                    _rb.velocity /= _thrustForce;
                    if (!_sFXManager.IsPlaying(_sFXManager.ReverseThrustASrc))
                        _sFXManager.PlaySound(_sFXManager.ReverseThrustASrc, _sFXManager.ReverseThrust);
                }
                else
                {
                    _brakeLight.intensity = 0;
                    _sFXManager.StopSound(_sFXManager.ReverseThrustASrc);
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            _health.TakeDamage(25);
            _sFXManager.PlaySound(_sFXManager.ASrc, _sFXManager.Hit);
            _impactParticles.Play();
        }

        private void DestroySpaceShip()
        {
            isDead = true;
            isDestroyed = true;
            _sFXManager.PlaySound(_sFXManager.ASrc, _sFXManager.Crash);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        void StopParticleEffects() 
        {
            foreach (ParticleSystem flames in _thrustParticles)
                flames.Stop();
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
            _sFXManager.PlaySound(_sFXManager.ASrc, _sFXManager.EMP);
            _EMPParticles.Play();
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

        public void DeployMine()
        {
            // Play sound
            print("mine");
        }
    }
}