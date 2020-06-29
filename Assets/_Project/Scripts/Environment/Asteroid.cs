using UnityEngine;

namespace Scripts.Environment
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private Vector3 initialVelocity;
        [SerializeField] private int _health = 100;
        [SerializeField] private GameObject[] _debris;
        private Rigidbody _rb;

        void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _rb.AddForce(initialVelocity); // Set initial velocity of satellite
        }

        void OnCollisionEnter(Collision collision)
        {
            print(collision.relativeVelocity.magnitude);
            if (collision.relativeVelocity.magnitude > 75)
                DestroyAsteroid();
        }

        void DestroyAsteroid()
        {
            // Play sound
            foreach (GameObject obj in _debris)
            {
                Instantiate(obj, transform.position, Quaternion.identity);
                obj.transform.localScale = new Vector3(5f, 5f, 5f);
            }
            Destroy(this.gameObject);
        }
    }
}