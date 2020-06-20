using UnityEngine;

namespace Scripts.Environment
{
    public class Rotate : MonoBehaviour
    {
        private Rigidbody _rb;
        public Vector3 eulerAngleVelocity;
        public bool _shouldRotate;   // true if the thing should rotate. We can switch it off by setting this to false.

        void Start() => _rb = GetComponent<Rigidbody>();

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_shouldRotate)
            {
                Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
                _rb.MoveRotation(_rb.rotation * deltaRotation);
            }
        }
    }
}