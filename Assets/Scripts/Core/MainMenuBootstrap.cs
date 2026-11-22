using UnityEngine;
using UnityEngine.UI;

namespace CarDriveParking
{
    /// <summary>
    /// Builds the first CAR DRIVE PARKING main menu at runtime.
    /// Add this component to an empty GameObject in a new Unity scene.
    /// Uses Unity's built-in UI system so no extra packages are required.
    /// </summary>
    public class MainMenuBootstrap : MonoBehaviour
    {
        private Canvas canvas;
        private GameObject mainPanel;
        private GameObject settingsPanel;
        private Text messageText;

        private Font uiFont;
        private RenderTexture vehiclePreviewTexture;

        private void Start()
        {
            uiFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
            BuildInterface();
            ShowMainMenu();
        }

        private void BuildInterface()
        {
            GameObject canvasObject = new GameObject("CAR DRIVE PARKING UI");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

            mainPanel = CreatePanel("Main Menu", canvas.transform, new Color(0.018f, 0.028f, 0.045f, 1f));
            CreateBlock(mainPanel.transform, "Right blue stage", new Vector2(0.52f, 0f), new Vector2(1f, 1f), new Color(0.025f, 0.075f, 0.12f, 1f));
            CreateBlock(mainPanel.transform, "Stage glow", new Vector2(0.72f, 0.12f), new Vector2(1f, 0.88f), new Color(0.035f, 0.12f, 0.19f, 0.55f));
            BuildVehicleShowcase(mainPanel.transform);
            CreateBlock(mainPanel.transform, "Accent line", new Vector2(0.055f, 0.17f), new Vector2(0.058f, 0.83f), new Color(0.08f, 0.62f, 0.95f, 1f));

            CreateTextAligned(mainPanel.transform, "CDP  /  MAIN MENU", 17, new Vector2(0.085f, 0.88f), new Vector2(0.42f, 0.93f), FontStyle.Bold, TextAnchor.MiddleLeft, new Color(0.28f, 0.72f, 0.98f));
            CreateTextAligned(mainPanel.transform, "CAR DRIVE", 58, new Vector2(0.085f, 0.73f), new Vector2(0.48f, 0.83f), FontStyle.Bold, TextAnchor.MiddleLeft, Color.white);
            CreateTextAligned(mainPanel.transform, "PARKING", 58, new Vector2(0.085f, 0.64f), new Vector2(0.48f, 0.74f), FontStyle.Bold, TextAnchor.MiddleLeft, Color.white);
            CreateTextAligned(mainPanel.transform, "DEINE STADT. DEIN AUTO. DEINE FREIHEIT.", 17, new Vector2(0.087f, 0.585f), new Vector2(0.49f, 0.63f), FontStyle.Normal, TextAnchor.MiddleLeft, new Color(0.62f, 0.72f, 0.82f));

            CreateButtonAt(mainPanel.transform, "SPIELEN", new Vector2(0.085f, 0.47f), new Vector2(0.34f, 0.535f), ShowPlayMessage, true);
            CreateButtonAt(mainPanel.transform, "EINSTELLUNGEN", new Vector2(0.085f, 0.385f), new Vector2(0.34f, 0.45f), ShowSettings, false);
            CreateButtonAt(mainPanel.transform, "ÜBER DAS SPIEL", new Vector2(0.085f, 0.30f), new Vector2(0.34f, 0.365f), ShowAboutMessage, false);
            CreateButtonAt(mainPanel.transform, "BEENDEN", new Vector2(0.085f, 0.215f), new Vector2(0.34f, 0.28f), () => Application.Quit(), false);

            CreateTextAligned(mainPanel.transform, "NEUSTADT  •  OPEN WORLD", 16, new Vector2(0.085f, 0.075f), new Vector2(0.42f, 0.12f), FontStyle.Bold, TextAnchor.MiddleLeft, new Color(0.36f, 0.63f, 0.78f));
            CreateTextAligned(mainPanel.transform, "VELTOR  /  V6 S", 20, new Vector2(0.60f, 0.19f), new Vector2(0.92f, 0.25f), FontStyle.Bold, TextAnchor.MiddleRight, new Color(0.48f, 0.78f, 0.96f));
            CreateTextAligned(mainPanel.transform, "FIRST LOOK", 14, new Vector2(0.60f, 0.145f), new Vector2(0.92f, 0.19f), FontStyle.Normal, TextAnchor.MiddleRight, new Color(0.55f, 0.65f, 0.73f));

            messageText = CreateTextAligned(mainPanel.transform, "", 16, new Vector2(0.085f, 0.13f), new Vector2(0.48f, 0.18f), FontStyle.Normal, TextAnchor.MiddleLeft, new Color(0.95f, 0.76f, 0.35f));
            settingsPanel = CreatePanel("Settings", canvas.transform, new Color(0.018f, 0.028f, 0.045f, 1f));
            CreateBlock(settingsPanel.transform, "Settings accent", new Vector2(0.055f, 0.12f), new Vector2(0.06f, 0.88f), new Color(0.08f, 0.62f, 0.95f, 1f));
            BuildSettings();
            settingsPanel.SetActive(false);
        }

        private void BuildVehicleShowcase(Transform parent)
        {
            vehiclePreviewTexture = new RenderTexture(1024, 768, 16, RenderTextureFormat.ARGB32);
            vehiclePreviewTexture.name = "CDP Vehicle Preview";

            GameObject cameraObject = new GameObject("Menu Vehicle Camera");
            Camera previewCamera = cameraObject.AddComponent<Camera>();
            previewCamera.targetTexture = vehiclePreviewTexture;
            previewCamera.clearFlags = CameraClearFlags.SolidColor;
            previewCamera.backgroundColor = new Color(0.025f, 0.075f, 0.12f, 1f);
            previewCamera.fieldOfView = 34f;
            previewCamera.nearClipPlane = 0.1f;
            previewCamera.farClipPlane = 100f;
            previewCamera.cullingMask = 1 << 8;
            cameraObject.transform.position = new Vector3(5.2f, 2.8f, -6.5f);
            cameraObject.transform.LookAt(new Vector3(0f, 0.9f, 0f));

            GameObject lightObject = new GameObject("Menu Vehicle Key Light");
            Light keyLight = lightObject.AddComponent<Light>();
            keyLight.type = LightType.Directional;
            keyLight.intensity = 1.35f;
            lightObject.transform.rotation = Quaternion.Euler(35f, -30f, 0f);
            lightObject.layer = 8;

            GameObject fillObject = new GameObject("Menu Vehicle Fill Light");
            Light fillLight = fillObject.AddComponent<Light>();
            fillLight.type = LightType.Directional;
            fillLight.intensity = 0.65f;
            fillLight.color = new Color(0.35f, 0.65f, 1f);
            fillObject.transform.rotation = Quaternion.Euler(20f, 145f, 0f);
            fillObject.layer = 8;

            GameObject car = new GameObject("VELTOR V6 S - Menu Preview");
            car.layer = 8;
            car.transform.position = Vector3.zero;
            CreateShowcasePart(car.transform, PrimitiveType.Cube, "Body shell", new Vector3(0f, 0.82f, 0f), new Vector3(1.85f, 0.62f, 4.25f), new Color(0.035f, 0.22f, 0.48f));
            CreateShowcasePart(car.transform, PrimitiveType.Cube, "Cabin base", new Vector3(0f, 1.38f, -0.15f), new Vector3(1.48f, 0.62f, 1.95f), new Color(0.025f, 0.07f, 0.11f));

            for (int side = -1; side <= 1; side += 2)
            {
                for (int axle = -1; axle <= 1; axle += 2)
                {
                    GameObject wheel = CreateShowcasePart(car.transform, PrimitiveType.Cylinder,
                        "Preview Wheel", new Vector3(side * 0.94f, 0.42f, axle * 1.35f),
                        new Vector3(0.72f, 0.16f, 0.72f), new Color(0.012f, 0.015f, 0.02f));
                    wheel.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                }
            }

            VehicleVisualBuilder.Build(car.transform);
            SetLayerRecursively(car, 8);
            car.AddComponent<MenuVehicleRotation>();

            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Menu Vehicle Floor";
            floor.layer = 8;
            floor.transform.position = new Vector3(0f, 0.02f, 0f);
            floor.transform.localScale = new Vector3(0.7f, 1f, 0.7f);
            floor.GetComponent<Renderer>().material = CreateShowcaseMaterial(new Color(0.035f, 0.085f, 0.12f));
            SetLayerRecursively(floor, 8);

            GameObject preview = new GameObject("Vehicle Preview Window", typeof(RectTransform), typeof(RawImage));
            preview.transform.SetParent(parent, false);
            RectTransform rect = preview.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.52f, 0.27f);
            rect.anchorMax = new Vector2(1f, 0.83f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            RawImage rawImage = preview.GetComponent<RawImage>();
            rawImage.texture = vehiclePreviewTexture;
            rawImage.color = Color.white;
            rawImage.raycastTarget = false;
        }

        private GameObject CreateShowcasePart(Transform parent, PrimitiveType type, string partName, Vector3 position, Vector3 scale, Color color)
        {
            GameObject part = GameObject.CreatePrimitive(type);
            part.name = partName;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = position;
            part.transform.localScale = scale;
            part.layer = 8;
            Renderer renderer = part.GetComponent<Renderer>();
            renderer.material = CreateShowcaseMaterial(color);
            Collider collider = part.GetComponent<Collider>();
            if (collider != null) Destroy(collider);
            return part;
        }

        private Material CreateShowcaseMaterial(Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            Material material = new Material(shader);
            material.color = color;
            material.SetFloat("_Metallic", 0.35f);
            material.SetFloat("_Smoothness", 0.55f);
            return material;
        }

        private void SetLayerRecursively(GameObject target, int layer)
        {
            target.layer = layer;
            foreach (Transform child in target.transform)
                SetLayerRecursively(child.gameObject, layer);
        }

        private void OnDestroy()
        {
            if (vehiclePreviewTexture != null)
            {
                vehiclePreviewTexture.Release();
                Destroy(vehiclePreviewTexture);
            }
        }

        private void ShowPlayMessage()
        {
            ShowMessage("Die Spielwelt Neustadt wird als Nächstes gebaut. Dein Spielstand bleibt offline.");
        }

        private void ShowAboutMessage()
        {
            ShowMessage("Ein eigenständiger Open-World-Fahr- und Parksimulator von CAR DRIVE PARKING.");
        }

        private void CreateBlock(Transform parent, string blockName, Vector2 anchorMin, Vector2 anchorMax, Color color)
        {
            GameObject obj = new GameObject(blockName, typeof(RectTransform), typeof(Image));
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            Image image = obj.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
        }

        private Text CreateTextAligned(Transform parent, string value, int size, Vector2 anchorMin, Vector2 anchorMax, FontStyle style, TextAnchor alignment, Color color)
        {
            GameObject obj = new GameObject("Text - " + value, typeof(RectTransform), typeof(Text));
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            Text text = obj.GetComponent<Text>();
            text.font = uiFont;
            text.text = value;
            text.fontSize = size;
            text.fontStyle = style;
            text.color = color;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        private Button CreateButtonAt(Transform parent, string label, Vector2 anchorMin, Vector2 anchorMax, UnityEngine.Events.UnityAction action, bool primary)
        {
            GameObject obj = new GameObject("Button - " + label, typeof(RectTransform), typeof(Image), typeof(Button));
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            Image image = obj.GetComponent<Image>();
            image.color = primary ? new Color(0.035f, 0.39f, 0.66f, 1f) : new Color(0.055f, 0.095f, 0.13f, 0.92f);
            Button button = obj.GetComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(action);
            Text text = CreateTextAligned(obj.transform, label, 19, new Vector2(0.08f, 0f), new Vector2(0.92f, 1f), FontStyle.Bold, TextAnchor.MiddleLeft, Color.white);
            text.raycastTarget = false;
            ColorBlock colors = button.colors;
            colors.highlightedColor = new Color(0.10f, 0.48f, 0.72f, 1f);
            colors.pressedColor = new Color(0.025f, 0.25f, 0.43f, 1f);
            button.colors = colors;
            return button;
        }

        private void BuildSettings()
        {
            CreateText(settingsPanel.transform, "EINSTELLUNGEN", 42, new Vector2(0.5f, 0.88f), new Vector2(0.5f, 0.88f), new Vector2(700, 70), FontStyle.Bold);

            CreateSettingLabel("Grafikqualität", 0.72f);
            CreateChoice(settingsPanel.transform, new Vector2(0.5f, 0.72f), new[] { "Niedrig", "Mittel", "Hoch", "Ultra" }, PlayerPrefs.GetInt("CDP_Quality", 2), value =>
            {
                QualitySettings.SetQualityLevel(value);
                PlayerPrefs.SetInt("CDP_Quality", value);
            });

            CreateSettingLabel("Auflösung", 0.61f);
            CreateChoice(settingsPanel.transform, new Vector2(0.5f, 0.61f), new[] { "1280 x 720", "1600 x 900", "1920 x 1080" }, PlayerPrefs.GetInt("CDP_Resolution", 2), value =>
            {
                int[,] sizes = { { 1280, 720 }, { 1600, 900 }, { 1920, 1080 } };
                Screen.SetResolution(sizes[value, 0], sizes[value, 1], Screen.fullScreen);
                PlayerPrefs.SetInt("CDP_Resolution", value);
            });

            CreateSettingLabel("Sprache", 0.50f);
            CreateChoice(settingsPanel.transform, new Vector2(0.5f, 0.50f), new[] { "Deutsch", "English" }, PlayerPrefs.GetInt("CDP_Language", 0), value => PlayerPrefs.SetInt("CDP_Language", value));

            CreateSettingLabel("Steuerung", 0.39f);
            CreateChoice(settingsPanel.transform, new Vector2(0.5f, 0.39f), new[] { "Automatisch", "Tasten / Controller", "Touch-Steuerung" }, PlayerPrefs.GetInt("CDP_Controls", 0), value => PlayerPrefs.SetInt("CDP_Controls", value));

            CreateSettingLabel("Lautstärke", 0.28f);
            int volumePercent = Mathf.RoundToInt(PlayerPrefs.GetFloat("CDP_Volume", 0.8f) * 100f);
            CreateChoice(settingsPanel.transform, new Vector2(0.5f, 0.28f), new[] { "0%", "20%", "40%", "60%", "80%", "100%" }, Mathf.Clamp(Mathf.RoundToInt(volumePercent / 20f), 0, 5), value =>
            {
                float volume = value * 0.2f;
                AudioListener.volume = volume;
                PlayerPrefs.SetFloat("CDP_Volume", volume);
            });

            CreateSettingLabel("Vollbild", 0.19f);
            CreateChoice(settingsPanel.transform, new Vector2(0.5f, 0.19f), new[] { "Aus", "Ein" }, PlayerPrefs.GetInt("CDP_Fullscreen", 1), value =>
            {
                Screen.fullScreen = value == 1;
                PlayerPrefs.SetInt("CDP_Fullscreen", value);
            });

            CreateButton(settingsPanel.transform, "ZURÜCK", new Vector2(0.5f, 0.08f), ShowMainMenu);
        }

        private GameObject CreatePanel(string panelName, Transform parent, Color color)
        {
            GameObject panel = new GameObject(panelName, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            panel.GetComponent<Image>().color = color;
            return panel;
        }

        private Text CreateText(Transform parent, string value, int size, Vector2 anchorMin, Vector2 anchorMax, Vector2 dimensions, FontStyle style)
        {
            GameObject obj = new GameObject("Text - " + value, typeof(RectTransform), typeof(Text));
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.sizeDelta = dimensions;
            rect.anchoredPosition = Vector2.zero;
            Text text = obj.GetComponent<Text>();
            text.font = uiFont;
            text.text = value;
            text.fontSize = size;
            text.fontStyle = style;
            text.color = Color.white;
            text.alignment = TextAnchor.MiddleCenter;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            return text;
        }

        private Button CreateButton(Transform parent, string label, Vector2 anchor, UnityEngine.Events.UnityAction action)
        {
            GameObject obj = new GameObject("Button - " + label, typeof(RectTransform), typeof(Image), typeof(Button));
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.sizeDelta = new Vector2(420, 62);
            rect.anchoredPosition = Vector2.zero;
            Image image = obj.GetComponent<Image>();
            image.color = new Color(0.10f, 0.32f, 0.52f, 1f);
            Button button = obj.GetComponent<Button>();
            button.targetGraphic = image;
            if (action != null) button.onClick.AddListener(action);
            CreateText(obj.transform, label, 22, Vector2.zero, Vector2.one, Vector2.zero, FontStyle.Bold);
            return button;
        }

        private void CreateSettingLabel(string label, float y)
        {
            CreateText(settingsPanel.transform, label, 19, new Vector2(0.18f, y - 0.025f), new Vector2(0.42f, y + 0.025f), new Vector2(0, 0), FontStyle.Normal);
        }

        private void CreateChoice(Transform parent, Vector2 anchor, string[] options, int initialValue, System.Action<int> onChanged)
        {
            int selected = Mathf.Clamp(initialValue, 0, options.Length - 1);
            Button button = CreateButton(parent, options[selected] + "  >", anchor, null);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(340, 48);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(220, 0);
            Text label = button.GetComponentInChildren<Text>();
            button.onClick.AddListener(() =>
            {
                selected = (selected + 1) % options.Length;
                label.text = options[selected] + "  >";
                onChanged?.Invoke(selected);
            });
            onChanged?.Invoke(selected);
        }

        private void ShowSettings()
        {
            mainPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }

        private void ShowMainMenu()
        {
            settingsPanel.SetActive(false);
            mainPanel.SetActive(true);
            if (messageText != null) messageText.text = "";
        }

        private void ShowMessage(string message)
        {
            if (messageText != null) messageText.text = message;
        }
    }
}
