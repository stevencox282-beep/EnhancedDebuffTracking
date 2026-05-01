using Il2Cpp;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.Events;

namespace EnhancedDebuffTracking
{
    // Box on the screen with 3 labels inside it
    public class DebuffPanel : MonoBehaviour
    {
        private static string LeftGameObjectName = "EDT_leftTextObject_EDT";
        private static string RightGameObjectName = "EDT_rightTextObject_EDT";
        private static string MiddleGameObjectName = "EDT_middleTextObject_EDT";

        // Holds the panel
        private static UIWindowPanel gUiWindowPanel  = null;

        // Displays a panel with to contain the data we want
        public void DisplayPanel(string panelName, Transform parentPanel, Vector2 panelSize)
        {
            // Setup the general panel parameters
            GameObject gameObject = new GameObject(panelName);
            // Add the panel to the Mid, this ensures we get rendered
            gameObject.transform.SetParent(parentPanel);
            gameObject.layer = Layers.UI;

            // Add the necessary component for a panel
            CanvasRenderer canvasRenderer = gameObject.AddComponent<CanvasRenderer>();
            CanvasGroup    canvasGroup    = gameObject.AddComponent<CanvasGroup>();
            UIDraggable    uiDraggable    = gameObject.AddComponent<UIDraggable>();
            RectTransform  rectTransform  = gameObject.AddComponent<RectTransform>();
            gUiWindowPanel = gameObject.AddComponent<UIWindowPanel>();

            // Block Raycasts to work around wonky click detection on the close button due other UI elements overlapping the close button image
            // I am not going to spend time making all my TextMesh's layout perfectly for this mod so block raycasts instead
            canvasGroup.blocksRaycasts = true;

            // Setup the Window Panel
            uiDraggable._windowPanel = gUiWindowPanel;
            gUiWindowPanel.CanvasGroup = canvasGroup;
            gUiWindowPanel._displayName = panelName;

            // Setup the default position of the panel and its general parameters
            rectTransform.sizeDelta = panelSize;
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchoredPosition = new Vector2(-(panelSize.x / 2), panelSize.y / 2);
            
            // Add the MANDATORY elements to a panel, the compilor will not error if you don't do this but nothing will work
            BuildCloseButtonAndBackground(parentPanel, gameObject);

            // Add in my text Meshs that will ultimate display the data in the panel
            BuildTextMeshs();

            // Load the panel and display it
            gUiWindowPanel.Hide();
        }

        // Constructs the close button and set the background
        private void BuildCloseButtonAndBackground(Transform parentPanel, GameObject gameObject)
        {
            // Source for copying button and backgrounds
            UITutorialPopup tutorialPopup = UIPanelRoots.Instance.Mid.transform.GetComponentInChildren<UITutorialPopup>();
            Transform tutorialButton = tutorialPopup.transform.GetChild(0);

            // Initialise the background for the new panel (MANDATORY)
            UnityEngine.UI.Image imageToCopy = tutorialPopup.GetComponent<UnityEngine.UI.Image>();
            var image = gameObject.AddComponent<UnityEngine.UI.Image>();
            image.type = UnityEngine.UI.Image.Type.Sliced;
            image.sprite = imageToCopy.sprite;

            // Initialise the close button of the panel (MANDATORY)
            var closeButton = GameObject.Instantiate(tutorialButton, tutorialButton.transform.position, tutorialButton.transform.rotation, gUiWindowPanel.transform);
            var closeButtonRect = closeButton.GetComponent<RectTransform>();
            closeButtonRect.sizeDelta = new Vector2(30, 30);
            closeButtonRect.anchoredPosition = new Vector2(-13.5f, -12); // Tiny size, top right corner, this ruins the box detection though
            closeButtonRect.pivot = new Vector2(0f, 0f);

            // Initialise on click behaviour of the close button
            var buttonComponent = closeButton.GetComponent<UnityEngine.UI.Button>();
            buttonComponent.onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddCall(new InvokableCall(new Action(() =>
            {
                // Actually unloads the panel, not hide
                gUiWindowPanel.Hide();
            })));
            // Make clicking sound
            buttonComponent.onClick.AddCall(new InvokableCall(new Action(() =>
            {
                closeButton.GetComponent<UI_Audio_Function>().Play_UI_Generic_Click();
            })));
        }

        // Setup the text meshs inside the panel that will display the data we want
        private void BuildTextMeshs()
        {
            // Create new GameObjects for the time display
            GameObject _leftGameObject = new GameObject(LeftGameObjectName);
            GameObject _middleGameObject = new GameObject(MiddleGameObjectName);
            GameObject _rightGameObject = new GameObject(RightGameObjectName);
            // Set its parent to the new window panel (which is parented to Mid)
            _leftGameObject.transform.SetParent(gUiWindowPanel.transform, false);
            _middleGameObject.transform.SetParent(gUiWindowPanel.transform, false);
            _rightGameObject.transform.SetParent(gUiWindowPanel.transform, false);

            // Add and configure the TextMeshPros for rendering the time data
            TextMeshProUGUI _leftTextMeshPro = _leftGameObject.AddComponent<TextMeshProUGUI>();
            _leftTextMeshPro.alignment = TextAlignmentOptions.Left;
            _leftTextMeshPro.fontSize = Globals.FontSize;
            _leftTextMeshPro.color = Color.yellow;
            _leftTextMeshPro.text = "";

            TextMeshProUGUI _middleTextMeshPro = _middleGameObject.AddComponent<TextMeshProUGUI>();
            _middleTextMeshPro.alignment = TextAlignmentOptions.Left;
            _middleTextMeshPro.fontSize = Globals.FontSize;
            _middleTextMeshPro.color = Color.yellow;
            _middleTextMeshPro.text = "";

            TextMeshProUGUI _rightTextMeshPro = _rightGameObject.AddComponent<TextMeshProUGUI>();
            _rightTextMeshPro.alignment = TextAlignmentOptions.Left;
            _rightTextMeshPro.fontSize = Globals.FontSize;
            _rightTextMeshPro.color = Color.yellow;
            _rightTextMeshPro.text = "";

            // Set up the RectTransform to position the texts correctly
            var leftTextRectTransform = _leftTextMeshPro.rectTransform;
            leftTextRectTransform.sizeDelta = new Vector2(Globals.MeshWidth, Globals.PanelMeshHeight);
            leftTextRectTransform.anchorMin = new Vector2(0.1f, 0f); // Just off Left Center
            leftTextRectTransform.anchorMax = new Vector2(0.1f, 0f); // Just off Left Center
            leftTextRectTransform.anchoredPosition = new Vector2(0f, 0f);
            leftTextRectTransform.pivot = new Vector2(0f, 0f);

            var middleTextRectTransform = _middleTextMeshPro.rectTransform;
            middleTextRectTransform.sizeDelta = new Vector2(Globals.MeshWidth, Globals.PanelMeshHeight);
            middleTextRectTransform.anchorMin = new Vector2(0.40f, 0f); // Middle Center
            middleTextRectTransform.anchorMax = new Vector2(0.40f, 0f); // Middle Center
            middleTextRectTransform.anchoredPosition = new Vector2(0, 0);
            middleTextRectTransform.pivot = new Vector2(0f, 0f);

            var rightTextRectTransform = _rightTextMeshPro.rectTransform;
            rightTextRectTransform.sizeDelta = new Vector2(Globals.MeshWidth, Globals.PanelMeshHeight);
            rightTextRectTransform.anchorMin = new Vector2(0.70f, 0f); // Right Center
            rightTextRectTransform.anchorMax = new Vector2(0.70f, 0f); // Right Center
            rightTextRectTransform.anchoredPosition = new Vector2(0, 0);
            rightTextRectTransform.pivot = new Vector2(0f, 0f);
        }

        //Update the text displayed in the Clock Box
        public void UpdateDebuffPanel(List<DebuffData> debuffList)
        {
            // Try and stop unwanted access to the panel to prevent exceptions
            if (gUiWindowPanel.isActiveAndEnabled && gUiWindowPanel.IsVisible)
            {
                // Update the panel data
                Transform leftGameObject = gUiWindowPanel.transform.Find(LeftGameObjectName);
                Transform rightGameObject = gUiWindowPanel.transform.Find(RightGameObjectName);
                Transform middleGameObject = gUiWindowPanel.transform.Find(MiddleGameObjectName);

                // Update the screen
                string leftText = "Unset 1";
                string middleText = "Unset 2";
                string rightText = "Unset 3";

                // TEST DATA FOR NOW
                int index = 0;
                foreach (var debuff in debuffList)
                {
                    if (index == 0)
                    {
                        leftText = debuff.debuffName;
                    }
                    else if (index == 1)
                    {
                        middleText = debuff.debuffName;
                    }
                    else if (index == 2)
                    {
                        rightText = debuff.debuffName;
                    }
                    index++;
                }
leftGameObject.GetComponent<TextMeshProUGUI>().text = leftText;
                rightGameObject.GetComponent<TextMeshProUGUI>().text = rightText;
                middleGameObject.GetComponent<TextMeshProUGUI>().text = middleText;
            }
        }
        
        // Called by the closing of the offensive target window
        public void HideDebuffPanel()
        {
            gUiWindowPanel.Show();
        }

        // Called by the /debuff command and on offensive target select
        public void ShowDebuffPanel()
        {
            gUiWindowPanel.Show();
        }
    }
}