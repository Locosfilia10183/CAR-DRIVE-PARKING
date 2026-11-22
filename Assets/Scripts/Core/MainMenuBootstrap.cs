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
            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            canvasObject.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

            mainPanel = CreatePanel("Main Menu", canvas.transform, new Color(0.035f, 0.055f, 0.08f, 1f));
            CreateText(mainPanel.transform, "CAR DRIVE", 58, new Vector2(0.5f, 0.79f), new Vector2(0.5f, 0.79f), new Vector2(800, 90), FontStyle.Bold);
            CreateText(mainPanel.transform, "PARKING", 58, new Vector2(0.5f, 0.70f), new Vector2(0.5f, 0.70f), new Vector2(800, 90), FontStyle.Bold);
            CreateText(mainPanel.transform, "OPEN WORLD • DRIVING • PARKING", 19, new Vector2(0.5f, 0.63f), new Vector2(0.5f, 0.63f), new Vector2(700, 40), FontStyle.Normal);

            CreateButton(mainPanel.transform, "SPIELEN", new Vector2(0.5f, 0.49f), () => ShowMessage("Die Spielwelt Neustadt wird als Nächstes gebaut."));
            CreateButton(mainPanel.transform, "EINSTELLUNGEN", new Vector2(0.5f, 0.39f), ShowSettings);
            CreateButton(mainPanel.transform, "ÜBER DAS SPIEL", new Vector2(0.5f, 0.29f), () => ShowMessage("CAR DRIVE PARKING – ein eigenes Open-World-Fahr- und Parkspiel."));
            CreateButton(mainPanel.transform, "BEENDEN", new Vector2(0.5f, 0.19f), () => Application.Quit());

            messageText = CreateText(mainPanel.transform, "", 18, new Vector2(0.5f, 0.08f), new Vector2(0.5f, 0.08f), new Vector2(1000, 50), FontStyle.Normal);
            settingsPanel = CreatePanel("Settings", canvas.transform, new Color(0.035f, 0.055f, 0.08f, 1f));
            BuildSettings();
            settingsPanel.SetActive(false);
        }

        private void BuildSettings()
        {
            CreateText(settingsPanel.transform, "EINSTELLUNGEN", 42, new Vector2(0.5f, 0.88f), new Vector2(0.5f, 0.88f), new Vector2(700, 70), FontStyle.Bold);

            CreateSettingLabel("Grafikqualität", 0.72f);
            Dropdown quality = CreateDropdown(settingsPanel.transform, new Vector2(0.5f, 0.72f), new[] { "Niedrig", "Mittel", "Hoch", "Ultra" });
            quality.value = Mathf.Clamp(PlayerPrefs.GetInt("CDP_Quality", 2), 0, 3);
            quality.onValueChanged.AddListener(value => { QualitySettings.SetQualityLevel(value); PlayerPrefs.SetInt("CDP_Quality", value); });

            CreateSettingLabel("Auflösung", 0.61f);
            Dropdown resolution = CreateDropdown(settingsPanel.transform, new Vector2(0.5f, 0.61f), new[] { "1280 x 720", "1600 x 900", "1920 x 1080" });
            resolution.onValueChanged.AddListener(value =>
            {
                int[,] sizes = { { 1280, 720 }, { 1600, 900 }, { 1920, 1080 } };
                Screen.SetResolution(sizes[value, 0], sizes[value, 1], Screen.fullScreen);
                PlayerPrefs.SetInt("CDP_Resolution", value);
            });
            resolution.value = Mathf.Clamp(PlayerPrefs.GetInt("CDP_Resolution", 2), 0, 2);

            CreateSettingLabel("Sprache", 0.50f);
            Dropdown language = CreateDropdown(settingsPanel.transform, new Vector2(0.5f, 0.50f), new[] { "Deutsch", "English" });
            language.value = Mathf.Clamp(PlayerPrefs.GetInt("CDP_Language", 0), 0, 1);
            language.onValueChanged.AddListener(value => PlayerPrefs.SetInt("CDP_Language", value));

            CreateSettingLabel("Steuerung", 0.39f);
            Dropdown controls = CreateDropdown(settingsPanel.transform, new Vector2(0.5f, 0.39f), new[] { "Automatisch", "Tasten / Controller", "Touch-Steuerung" });
            controls.value = Mathf.Clamp(PlayerPrefs.GetInt("CDP_Controls", 0), 0, 2);
            controls.onValueChanged.AddListener(value => PlayerPrefs.SetInt("CDP_Controls", value));

            CreateSettingLabel("Lautstärke", 0.28f);
            Slider volume = CreateSlider(settingsPanel.transform, new Vector2(0.5f, 0.28f));
            volume.value = PlayerPrefs.GetFloat("CDP_Volume", 0.8f);
            AudioListener.volume = volume.value;
            volume.onValueChanged.AddListener(value => { AudioListener.volume = value; PlayerPrefs.SetFloat("CDP_Volume", value); });

            Toggle fullscreen = CreateToggle(settingsPanel.transform, "Vollbild", new Vector2(0.5f, 0.19f));
            fullscreen.isOn = PlayerPrefs.GetInt("CDP_Fullscreen", 1) == 1;
            Screen.fullScreen = fullscreen.isOn;
            fullscreen.onValueChanged.AddListener(value => { Screen.fullScreen = value; PlayerPrefs.SetInt("CDP_Fullscreen", value ? 1 : 0); });

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
            button.onClick.AddListener(action);
            CreateText(obj.transform, label, 22, Vector2.zero, Vector2.one, Vector2.zero, FontStyle.Bold);
            return button;
        }

        private void CreateSettingLabel(string label, float y)
        {
            CreateText(settingsPanel.transform, label, 19, new Vector2(0.18f, y - 0.025f), new Vector2(0.42f, y + 0.025f), new Vector2(0, 0), FontStyle.Normal);
        }

        private Dropdown CreateDropdown(Transform parent, Vector2 anchor, string[] options)
        {
            GameObject obj = new GameObject("Dropdown", typeof(RectTransform), typeof(Image), typeof(Dropdown));
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.sizeDelta = new Vector2(340, 48);
            rect.anchoredPosition = new Vector2(220, 0);
            obj.GetComponent<Image>().color = new Color(0.14f, 0.18f, 0.23f, 1f);
            Dropdown dropdown = obj.GetComponent<Dropdown>();
            dropdown.captionText = CreateText(obj.transform, options[0], 18, new Vector2(0.05f, 0f), new Vector2(0.95f, 1f), Vector2.zero, FontStyle.Normal);
            dropdown.options.Clear();
            foreach (string option in options) dropdown.options.Add(new Dropdown.OptionData(option));
            return dropdown;
        }

        private Slider CreateSlider(Transform parent, Vector2 anchor)
        {
            GameObject obj = new GameObject("Volume Slider", typeof(RectTransform), typeof(Slider));
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.sizeDelta = new Vector2(340, 30);
            rect.anchoredPosition = new Vector2(220, 0);
            Slider slider = obj.GetComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            return slider;
        }

        private Toggle CreateToggle(Transform parent, string label, Vector2 anchor)
        {
            GameObject obj = new GameObject("Toggle - " + label, typeof(RectTransform), typeof(Toggle), typeof(Image));
            obj.transform.SetParent(parent, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.sizeDelta = new Vector2(340, 40);
            rect.anchoredPosition = new Vector2(220, 0);
            Toggle toggle = obj.GetComponent<Toggle>();
            toggle.targetGraphic = obj.GetComponent<Image>();
            CreateText(obj.transform, label, 18, Vector2.zero, Vector2.one, Vector2.zero, FontStyle.Normal);
            return toggle;
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
