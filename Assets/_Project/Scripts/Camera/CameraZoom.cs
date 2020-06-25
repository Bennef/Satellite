using Scripts.Player;
using UnityEngine;

namespace Scripts.Camera
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private GameObject spaceShip;
        [SerializeField] private Transform barycenter;

        public GameObject SpaceShip { get => spaceShip; set => spaceShip = value; }
        public Transform Barycenter { get => barycenter; set => barycenter = value; }

        private void Start()
        {
            spaceShip = GameObject.FindGameObjectWithTag("Player");
        }

        void LateUpdate() => FixedCameraFollowSmooth(SpaceShip.transform, spaceShip.GetComponent<SpaceShip>().Barycenter);

        // Follow Two Transforms with a Fixed-Orientation Camera
        public void FixedCameraFollowSmooth(Transform t1, Transform t2)
        {
            // How many units should we keep from the spacehip/barycenter
            float zoomFactor = 2.5f;
            float followTimeDelta = 0.8f;

            // Midpoint we're after
            Vector3 midpoint = (t1.position + t2.position) / 2f;

            // Distance between objects
            float distance = (t1.position - t2.position).magnitude;

            // Move camera a certain distance
            Vector3 cameraDestination = midpoint - transform.forward * distance * zoomFactor;

            // You specified to use MoveTowards instead of Slerp
            transform.position = Vector3.Slerp(transform.position, cameraDestination, followTimeDelta);

            // Snap when close enough to prevent annoying slerp behavior
            if ((cameraDestination - transform.position).magnitude <= 0.05f)
                transform.position = cameraDestination;
        }
    }
}