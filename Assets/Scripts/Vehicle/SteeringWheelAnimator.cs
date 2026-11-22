using UnityEngine;

namespace CarDriveParking
{
    /// <summary>Turns the cockpit steering wheel with the actual driving input.</summary>
    public class SteeringWheelAnimator : MonoBehaviour
    {
        [SerializeField] private VehicleInput input;
        [SerializeField] private Transform steeringWheel;
        [SerializeField, Range(180f, 720f)] private float maximumRotation = 450f;
        private Quaternion startingRotation;

        public void Configure(VehicleInput vehicleInput, Transform wheel)
        {
            input = vehicleInput;
            steeringWheel = wheel;
            if (steeringWheel != null)
                startingRotation = steeringWheel.localRotation;
        }

        private void Awake()
        {
            if (input == null) input = GetComponent<VehicleInput>();
            if (steeringWheel != null) startingRotation = steeringWheel.localRotation;
        }

        private void Update()
        {
            if (input == null || steeringWheel == null) return;
            Quaternion target = startingRotation * Quaternion.AngleAxis(-input.Steering * maximumRotation, Vector3.forward);
            steeringWheel.localRotation = Quaternion.Slerp(steeringWheel.localRotation, target, Time.deltaTime * 12f);
        }
    }
}