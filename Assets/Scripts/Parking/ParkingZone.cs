using System.Collections.Generic;
using UnityEngine;

namespace CarDriveParking
{
    [RequireComponent(typeof(BoxCollider))]
    public class ParkingZone : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float maximumParkingSpeedKph = 1.5f;
        [SerializeField, Range(1f, 45f)] private float allowedAngleDegrees = 12f;
        private readonly HashSet<Rigidbody> parkedVehicles = new HashSet<Rigidbody>();

        private void Reset()
        {
            BoxCollider box = GetComponent<BoxCollider>();
            box.isTrigger = true;
        }

        private void OnTriggerStay(Collider other)
        {
            Rigidbody vehicleBody = other.attachedRigidbody;
            if (vehicleBody == null || parkedVehicles.Contains(vehicleBody)) return;

            float speedKph = vehicleBody.velocity.magnitude * 3.6f;
            float angle = Vector3.Angle(vehicleBody.transform.forward, transform.forward);

            if (speedKph <= maximumParkingSpeedKph && angle <= allowedAngleDegrees)
            {
                parkedVehicles.Add(vehicleBody);
                Debug.Log("Parking successful! Vehicle stopped and aligned in the parking zone.", vehicleBody);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Rigidbody vehicleBody = other.attachedRigidbody;
            if (vehicleBody != null) parkedVehicles.Remove(vehicleBody);
        }
    }
}