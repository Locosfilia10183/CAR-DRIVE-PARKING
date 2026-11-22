using UnityEngine;

namespace CarDriveParking
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehicleController : MonoBehaviour
    {
        public enum DriveType { FrontWheelDrive, RearWheelDrive, AllWheelDrive }

        [Header("Input")]
        [SerializeField] private VehicleInput input;
        [Header("Wheel colliders")]
        [SerializeField] private WheelCollider frontLeft;
        [SerializeField] private WheelCollider frontRight;
        [SerializeField] private WheelCollider rearLeft;
        [SerializeField] private WheelCollider rearRight;
        [Header("Wheel visual meshes (optional)")]
        [SerializeField] private Transform frontLeftMesh;
        [SerializeField] private Transform frontRightMesh;
        [SerializeField] private Transform rearLeftMesh;
        [SerializeField] private Transform rearRightMesh;
        [Header("Vehicle tuning")]
        [SerializeField] private DriveType driveType = DriveType.RearWheelDrive;
        [SerializeField, Min(100f)] private float motorTorque = 900f;
        [SerializeField, Min(100f)] private float brakeTorque = 2500f;
        [SerializeField, Min(10f)] private float handbrakeTorque = 4500f;
        [SerializeField, Range(10f, 45f)] private float maxSteerAngle = 30f;
        [SerializeField, Min(10f)] private float highSpeedSteerAngle = 10f;
        [SerializeField, Min(10f)] private float maxSpeedKph = 160f;
        [SerializeField] private Vector3 centerOfMassOffset = new Vector3(0f, -0.45f, 0f);
        [SerializeField, Min(0f)] private float downforce = 40f;

        private Rigidbody body;
        public float SpeedKph { get; private set; }

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            body.centerOfMass += centerOfMassOffset;
            if (input == null) input = GetComponent<VehicleInput>();
            if (input == null) Debug.LogError("Add VehicleInput to this vehicle.", this);
            if (!HasAllWheels()) Debug.LogError("Assign all four WheelColliders in the Inspector.", this);
        }

        private void FixedUpdate()
        {
            if (input == null || !HasAllWheels()) return;

            SpeedKph = body.velocity.magnitude * 3.6f;
            float steerLimit = Mathf.Lerp(maxSteerAngle, highSpeedSteerAngle, Mathf.Clamp01(SpeedKph / maxSpeedKph));
            float steer = input.Steering * steerLimit;
            frontLeft.steerAngle = steer;
            frontRight.steerAngle = steer;

            float forwardSpeed = transform.InverseTransformDirection(body.velocity).z;
            float requestedDrive = input.Throttle;
            float serviceBrake = input.Brake;

            if (requestedDrive < -0.05f && forwardSpeed > 1f)
            {
                serviceBrake = 1f;
                requestedDrive = 0f;
            }
            else if (requestedDrive > 0.05f && forwardSpeed < -1f)
            {
                serviceBrake = 1f;
                requestedDrive = 0f;
            }

            ApplyDriveTorque(requestedDrive * motorTorque);
            float normalBrake = serviceBrake * brakeTorque;
            frontLeft.brakeTorque = normalBrake;
            frontRight.brakeTorque = normalBrake;
            rearLeft.brakeTorque = normalBrake + (input.Handbrake ? handbrakeTorque : 0f);
            rearRight.brakeTorque = normalBrake + (input.Handbrake ? handbrakeTorque : 0f);
            body.AddForce(-transform.up * (downforce * body.velocity.magnitude), ForceMode.Force);
        }

        private void ApplyDriveTorque(float torque)
        {
            frontLeft.motorTorque = 0f;
            frontRight.motorTorque = 0f;
            rearLeft.motorTorque = 0f;
            rearRight.motorTorque = 0f;

            if (driveType == DriveType.FrontWheelDrive)
            {
                frontLeft.motorTorque = torque * 0.5f;
                frontRight.motorTorque = torque * 0.5f;
            }
            else if (driveType == DriveType.RearWheelDrive)
            {
                rearLeft.motorTorque = torque * 0.5f;
                rearRight.motorTorque = torque * 0.5f;
            }
            else
            {
                frontLeft.motorTorque = torque * 0.25f;
                frontRight.motorTorque = torque * 0.25f;
                rearLeft.motorTorque = torque * 0.25f;
                rearRight.motorTorque = torque * 0.25f;
            }
        }

        private bool HasAllWheels()
        {
            return frontLeft != null && frontRight != null && rearLeft != null && rearRight != null;
        }

        private void Update()
        {
            UpdateWheelVisual(frontLeft, frontLeftMesh);
            UpdateWheelVisual(frontRight, frontRightMesh);
            UpdateWheelVisual(rearLeft, rearLeftMesh);
            UpdateWheelVisual(rearRight, rearRightMesh);
        }

        private static void UpdateWheelVisual(WheelCollider wheel, Transform wheelMesh)
        {
            if (wheel == null || wheelMesh == null) return;
            wheel.GetWorldPose(out Vector3 position, out Quaternion rotation);
            wheelMesh.SetPositionAndRotation(position, rotation);
        }
    }
}