using UnityEngine;

namespace Scripts.Environment
{
    public class Satellite : MonoBehaviour
    {
        [SerializeField] private Vector3 _initialVelocity;

        private Rigidbody _rb;

        //[SerializeField]
        //private Transform _barycenter; 

        // Start is called before the first frame update
        void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _rb.AddForce(_initialVelocity); // Set initial velocity of ship.
        }
    }
}