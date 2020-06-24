using UnityEngine;

namespace Scripts.Camera
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private Transform spaceShip;
        [SerializeField] private Transform barycenter;

        public Transform SpaceShip { get => spaceShip; set => spaceShip = value; }
        public Transform Barycenter { get => barycenter; set => barycenter = value; } // Currently set to Planet 1 - change later
        
        private void Start() => SpaceShip = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        void LateUpdate() => FixedCameraFollowSmooth(SpaceShip, Barycenter);

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