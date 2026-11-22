using UnityEngine;

namespace CarDriveParking
{
    /// <summary>
    /// Slowly rotates the menu vehicle so the main menu has a live 3D showcase.
    /// </summary>
    public class MenuVehicleRotation : MonoBehaviour
    {
        [SerializeField] private float degreesPerSecond = 7f;

        private void Update()
        {
            transform.Rotate(Vector3.up, degreesPerSecond * Time.deltaTime, Space.World);
        }
    }
}
