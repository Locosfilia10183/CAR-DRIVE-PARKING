using UnityEngine;

namespace CarDriveParking
{
    public class CameraRig : MonoBehaviour
    {
        [SerializeField] private Camera exteriorCamera;
        [SerializeField] private Camera interiorCamera;
        [SerializeField] private KeyCode switchKey = KeyCode.C;
        private bool showingInterior;

        public void Configure(Camera exterior, Camera interior)
        {
            exteriorCamera = exterior;
            interiorCamera = interior;
        }

        private void Start() { SetInterior(false); }

        private void Update()
        {
            if (Input.GetKeyDown(switchKey)) ToggleCamera();
        }

        public void ToggleCamera() { SetInterior(!showingInterior); }

        public void SetInterior(bool useInterior)
        {
            showingInterior = useInterior;
            if (exteriorCamera != null) exteriorCamera.gameObject.SetActive(!showingInterior);
            if (interiorCamera != null) interiorCamera.gameObject.SetActive(showingInterior);
        }
    }
}