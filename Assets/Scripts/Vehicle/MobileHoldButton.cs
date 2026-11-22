using UnityEngine;
using UnityEngine.EventSystems;

namespace CarDriveParking
{
    /// <summary>Attach to a UI Button and select its control in the Inspector.</summary>
    public class MobileHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private VehicleInput targetInput;
        [SerializeField] private VehicleInput.MobileControl control;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (targetInput != null) targetInput.SetMobileControl(control, true);
        }

        public void OnPointerUp(PointerEventData eventData) { Release(); }
        public void OnPointerExit(PointerEventData eventData) { Release(); }

        private void OnDisable() { Release(); }

        private void Release()
        {
            if (targetInput != null) targetInput.SetMobileControl(control, false);
        }
    }
}