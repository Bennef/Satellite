using Scripts.Audio;
using UnityEngine;

namespace Satellites
{
    public class Mine : MonoBehaviour
    {
        [SerializeField] private Transform explosionPrefab;
        [SerializeField] private float MineRadius = 50;
        [SerializeField] private float MinePower;
        private SFXManager _sFXManager;

        private void Start()
        {
            _sFXManager = FindObjectOfType<SFXManager>();
        }

        void OnCollisionEnter(Collision collision)
        {
           if (!collision.gameObject.CompareTag("Mine"))
                DestroyMine();
        }

        void DestroyMine()
        {
            // Play sound - To Do
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, MineRadius);

            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null && hit.tag != "Player")
                    rb.AddExplosionForce(MinePower, explosionPos, MineRadius, 3.0F);
                _sFXManager.PlaySound(_sFXManager.ASrc, _sFXManager.Crash);
            }
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}

