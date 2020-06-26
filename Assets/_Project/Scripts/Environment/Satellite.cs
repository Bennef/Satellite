using UnityEngine;

namespace Scripts.Environment
{
    public class Satellite : MonoBehaviour
    {
        [SerializeField] private Vector3 initialVelocity;
        [SerializeField] private int _health = 100;
        private Rigidbody _rb;

        void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _rb.AddForce(initialVelocity); // Set initial velocity of satellite
        }
    }
}