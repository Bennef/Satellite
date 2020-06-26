using UnityEngine;

namespace Scripts.Environment
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private Vector3 eulerAngleVelocity;
        [SerializeField] public bool shouldRotate = true;   
        private Rigidbody _rb;

        void Start() => _rb = GetComponent<Rigidbody>();

        void FixedUpdate()
        {
            if (shouldRotate)
            {
                Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
                _rb.MoveRotation(_rb.rotation * deltaRotation);
            }
        }
    }
}