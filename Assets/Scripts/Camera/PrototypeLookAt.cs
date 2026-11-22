using UnityEngine;

namespace CarDriveParking
{
    public class PrototypeLookAt : MonoBehaviour
    {
        public Transform Target { get; set; }

        private void LateUpdate()
        {
            if (Target == null) return;
            Vector3 focus = Target.position + Vector3.up * 0.95f;
            transform.rotation = Quaternion.LookRotation(focus - transform.position, Vector3.up);
        }
    }
}