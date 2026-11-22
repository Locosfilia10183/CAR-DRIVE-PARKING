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
