using Il2Cpp;
using Il2CppTMPro;
using MelonLoader;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

        // Names for images
        private static string imageNameOne = "EDT_ImageNameOne_EDT";
        private static string imageNameTwo = "EDT_ImageNameTwo_EDT";
        private static string imageNameThree = "EDT_ImageNameThree_EDT";
        private static string imageNameFour = "EDT_ImageNameFour_EDT";
        private static string imageNameFive = "EDT_ImageNameFive_EDT";
        private static string imageNameSix = "EDT_ImageNameSix_EDT";
        private static string imageNameSeven = "EDT_ImageNameSeven_EDT";
        private static string imageNameEight = "EDT_ImageNameEight_EDT";
        private static string imageNameNine = "EDT_ImageNameNine_EDT";
        private static string imageNameTen = "EDT_ImageNameTen_EDT";

        // List the string values for all MaxDisplayableDebuffs debuffs
        private static List<string> cleanTextList = new List<string>()
        {
            { "" }, { "" }, { "" }, { "" }, { "" },
            { "" }, { "" }, { "" }, { "" }, { "" },
        };

        private static List<Color> barColours = new List<Color>()
        {
            { Color.blue}, { Color.green}, { Color.purple }, { Color.brown }, { Color.red },
            { Color.blue}, { Color.green}, { Color.purple }, { Color.brown }, { Color.red },
        };

        // Setup lists to aid in the accessing of transform data later on
        List<Transform> textMeshObjects = new List<Transform>();
        List<Transform> imageObjects = new List<Transform>();
        

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

            // Add in the uimages that will be the progress bars
            BuildImages();

            // Add in Text Meshs that display the data
            BuildTextMeshs();

            // Ensure the panel is not displayed immediatly, this will trigger on target selection
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
        private TextMeshProUGUI BuildTextMeshComponent(GameObject gameObject)
        {
            // Add and configure the TextMeshPros for rendering the time data
            TextMeshProUGUI textMesh = gameObject.AddComponent<TextMeshProUGUI>();
            textMesh.alignment = TextAlignmentOptions.Left;
            textMesh.fontSize = Globals.FontSize;
            textMesh.color = Color.white;
            textMesh.text = "";

            return textMesh;
        }


        private void BuildTextMesh(string name, float panelHeightOffset)
        {
            GameObject gameObject = new GameObject(name);
            // Set its parent to the new window panel (which is parented to Mid)
            gameObject.transform.SetParent(gUiWindowPanel.transform, false);

            TextMeshProUGUI textMesh = BuildTextMeshComponent(gameObject);
            var rectTransformOne = textMesh.rectTransform;
            rectTransformOne.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformOne.anchorMin = new Vector2(Globals.LeftMargin, panelHeightOffset);
            rectTransformOne.anchorMax = new Vector2(Globals.LeftMargin, panelHeightOffset);
            rectTransformOne.anchoredPosition = new Vector2(0f, 0f);
            rectTransformOne.pivot = new Vector2(0f, 0f);
        }

        private void BuildImage(string name, float panelHeightOffset)
        {
            GameObject gameObject = new GameObject(name);
            // Set its parent to the new window panel (which is parented to Mid)
            gameObject.transform.SetParent(gUiWindowPanel.transform, false);
            Image image = BuildImageComponent(gameObject);

            var transform = image.rectTransform;
            transform.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            transform.anchorMin = new Vector2(Globals.LeftMargin, panelHeightOffset);
            transform.anchorMax = new Vector2(Globals.LeftMargin, panelHeightOffset);
            transform.anchoredPosition = new Vector2(0f, 0f);
            transform.pivot = new Vector2(0f, 0f);
        }

        // Builder function to create a default image
        private UnityEngine.UI.Image BuildImageComponent(GameObject gameObject)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.pink);
            tex.Apply();
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));

            gameObject.layer = Layers.UI;
            var image = gameObject.AddComponent<Image>();
            image.sprite = sprite;
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Horizontal;
            image.color = Color.pink;
            image.fillAmount = 0.5f; // 1.0f is full 0.0f is empty
            return image;
        }

        private void BuildImages() 
        {
            BuildImage(imageNameOne,   Globals.HeightOneOffset);
            BuildImage(imageNameTwo,   Globals.HeightTwoOffset);
            BuildImage(imageNameThree, Globals.HeightThreeOffset);
            BuildImage(imageNameFour,  Globals.HeightFourOffset);
            BuildImage(imageNameFive,  Globals.HeightFiveOffset);
            BuildImage(imageNameSix,   Globals.HeightSixOffset);
            BuildImage(imageNameSeven, Globals.HeightSevenOffset);
            BuildImage(imageNameEight, Globals.HeightEightOffset);
            BuildImage(imageNameNine,  Globals.HeightNineOffset);
            BuildImage(imageNameTen,   Globals.HeightTenOffset);

            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameOne));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwo));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameThree));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameFour));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameFive));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameSix));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameSeven));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameEight));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameNine));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTen));
            
        }

        // Setup the text meshs inside the panel that will display the data we want
        private void BuildTextMeshs()
        {
            BuildTextMesh(nameOne,   Globals.HeightOneOffset);
            BuildTextMesh(nameTwo,   Globals.HeightTwoOffset);
            BuildTextMesh(nameThree, Globals.HeightThreeOffset);
            BuildTextMesh(nameFour,  Globals.HeightFourOffset);
            BuildTextMesh(nameFive,  Globals.HeightFiveOffset);
            BuildTextMesh(nameSix,   Globals.HeightSixOffset);
            BuildTextMesh(nameSeven, Globals.HeightSevenOffset);
            BuildTextMesh(nameEight, Globals.HeightEightOffset);
            BuildTextMesh(nameNine,  Globals.HeightNineOffset);
            BuildTextMesh(nameTen,   Globals.HeightTenOffset);

            // Create list that holds all the Tranforms for every TextMesh
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameOne));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameTwo));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameThree));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameFour));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameFive));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameSix));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameSeven));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameEight));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameNine));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameTen));
        }


        //Update the text displayed in the Debuff Box
        public void ResetDebuffPanel()
        {
            // Try and stop unwanted access to the panel to prevent exceptions
            if (gUiWindowPanel.isActiveAndEnabled && gUiWindowPanel.IsVisible)
            {
                // Parse the list of all debuffs on the current target and display the first MaxDisplayableDebuffs
                for (int i = 0; (i < cleanTextList.Count && i < Globals.MaxDisplayableDebuffs); i++)
                {
                    // reset to an clean list
                    textMeshObjects[i].GetComponent<TextMeshProUGUI>().text = cleanTextList[i];
                    // Now update the progress bar colour and time
                    UnityEngine.UI.Image image = imageObjects[i].transform.GetComponent<UnityEngine.UI.Image>();
                    // Set colour to black on reset
                    image.color = Color.black;
                    image.fillAmount = 0.5f;
                }
            }
        }

        //Update the text displayed in the Debuff Box
        public void UpdateDebuffPanel(List<DebuffData> debuffList)
        {
            // Try and stop unwanted access to the panel to prevent exceptions
            if (gUiWindowPanel.isActiveAndEnabled && gUiWindowPanel.IsVisible)
            {
                // Parse the list of all debuffs on the current target and display the first MaxDisplayableDebuffs
                for (int i = 0; (i < debuffList.Count && i < Globals.MaxDisplayableDebuffs) ; i++)
                {
                    DebuffData debuff = debuffList[i];

                    // Update the displayed string "DebuffName 22s", leave the leading space in
                    textMeshObjects[i].GetComponent<TextMeshProUGUI>().text = $" {debuff.debuffName}, {debuff.debuffDurationRemaining}s, {debuff.numStacks}/{debuff.maxStacks} Stacks ({debuff.casterName})"; ;

                    // Now update the progress bar colour and time
                    Image image = imageObjects[i].transform.GetComponent<UnityEngine.UI.Image>();

                    // Set colour based on the debuff type
                    image.color = barColours[i];
                    // We store a number between 0.0f and 1.0f
                    // ((1 / debuffDuration) * debuffDurationRemaining)
                    // (( 1 / 150) * 100) 
                    image.fillAmount = ((1 / debuff.debuffDuration) * debuff.debuffDurationRemaining);
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