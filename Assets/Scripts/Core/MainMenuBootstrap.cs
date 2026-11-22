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

            // Temporary abstract vehicle artwork; a real 3D showroom car comes later.
            CreateBlock(mainPanel.transform, "Vehicle shadow", new Vector2(0.61f, 0.34f), new Vector2(0.94f, 0.39f), new Color(0.008f, 0.018f, 0.028f, 0.8f));
            CreateBlock(mainPanel.transform, "Vehicle body silhouette", new Vector2(0.64f, 0.39f), new Vector2(0.91f, 0.51f), new Color(0.055f, 0.22f, 0.34f, 1f));
            CreateBlock(mainPanel.transform, "Vehicle cabin silhouette", new Vector2(0.70f, 0.50f), new Vector2(0.85f, 0.61f), new Color(0.035f, 0.12f, 0.19f, 1f));
            CreateBlock(mainPanel.transform, "Vehicle front highlight", new Vector2(0.87f, 0.43f), new Vector2(0.915f, 0.46f), new Color(0.18f, 0.72f, 0.98f, 1f));
            CreateBlock(mainPanel.transform, "Vehicle rear highlight", new Vector2(0.635f, 0.43f), new Vector2(0.66f, 0.46f), new Color(0.95f, 0.22f, 0.18f, 1f));
            CreateBlock(mainPanel.transform, "Front wheel", new Vector2(0.82f, 0.35f), new Vector2(0.87f, 0.42f), new Color(0.012f, 0.018f, 0.025f, 1f));
            CreateBlock(mainPanel.transform, "Rear wheel", new Vector2(0.67f, 0.35f), new Vector2(0.72f, 0.42f), new Color(0.012f, 0.018f, 0.025f, 1f));

            messageText = CreateTextAligned(mainPanel.transform, "", 16, new Vector2(0.085f, 0.13f), new Vector2(0.48f, 0.18f), FontStyle.Normal, TextAnchor.MiddleLeft, new Color(0.95f, 0.76f, 0.35f));
            settingsPanel = CreatePanel("Settings", canvas.transform, new Color(0.018f, 0.028f, 0.045f, 1f));
            CreateBlock(settingsPanel.transform, "Settings accent", new Vector2(0.055f, 0.12f), new Vector2(0.06f, 0.88f), new Color(0.08f, 0.62f, 0.95f, 1f));
            BuildSettings();
            settingsPanel.SetActive(false);
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
