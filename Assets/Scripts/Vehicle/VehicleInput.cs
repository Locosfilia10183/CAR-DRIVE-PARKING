using UnityEngine;
namespace CarDriveParking
{
    public class VehicleInput : MonoBehaviour
    {
        public enum MobileControl { SteerLeft, SteerRight, Throttle, Brake, Handbrake }
        public float Steering { get; private set; }
        public float Throttle { get; private set; }
        public float Brake { get; private set; }
        public bool Handbrake { get; private set; }
        private bool mobileLeft, mobileRight, mobileThrottle, mobileBrake, mobileHandbrake;
        private void Update()
        {
            float steer = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) steer -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) steer += 1f;
            float throttle = 0f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) throttle = 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) throttle = -1f;
            Steering = Mathf.Clamp(steer + (mobileRight ? 1f : 0f) - (mobileLeft ? 1f : 0f), -1f, 1f);
            Throttle = Mathf.Clamp(throttle + (mobileThrottle ? 1f : 0f), -1f, 1f);
            Brake = mobileBrake ? 1f : 0f;
            Handbrake = mobileHandbrake || Input.GetKey(KeyCode.Space);
        }
        public void SetMobileControl(MobileControl control, bool pressed)
        {
            switch (control)
            {
                case MobileControl.SteerLeft: mobileLeft = pressed; break;
                case MobileControl.SteerRight: mobileRight = pressed; break;
                case MobileControl.Throttle: mobileThrottle = pressed; break;
                case MobileControl.Brake: mobileBrake = pressed; break;
                case MobileControl.Handbrake: mobileHandbrake = pressed; break;
            }
        }
    }
}