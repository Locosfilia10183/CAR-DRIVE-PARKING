using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CarDriveParking
{
    /// <summary>
    /// Turns the generated Neustadt district into a simple playable driving scene.
    /// Add this beside NeustadtMapBuilder on the same scene bootstrap object.
    /// </summary>
    [DefaultExecutionOrder(100)]
    public sealed class NeustadtPlayableBootstrap : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private Vector3 spawnPosition = new Vector3(29f, -0.08f, -27f);
        [SerializeField] private bool createTouchControls = true;

        private VehicleController vehicle;
        private GUIStyle hudStyle;

        private void Start()
        {
            DisableOverviewCamera();
            GameObject car = CreatePlayerCar();
            CreateFollowCameras(car.transform);
            if (createTouchControls) CreateTouchHud(car.GetComponent<VehicleInput>(), car.GetComponent<CameraRig>());
            Debug.Log("Neustadt is playable. Drive with WASD / arrow keys, Space for handbrake, C to switch camera. Touch buttons are also available.", this);
        }

        private void DisableOverviewCamera()
        {
            Camera[] cameras = FindObjectsOfType<Camera>();
            foreach (Camera camera in cameras)
            {
                if (camera.name.Contains("Overview") || camera.name.Contains("Main Camera"))
                    camera.gameObject.SetActive(false);
            }
        }

        private GameObject CreatePlayerCar()
        {
            GameObject car = new GameObject("Player - VELTOR V6 S");
            car.transform.position = spawnPosition;

            Rigidbody body = car.AddComponent<Rigidbody>();
            body.mass = 1450f;
            body.drag = 0.02f;
            body.angularDrag = 0.65f;
            body.interpolation = RigidbodyInterpolation.Interpolate;
            body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            body.constraints = RigidbodyConstraints.None;

            BoxCollider chassis = car.AddComponent<BoxCollider>();
            chassis.center = new Vector3(0f, 0.86f, 0f);
            chassis.size = new Vector3(1.82f, 0.58f, 4.15f);

            VehicleVisualBuilder.Build(car.transform);
            VehicleInput input = car.AddComponent<VehicleInput>();

            Material tire = MakeMaterial(new Color(0.025f, 0.025f, 0.03f));
            Material rim = MakeMaterial(new Color(0.58f, 0.62f, 0.66f));

            WheelCollider fl, fr, rl, rr;
            Transform flVisual, frVisual, rlVisual, rrVisual;
            CreateWheel(car.transform, "Front Left", new Vector3(-0.91f, 0.42f, 1.35f), tire, rim, out fl, out flVisual);
            CreateWheel(car.transform, "Front Right", new Vector3(0.91f, 0.42f, 1.35f), tire, rim, out fr, out frVisual);
            CreateWheel(car.transform, "Rear Left", new Vector3(-0.91f, 0.42f, -1.35f), tire, rim, out rl, out rlVisual);
            CreateWheel(car.transform, "Rear Right", new Vector3(0.91f, 0.42f, -1.35f), tire, rim, out rr, out rrVisual);

            vehicle = car.AddComponent<VehicleController>();
            vehicle.Configure(input, fl, fr, rl, rr, flVisual, frVisual, rlVisual, rrVisual);
            return car;
        }

        private void CreateWheel(Transform parent, string label, Vector3 localPosition, Material tire, Material rim,
            out WheelCollider wheel, out Transform visual)
        {
            GameObject wheelObject = new GameObject(label + " WheelCollider");
            wheelObject.transform.SetParent(parent, false);
            wheelObject.transform.localPosition = localPosition;

            wheel = wheelObject.AddComponent<WheelCollider>();
            wheel.mass = 22f;
            wheel.radius = 0.36f;
            wheel.suspensionDistance = 0.20f;
            wheel.wheelDampingRate = 1.5f;
            JointSpring spring = wheel.suspensionSpring;
            spring.spring = 35000f;
            spring.damper = 4500f;
            spring.targetPosition = 0.5f;
            wheel.suspensionSpring = spring;

            GameObject pivot = new GameObject(label + " Wheel Visual");
            pivot.transform.SetParent(parent, false);
            pivot.transform.localPosition = localPosition;
            visual = pivot.transform;

            MakeWheelMesh(visual, "Tire", new Vector3(0.72f, 0.16f, 0.72f), tire);
            MakeWheelMesh(visual, "Rim", new Vector3(0.42f, 0.17f, 0.42f), rim);
        }

        private void MakeWheelMesh(Transform parent, string label, Vector3 scale, Material material)
        {
            GameObject mesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            mesh.name = label;
            mesh.transform.SetParent(parent, false);
            mesh.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            mesh.transform.localScale = scale;
            mesh.GetComponent<Renderer>().sharedMaterial = material;
            Collider collider = mesh.GetComponent<Collider>();
            if (collider != null) Destroy(collider);
        }

        private void CreateFollowCameras(Transform car)
        {
            GameObject exteriorObject = new GameObject("Player Exterior Camera");
            exteriorObject.transform.SetParent(car, false);
            exteriorObject.transform.localPosition = new Vector3(0f, 3.1f, -7f);
            Camera exterior = exteriorObject.AddComponent<Camera>();
            exterior.fieldOfView = 68f;
            exterior.nearClipPlane = 0.1f;
            exteriorObject.AddComponent<AudioListener>();
            exteriorObject.AddComponent<PrototypeLookAt>().Target = car;

            GameObject interiorObject = new GameObject("Player Interior Camera");
            interiorObject.transform.SetParent(car, false);
            interiorObject.transform.localPosition = new Vector3(-0.43f, 1.52f, 0.48f);
            interiorObject.transform.localRotation = Quaternion.identity;
            Camera interior = interiorObject.AddComponent<Camera>();
            interior.fieldOfView = 78f;
            interior.enabled = false;

            CameraRig rig = car.gameObject.AddComponent<CameraRig>();
            rig.Configure(exterior, interior);
        }

        private void CreateTouchHud(VehicleInput input, CameraRig cameraRig)
        {
            if (EventSystem.current == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }

            GameObject canvasObject = new GameObject("Driving Touch Controls");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 20;
            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            CreateControlButton(canvas.transform, "◀", new Vector2(0.08f, 0.16f), new Vector2(150f, 130f), input, VehicleInput.MobileControl.SteerLeft);
            CreateControlButton(canvas.transform, "▶", new Vector2(0.25f, 0.16f), new Vector2(150f, 130f), input, VehicleInput.MobileControl.SteerRight);
            CreateControlButton(canvas.transform, "GAS", new Vector2(0.91f, 0.17f), new Vector2(170f, 145f), input, VehicleInput.MobileControl.Throttle);
            CreateControlButton(canvas.transform, "BREMSE", new Vector2(0.75f, 0.12f), new Vector2(155f, 115f), input, VehicleInput.MobileControl.Brake);
            CreateControlButton(canvas.transform, "HANDBREMSE", new Vector2(0.75f, 0.28f), new Vector2(155f, 90f), input, VehicleInput.MobileControl.Handbrake);
            CreateActionButton(canvas.transform, "KAMERA  C", new Vector2(0.89f, 0.91f), new Vector2(190f, 70f), cameraRig.ToggleCamera);
        }

        private void CreateControlButton(Transform parent, string label, Vector2 anchor, Vector2 size,
            VehicleInput input, VehicleInput.MobileControl control)
        {
            GameObject buttonObject = CreateButtonBase(parent, label, anchor, size);
            MobileHoldButton hold = buttonObject.AddComponent<MobileHoldButton>();
            // Configure through the public setup method to avoid Inspector-only wiring.
            hold.Configure(input, control);
        }

        private void CreateActionButton(Transform parent, string label, Vector2 anchor, Vector2 size, UnityEngine.Events.UnityAction action)
        {
            GameObject buttonObject = CreateButtonBase(parent, label, anchor, size);
            buttonObject.GetComponent<Button>().onClick.AddListener(action);
        }

        private GameObject CreateButtonBase(Transform parent, string label, Vector2 anchor, Vector2 size)
        {
            GameObject buttonObject = new GameObject("Touch Button - " + label);
            buttonObject.transform.SetParent(parent, false);
            RectTransform rect = buttonObject.AddComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = size;

            Image image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.04f, 0.08f, 0.13f, 0.72f);
            Button button = buttonObject.AddComponent<Button>();
            button.targetGraphic = image;

            GameObject textObject = new GameObject("Label");
            textObject.transform.SetParent(buttonObject.transform, false);
            RectTransform textRect = textObject.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            Text text = textObject.AddComponent<Text>();
            text.text = label;
            text.alignment = TextAnchor.MiddleCenter;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 28;
            text.fontStyle = FontStyle.Bold;
            text.color = Color.white;
            return buttonObject;
        }

        private Material MakeMaterial(Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            Material material = new Material(shader);
            material.color = color;
            return material;
        }

        private void OnGUI()
        {
            if (vehicle == null) return;
            if (hudStyle == null)
            {
                hudStyle = new GUIStyle(GUI.skin.label);
                hudStyle.fontSize = Mathf.Max(18, Screen.height / 35);
                hudStyle.fontStyle = FontStyle.Bold;
                hudStyle.normal.textColor = Color.white;
            }

            GUI.Label(new Rect(22f, 18f, 700f, 42f),
                "CAR DRIVE PARKING   |   " + Mathf.RoundToInt(vehicle.SpeedKph) + " km/h", hudStyle);
            GUI.Label(new Rect(22f, 58f, 900f, 36f),
                "WASD / Pfeiltasten  •  Leertaste: Handbremse  •  C: Kamera", hudStyle);
        }
    }
}
