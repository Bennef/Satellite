using UnityEngine;

namespace Scripts.Environment
{
    public class Satellite : MonoBehaviour
    {
        [SerializeField] private Vector3 initialVelocity;
        //[SerializeField] private Transform _barycenter; 
        private Rigidbody _rb;

        void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _rb.AddForce(initialVelocity); // Set initial velocity of satellite
        }
    }
}