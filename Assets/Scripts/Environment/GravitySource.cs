using UnityEngine;

namespace Scripts.Environment
{
    [RequireComponent(typeof(Rigidbody))]
    public class GravitySource : MonoBehaviour
    {
        [SerializeField] private float _gravity;

        public float Gravity { get => _gravity; set => _gravity = value; }

        void OnTriggerStay(Collider other)
        {
            Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
            if (!otherRigidbody.CompareTag("Planet"))
            {
                Vector3 difference = this.gameObject.transform.position - other.gameObject.transform.position;

                float dist = difference.magnitude;
                Vector3 gravityDirection = difference.normalized;
                Vector3 gravityVector = (gravityDirection * _gravity) / (dist * dist);

                otherRigidbody.AddForce(gravityVector, ForceMode.Acceleration);
                //Debug.Log(gameObject.name + ",    " + otherRigidbody.name);
            }
        }
    }
}