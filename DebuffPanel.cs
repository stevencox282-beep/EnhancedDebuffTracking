using Il2Cpp;
using Il2CppTMPro;
using UnityEngine;
using UnityEngine.Events;

namespace EnhancedDebuffTracking
{
    // Box on the screen with 3 labels inside it
    public class DebuffPanel : MonoBehaviour
    {
        // Generic names to be used by any object, helps with calls to Find()
        private static string nameOne = "EDT_NameOne_EDT";
        private static string nameTwo = "EDT_NameTwo_EDT";
        private static string nameThree = "EDT_NameThree_EDT";
        private static string nameFour = "EDT_NameFour_EDT";
        private static string nameFive = "EDT_NameFive_EDT";
        private static string nameSix = "EDT_NameSix_EDT";
        private static string nameSeven = "EDT_NameSeven_EDT";
        private static string nameEight = "EDT_NameEight_EDT";
        private static string nameNine = "EDT_NameNine_EDT";
        private static string nameTen = "EDT_NameTen_EDT";

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

        // Builder function to create a default TextMesh
        private TextMeshProUGUI BuildTextMeshProUGUI(GameObject gameObject)
        {
            // Add and configure the TextMeshPros for rendering the time data
            TextMeshProUGUI textMesh = gameObject.AddComponent<TextMeshProUGUI>();
            textMesh.alignment = TextAlignmentOptions.Left;
            textMesh.fontSize = Globals.FontSize;
            textMesh.color = Color.yellow;
            textMesh.text = "";

            return textMesh;
        }

        // Setup the text meshs inside the panel that will display the data we want
        private void BuildTextMeshs()
        {
            // Create new GameObjects for the debuff display
            GameObject gameObjectOne = new GameObject(nameOne);
            GameObject gameObjectTwo = new GameObject(nameTwo);
            GameObject gameObjectThree = new GameObject(nameThree);
            GameObject gameObjectFour = new GameObject(nameFour);
            GameObject gameObjectFive = new GameObject(nameFive);
            GameObject gameObjectSix = new GameObject(nameSix);
            GameObject gameObjectSeven = new GameObject(nameSeven);
            GameObject gameObjectEight = new GameObject(nameEight);
            GameObject gameObjectNine = new GameObject(nameNine);
            GameObject gameObjectTen = new GameObject(nameTen);

            // Set its parent to the new window panel (which is parented to Mid)
            gameObjectOne.transform.SetParent(gUiWindowPanel.transform, false);
            gameObjectTwo.transform.SetParent(gUiWindowPanel.transform, false);
            gameObjectThree.transform.SetParent(gUiWindowPanel.transform, false);
            gameObjectFour.transform.SetParent(gUiWindowPanel.transform, false);
            gameObjectFive.transform.SetParent(gUiWindowPanel.transform, false);
            gameObjectSix.transform.SetParent(gUiWindowPanel.transform, false);
            gameObjectSeven.transform.SetParent(gUiWindowPanel.transform, false);
            gameObjectEight.transform.SetParent(gUiWindowPanel.transform, false);
            gameObjectNine.transform.SetParent(gUiWindowPanel.transform, false);
            gameObjectTen.transform.SetParent(gUiWindowPanel.transform, false);

            TextMeshProUGUI textMeshProOne = BuildTextMeshProUGUI(gameObjectOne);
            TextMeshProUGUI textMeshProTwo = BuildTextMeshProUGUI(gameObjectTwo);
            TextMeshProUGUI textMeshProThree = BuildTextMeshProUGUI(gameObjectThree);
            TextMeshProUGUI textMeshProFour = BuildTextMeshProUGUI(gameObjectFour);
            TextMeshProUGUI textMeshProFive = BuildTextMeshProUGUI(gameObjectFive);
            TextMeshProUGUI textMeshProSix = BuildTextMeshProUGUI(gameObjectSix);
            TextMeshProUGUI textMeshProSeven = BuildTextMeshProUGUI(gameObjectSeven);
            TextMeshProUGUI textMeshProEight = BuildTextMeshProUGUI(gameObjectEight);
            TextMeshProUGUI textMeshProNine = BuildTextMeshProUGUI(gameObjectNine);
            TextMeshProUGUI textMeshProTen = BuildTextMeshProUGUI(gameObjectTen);

            // Set up the RectTransform to position the texts correctly
            var textRectTransformOne = textMeshProOne.rectTransform;
            textRectTransformOne.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            textRectTransformOne.anchorMin = new Vector2(0.05f, 0.9f); // Just off Bottom Left
            textRectTransformOne.anchorMax = new Vector2(0.05f, 0.9f); // Just off Bottom Left 
            textRectTransformOne.anchoredPosition = new Vector2(0f, 0f);
            textRectTransformOne.pivot = new Vector2(0f, 0f);

            var textRectTransformTwo = textMeshProTwo.rectTransform;
            textRectTransformTwo.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            textRectTransformTwo.anchorMin = new Vector2(0.05f, 0.8f); // Bottom Middle 
            textRectTransformTwo.anchorMax = new Vector2(0.05f, 0.8f); // Bottom Middle 
            textRectTransformTwo.anchoredPosition = new Vector2(0, 0);
            textRectTransformTwo.pivot = new Vector2(0f, 0f);

            var textRectTransformThree = textMeshProThree.rectTransform;
            textRectTransformThree.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            textRectTransformThree.anchorMin = new Vector2(0.05f, 0.7f); // Bottom Right 
            textRectTransformThree.anchorMax = new Vector2(0.05f, 0.7f); // Bottom Right
            textRectTransformThree.anchoredPosition = new Vector2(0, 0);
            textRectTransformThree.pivot = new Vector2(0f, 0f);

            var textRectTransformFour = textMeshProFour.rectTransform;
            textRectTransformFour.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            textRectTransformFour.anchorMin = new Vector2(0.05f, 0.6f); // Bottom Right 
            textRectTransformFour.anchorMax = new Vector2(0.05f, 0.6f); // Bottom Right
            textRectTransformFour.anchoredPosition = new Vector2(0, 0);
            textRectTransformFour.pivot = new Vector2(0f, 0f);

            var textRectTransformFive = textMeshProFive.rectTransform;
            textRectTransformFive.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            textRectTransformFive.anchorMin = new Vector2(0.05f, 0.5f); // Bottom Right 
            textRectTransformFive.anchorMax = new Vector2(0.05f, 0.5f); // Bottom Right
            textRectTransformFive.anchoredPosition = new Vector2(0, 0);
            textRectTransformFive.pivot = new Vector2(0f, 0f);

            var textRectTransformSix = textMeshProSix.rectTransform;
            textRectTransformSix.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            textRectTransformSix.anchorMin = new Vector2(0.05f, 0.4f); // Bottom Right 
            textRectTransformSix.anchorMax = new Vector2(0.05f, 0.4f); // Bottom Right
            textRectTransformSix.anchoredPosition = new Vector2(0, 0);
            textRectTransformSix.pivot = new Vector2(0f, 0f);

            var textRectTransformSeven = textMeshProSeven.rectTransform;
            textRectTransformSeven.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            textRectTransformSeven.anchorMin = new Vector2(0.05f, 0.3f); // Bottom Right 
            textRectTransformSeven.anchorMax = new Vector2(0.05f, 0.3f); // Bottom Right
            textRectTransformSeven.anchoredPosition = new Vector2(0, 0);
            textRectTransformSeven.pivot = new Vector2(0f, 0f);

            var textRectTransformEight = textMeshProEight.rectTransform;
            textRectTransformEight.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            textRectTransformEight.anchorMin = new Vector2(0.05f, 0.2f); // Bottom Right 
            textRectTransformEight.anchorMax = new Vector2(0.05f, 0.2f); // Bottom Right
            textRectTransformEight.anchoredPosition = new Vector2(0, 0);
            textRectTransformEight.pivot = new Vector2(0f, 0f);

            var textRectTransformNine = textMeshProNine.rectTransform;
            textRectTransformNine.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            textRectTransformNine.anchorMin = new Vector2(0.05f, 0.1f); // Bottom Right 
            textRectTransformNine.anchorMax = new Vector2(0.05f, 0.1f); // Bottom Right
            textRectTransformNine.anchoredPosition = new Vector2(0, 0);
            textRectTransformNine.pivot = new Vector2(0f, 0f);

            var textRectTransformTen = textMeshProTen.rectTransform;
            textRectTransformTen.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            textRectTransformTen.anchorMin = new Vector2(0.05f, 0f); // Bottom Right 
            textRectTransformTen.anchorMax = new Vector2(0.05f, 0f); // Bottom Right
            textRectTransformTen.anchoredPosition = new Vector2(0, 0);
            textRectTransformTen.pivot = new Vector2(0f, 0f);
        }


        //Update the text displayed in the Debuff Box
        public void ResetDebuffPanel()
        {
            // Try and stop unwanted access to the panel to prevent exceptions
            if (gUiWindowPanel.isActiveAndEnabled && gUiWindowPanel.IsVisible)
            {
                // Create list that holds all the Tranforms for every TextMesh
                List<Transform> transformList = new List<Transform>() {
                    {gUiWindowPanel.transform.Find(nameOne)},   {gUiWindowPanel.transform.Find(nameTwo)},
                    {gUiWindowPanel.transform.Find(nameThree)}, {gUiWindowPanel.transform.Find(nameFour)},
                    {gUiWindowPanel.transform.Find(nameFive)},  {gUiWindowPanel.transform.Find(nameSix)},
                    {gUiWindowPanel.transform.Find(nameSeven)}, {gUiWindowPanel.transform.Find(nameEight)},
                    {gUiWindowPanel.transform.Find(nameNine)},  {gUiWindowPanel.transform.Find(nameTen)}
                };

                // List the string values for all MaxDisplayableDebuffs debuffs
                List<string> displayedTextList = new List<string>()
                {
                    { "Unset 1" }, { "Unset 2" }, { "Unset 3" }, { "Unset 4" }, { "Unset 5" },
                    { "Unset 6" }, { "Unset 7" }, { "Unset 8" }, { "Unset 9" }, { "Unset 10" },
                };

                // Parse the list of all debuffs on the current target and display the first MaxDisplayableDebuffs
                for (int i = 0; (i < displayedTextList.Count && i < Globals.MaxDisplayableDebuffs); i++)
                {
                    // reset to an clean list
                    transformList[i].GetComponent<TextMeshProUGUI>().text = displayedTextList[i];
                }
            }
        }

        //Update the text displayed in the Debuff Box
        public void UpdateDebuffPanel(List<DebuffData> debuffList)
        {
            // Try and stop unwanted access to the panel to prevent exceptions
            if (gUiWindowPanel.isActiveAndEnabled && gUiWindowPanel.IsVisible)
            {
                // Create list that holds all the Tranforms for every TextMesh
                List<Transform> transformList = new List<Transform>() {
                    {gUiWindowPanel.transform.Find(nameOne)},   {gUiWindowPanel.transform.Find(nameTwo)},
                    {gUiWindowPanel.transform.Find(nameThree)}, {gUiWindowPanel.transform.Find(nameFour)},
                    {gUiWindowPanel.transform.Find(nameFive)},  {gUiWindowPanel.transform.Find(nameSix)},
                    {gUiWindowPanel.transform.Find(nameSeven)}, {gUiWindowPanel.transform.Find(nameEight)},
                    {gUiWindowPanel.transform.Find(nameNine)},  {gUiWindowPanel.transform.Find(nameTen)}
                };

                // List the string values for all MaxDisplayableDebuffs debuffs
                List<string> displayedTextList = new List<string>()
                {
                    { "Unset 1" }, { "Unset 2" }, { "Unset 3" }, { "Unset 4" }, { "Unset 5" },
                    { "Unset 6" }, { "Unset 7" }, { "Unset 8" }, { "Unset 9" }, { "Unset 10" },
                };

                // Parse the list of all debuffs on the current target and display the first MaxDisplayableDebuffs
                for (int i = 0; (i < debuffList.Count && i < Globals.MaxDisplayableDebuffs) ; i++)
                {
                    // use the default list, this means any buffs that just expired are tidied up and removed from displayUpdate the reset list, as this is necessary 
                    displayedTextList[i] = debuffList[i].debuffName + " " + debuffList[i].debuffDurationRemaining + "s"; ;
                    // Update the displayed string "DebuffName 22s"
                    transformList[i].GetComponent<TextMeshProUGUI>().text = displayedTextList[i];
                }
            }
        }

        // Called by the closing of the offensive target window
        public void HideDebuffPanel()
        {
            gUiWindowPanel.Hide();
        }

        // Called by the /debuff command and on offensive target select
        public void ShowDebuffPanel()
        {
            gUiWindowPanel.Show();
        }
    }
}