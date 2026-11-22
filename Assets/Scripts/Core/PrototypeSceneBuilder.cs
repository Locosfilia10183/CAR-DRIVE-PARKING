using UnityEngine;

namespace CarDriveParking
{
    /// <summary>
    /// Creates a temporary, fully primitive test scene at runtime.
    /// Attach this to an empty GameObject in a new Unity scene and press Play.
    /// Replace all generated visuals with original production assets later.
    /// </summary>
    public class PrototypeSceneBuilder : MonoBehaviour
    {
        [SerializeField] private bool buildOnPlay = true;
        [SerializeField] private Vector3 vehicleSpawnPosition = new Vector3(0f, 0.65f, -8f);

        private void Start()
        {
            if (buildOnPlay)
                BuildScene();
        }

        [ContextMenu("Build Prototype Scene Now")]
        public void BuildScene()
        {
            CreateGround();
            GameObject vehicle = CreateVehicle();
            CreateParkingBay();
            CreateCameraRig(vehicle.transform);
            Debug.Log("CAR DRIVE PARKING prototype scene created. Use WASD/arrows to drive and C to change camera.", this);
        }

        private void CreateGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Prototype Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(12f, 1f, 12f);
            ground.GetComponent<Renderer>().material = MakeMaterial(new Color(0.22f, 0.25f, 0.27f));
        }

        private GameObject CreateVehicle()
        {
            GameObject car = new GameObject("VELTOR V6 S - Prototype");
            car.transform.position = vehicleSpawnPosition;

            Rigidbody body = car.AddComponent<Rigidbody>();
            body.mass = 1450f;
            body.drag = 0.015f;
            body.angularDrag = 0.5f;
            body.interpolation = RigidbodyInterpolation.Interpolate;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            BoxCollider bodyCollider = car.AddComponent<BoxCollider>();
            bodyCollider.center = new Vector3(0f, 0.86f, 0f);
            bodyCollider.size = new Vector3(1.85f, 0.62f, 4.25f);

            Material paint = MakeMaterial(new Color(0.08f, 0.28f, 0.72f));
            Material glass = MakeMaterial(new Color(0.06f, 0.10f, 0.14f));
            Material tire = MakeMaterial(new Color(0.025f, 0.025f, 0.03f));
            Material rim = MakeMaterial(new Color(0.62f, 0.65f, 0.68f));

            CreatePrimitivePart(car.transform, PrimitiveType.Cube, "Body shell", new Vector3(0f, 0.86f, 0f), new Vector3(1.85f, 0.62f, 4.25f), paint);
            CreatePrimitivePart(car.transform, PrimitiveType.Cube, "Cabin", new Vector3(0f, 1.38f, -0.15f), new Vector3(1.48f, 0.62f, 1.95f), glass);
            CreatePrimitivePart(car.transform, PrimitiveType.Cube, "Front bumper", new Vector3(0f, 0.62f, 2.08f), new Vector3(1.82f, 0.22f, 0.18f), paint);
            CreatePrimitivePart(car.transform, PrimitiveType.Cube, "Rear bumper", new Vector3(0f, 0.62f, -2.08f), new Vector3(1.82f, 0.22f, 0.18f), paint);

            WheelCollider fl, fr, rl, rr;
            Transform flVisual, frVisual, rlVisual, rrVisual;
            CreateWheel(car.transform, "Front Left", new Vector3(-0.91f, 0.42f, 1.35f), tire, rim, out fl, out flVisual);
            CreateWheel(car.transform, "Front Right", new Vector3(0.91f, 0.42f, 1.35f), tire, rim, out fr, out frVisual);
            CreateWheel(car.transform, "Rear Left", new Vector3(-0.91f, 0.42f, -1.35f), tire, rim, out rl, out rlVisual);
            CreateWheel(car.transform, "Rear Right", new Vector3(0.91f, 0.42f, -1.35f), tire, rim, out rr, out rrVisual);

            VehicleInput input = car.AddComponent<VehicleInput>();
            VehicleController controller = car.AddComponent<VehicleController>();
            controller.Configure(input, fl, fr, rl, rr, flVisual, frVisual, rlVisual, rrVisual);

            return car;
        }

        private void CreateWheel(Transform parent, string wheelName, Vector3 localPosition, Material tire, Material rim,
            out WheelCollider wheelCollider, out Transform visualPivot)
        {
            GameObject wheelObject = new GameObject(wheelName + " WheelCollider");
            wheelObject.transform.SetParent(parent, false);
            wheelObject.transform.localPosition = localPosition;

            wheelCollider = wheelObject.AddComponent<WheelCollider>();
            wheelCollider.mass = 22f;
            wheelCollider.radius = 0.36f;
            wheelCollider.suspensionDistance = 0.20f;
            wheelCollider.wheelDampingRate = 1.2f;
            JointSpring spring = wheelCollider.suspensionSpring;
            spring.spring = 35000f;
            spring.damper = 4500f;
            spring.targetPosition = 0.5f;
            wheelCollider.suspensionSpring = spring;

            GameObject pivot = new GameObject(wheelName + " Visual");
            pivot.transform.SetParent(parent, false);
            pivot.transform.localPosition = localPosition;
            visualPivot = pivot.transform;

            GameObject tireMesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tireMesh.name = "Tire";
            tireMesh.transform.SetParent(visualPivot, false);
            tireMesh.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            tireMesh.transform.localScale = new Vector3(0.72f, 0.16f, 0.72f);
            tireMesh.GetComponent<Renderer>().material = tire;
            Destroy(tireMesh.GetComponent<Collider>());

            GameObject rimMesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rimMesh.name = "Rim";
            rimMesh.transform.SetParent(visualPivot, false);
            rimMesh.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            rimMesh.transform.localScale = new Vector3(0.42f, 0.17f, 0.42f);
            rimMesh.GetComponent<Renderer>().material = rim;
            Destroy(rimMesh.GetComponent<Collider>());
        }

        private void CreateParkingBay()
        {
            GameObject bay = new GameObject("Parking Bay - Stop and Align");
            bay.transform.position = new Vector3(0f, 0.12f, 5f);
            BoxCollider trigger = bay.AddComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.size = new Vector3(3f, 1.5f, 6f);
            bay.AddComponent<ParkingZone>();

            Material marking = MakeMaterial(new Color(0.12f, 0.75f, 0.28f));
            CreatePrimitivePart(bay.transform, PrimitiveType.Cube, "Left parking line",
                new Vector3(-1.45f, 0.01f, 0f), new Vector3(0.08f, 0.025f, 6f), marking, false);
            CreatePrimitivePart(bay.transform, PrimitiveType.Cube, "Right parking line",
                new Vector3(1.45f, 0.01f, 0f), new Vector3(0.08f, 0.025f, 6f), marking, false);
            CreatePrimitivePart(bay.transform, PrimitiveType.Cube, "Front parking line",
                new Vector3(0f, 0.01f, 2.96f), new Vector3(3f, 0.025f, 0.08f), marking, false);
        }

        private void CreateCameraRig(Transform vehicle)
        {
            GameObject exteriorObject = new GameObject("Exterior Camera");
            exteriorObject.transform.SetParent(vehicle, false);
            exteriorObject.transform.localPosition = new Vector3(0f, 3.0f, -6.5f);
            Camera exterior = exteriorObject.AddComponent<Camera>();
            exterior.fieldOfView = 68f;
            exteriorObject.AddComponent<PrototypeLookAt>().Target = vehicle;

            GameObject interiorObject = new GameObject("Interior Camera");
            interiorObject.transform.SetParent(vehicle, false);
            interiorObject.transform.localPosition = new Vector3(0f, 1.48f, 0.45f);
            interiorObject.transform.localRotation = Quaternion.identity;
            Camera interior = interiorObject.AddComponent<Camera>();
            interior.fieldOfView = 78f;

            CameraRig rig = vehicle.gameObject.AddComponent<CameraRig>();
            // CameraRig discovers its cameras through the public setup method.
            rig.Configure(exterior, interior);
        }

        private GameObject CreatePrimitivePart(Transform parent, PrimitiveType type, string partName,
            Vector3 localPosition, Vector3 localScale, Material material, bool removeCollider = true)
        {
            GameObject part = GameObject.CreatePrimitive(type);
            part.name = partName;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localScale = localScale;
            part.GetComponent<Renderer>().material = material;
            if (removeCollider)
                Destroy(part.GetComponent<Collider>());
            return part;
        }

        private Material MakeMaterial(Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            Material material = new Material(shader);
            material.color = color;
            return material;
        }
    }
}