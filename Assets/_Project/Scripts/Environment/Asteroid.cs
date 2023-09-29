using TMPro;
using UnityEngine;

namespace Scripts.Environment
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private Vector3 initialVelocity;
        [SerializeField] private int _health = 100;
        [SerializeField] private bool _isDestroyed;
        [SerializeField] private GameObject[] _debris;
        private Rigidbody _rb;
        private Galaxy _galaxy;

        public bool IsDestroyed { get => _isDestroyed; }

        private void Awake()
        {
            _galaxy = FindObjectOfType<Galaxy>();
        }

        void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
            _rb.AddForce(initialVelocity); // Set initial velocity of satellite
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > 75 || collision.gameObject.CompareTag("Planet"))
                DestroyAsteroid();
        }

        void DestroyAsteroid()
        {
            // Play sound - To Do
            foreach (GameObject obj in _debris)
            {
                Instantiate(obj, transform.position, Quaternion.identity);
                obj.transform.localScale = new Vector3(5f, 5f, 5f);
            }
            _isDestroyed = true;
            _galaxy.UpdateAsteroidList();
            Destroy(this.gameObject);
        }
    }
}