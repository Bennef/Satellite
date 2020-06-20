using UnityEngine;

namespace Scripts.Inputs
{
    public class InputHandler : MonoBehaviour
    {
        public bool GetThrustButtonDown()
        {
            return Input.GetButton("Thrust");
        }

        public bool GetBrakeButtonDown()
        {
            return Input.GetButton("Brake");
        }

        public bool GetEMPButtonDown()
        {
            return Input.GetButtonDown("EMP");
        }

        public bool GetDeploySatelliteButtonDown()
        {
            return Input.GetButtonDown("Deploy Satellite");
        }
    }
}