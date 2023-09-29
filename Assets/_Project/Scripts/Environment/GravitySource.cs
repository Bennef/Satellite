using UnityEngine;

namespace Scripts.Environment
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravitySource : MonoBehaviour
    {
        private const float _GRAVITY = 200000;

        public float Gravity { get => _GRAVITY; }

        void OnTriggerStay(Collider other)
        {
            Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
            if (!otherRigidbody.CompareTag("Planet"))
            {
                Vector3 difference = this.gameObject.transform.position - other.gameObject.transform.position;

                float dist = difference.magnitude;
                Vector3 gravityDirection = difference.normalized;
                Vector3 gravityVector = (gravityDirection * _GRAVITY) / (dist * dist);

                otherRigidbody.AddForce(gravityVector, ForceMode.Acceleration);
            }
        }
    }
}