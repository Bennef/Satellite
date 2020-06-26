using UnityEngine;

namespace Scripts.Inputs
{
    public class InputHandler : MonoBehaviour
    {
        public bool GetThrustButtonDown() => Input.GetButton("Thrust");

        public bool GetBrakeButtonDown() => Input.GetButton("Brake");

        public bool GetLeftButtonDown() => Input.GetButton("Turn Left");

        public bool GetRighttButtonDown() => Input.GetButton("Turn Right");

        public bool GetEMPButtonDown() => Input.GetButtonDown("EMP");

        public bool GetFaceTrajectoryButtonDown() => Input.GetButtonDown("Face Trajectory");
        
        public bool GetDeployMineButtonDown() => Input.GetButtonDown("Deploy Mine");
    }
}