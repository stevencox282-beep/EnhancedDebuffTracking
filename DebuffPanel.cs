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
            { "Unset 1" }, { "Unset 2" }, { "Unset 3" }, { "Unset 4" }, { "Unset 5" },
            { "Unset 6" }, { "Unset 7" }, { "Unset 8" }, { "Unset 9" }, { "Unset 10" },
        };

        private static List<Color> barColours = new List<Color>()
        {
            { Color.blue}, { Color.green}, { Color.purple }, { Color.white }, { Color.red },
            { Color.blue}, { Color.green}, { Color.purple }, { Color.white }, { Color.red },
        };

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
        private TextMeshProUGUI BuildTextMesh(GameObject gameObject)
        {
            // Add and configure the TextMeshPros for rendering the time data
            TextMeshProUGUI textMesh = gameObject.AddComponent<TextMeshProUGUI>();
            textMesh.alignment = TextAlignmentOptions.Left;
            textMesh.fontSize = Globals.FontSize;
            textMesh.color = Color.white;
            textMesh.text = "";

            return textMesh;
        }

        // Builder function to create a default image
        private UnityEngine.UI.Image BuildImage(GameObject gameObject)
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
            // Create new GameObjects for the debuff display
            GameObject gameObjectOne = new GameObject(imageNameOne);
            GameObject gameObjectTwo = new GameObject(imageNameTwo);
            GameObject gameObjectThree = new GameObject(imageNameThree);
            GameObject gameObjectFour = new GameObject(imageNameFour);
            GameObject gameObjectFive = new GameObject(imageNameFive);
            GameObject gameObjectSix = new GameObject(imageNameSix);
            GameObject gameObjectSeven = new GameObject(imageNameSeven);
            GameObject gameObjectEight = new GameObject(imageNameEight);
            GameObject gameObjectNine = new GameObject(imageNameNine);
            GameObject gameObjectTen = new GameObject(imageNameTen);

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

            // Build all the required TextMeshs
            UnityEngine.UI.Image imageOne = BuildImage(gameObjectOne);
            UnityEngine.UI.Image imageTwo = BuildImage(gameObjectTwo);
            UnityEngine.UI.Image imageThree = BuildImage(gameObjectThree);
            UnityEngine.UI.Image imageFour = BuildImage(gameObjectFour);
            UnityEngine.UI.Image imageFive = BuildImage(gameObjectFive);
            UnityEngine.UI.Image imageSix = BuildImage(gameObjectSix);
            UnityEngine.UI.Image imageSeven = BuildImage(gameObjectSeven);
            UnityEngine.UI.Image imageEight = BuildImage(gameObjectEight);
            UnityEngine.UI.Image imageNine = BuildImage(gameObjectNine);
            UnityEngine.UI.Image imageTen = BuildImage(gameObjectTen);

            var rectTransformOne = imageOne.rectTransform;
            rectTransformOne.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformOne.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightOne);
            rectTransformOne.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightOne);
            rectTransformOne.anchoredPosition = new Vector2(0f, 0f);
            rectTransformOne.pivot = new Vector2(0f, 0f);
            
            var rectTransformTwo = imageTwo.rectTransform;
            rectTransformTwo.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformTwo.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightTwo);
            rectTransformTwo.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightTwo);
            rectTransformTwo.anchoredPosition = new Vector2(0, 0);
            rectTransformTwo.pivot = new Vector2(0f, 0f);

            var rectTransformThree = imageThree.rectTransform;
            rectTransformThree.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformThree.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightThree);
            rectTransformThree.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightThree);
            rectTransformThree.anchoredPosition = new Vector2(0, 0);
            rectTransformThree.pivot = new Vector2(0f, 0f);

            var rectTransformFour = imageFour.rectTransform;
            rectTransformFour.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformFour.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightFour);
            rectTransformFour.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightFour);
            rectTransformFour.anchoredPosition = new Vector2(0, 0);
            rectTransformFour.pivot = new Vector2(0f, 0f);

            var rectTransformFive = imageFive.rectTransform;
            rectTransformFive.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformFive.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightFive);
            rectTransformFive.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightFive);
            rectTransformFive.anchoredPosition = new Vector2(0, 0);
            rectTransformFive.pivot = new Vector2(0f, 0f);

            var rectTransformSix = imageSix.rectTransform;
            rectTransformSix.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformSix.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightSix);
            rectTransformSix.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightSix);
            rectTransformSix.anchoredPosition = new Vector2(0, 0);
            rectTransformSix.pivot = new Vector2(0f, 0f);

            var rectTransformSeven = imageSeven.rectTransform;
            rectTransformSeven.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformSeven.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightSeven);
            rectTransformSeven.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightSeven);
            rectTransformSeven.anchoredPosition = new Vector2(0, 0);
            rectTransformSeven.pivot = new Vector2(0f, 0f);

            var rectTransformEight = imageEight.rectTransform;
            rectTransformEight.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformEight.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightEight);
            rectTransformEight.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightEight);
            rectTransformEight.anchoredPosition = new Vector2(0, 0);
            rectTransformEight.pivot = new Vector2(0f, 0f);

            var rectTransformNine = imageNine.rectTransform;
            rectTransformNine.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformNine.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightNine);
            rectTransformNine.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightNine);
            rectTransformNine.anchoredPosition = new Vector2(0, 0);
            rectTransformNine.pivot = new Vector2(0f, 0f);

            var rectTransformTen = imageTen.rectTransform;
            rectTransformTen.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformTen.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightTen);
            rectTransformTen.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightTen);
            rectTransformTen.anchoredPosition = new Vector2(0, 0);
            rectTransformTen.pivot = new Vector2(0f, 0f);
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

            // Build all the required TextMeshs
            TextMeshProUGUI textMeshOne = BuildTextMesh(gameObjectOne);
            TextMeshProUGUI textMeshTwo = BuildTextMesh(gameObjectTwo);
            TextMeshProUGUI textMeshThree = BuildTextMesh(gameObjectThree);
            TextMeshProUGUI textMeshFour = BuildTextMesh(gameObjectFour);
            TextMeshProUGUI textMeshFive = BuildTextMesh(gameObjectFive);
            TextMeshProUGUI textMeshSix = BuildTextMesh(gameObjectSix);
            TextMeshProUGUI textMeshSeven = BuildTextMesh(gameObjectSeven);
            TextMeshProUGUI textMeshEight = BuildTextMesh(gameObjectEight);
            TextMeshProUGUI textMeshNine = BuildTextMesh(gameObjectNine);
            TextMeshProUGUI textMeshTen = BuildTextMesh(gameObjectTen);

            // Set up the RectTransform to position the texts correctly
            var rectTransformOne = textMeshOne.rectTransform;
            rectTransformOne.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformOne.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightOne); 
            rectTransformOne.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightOne); 
            rectTransformOne.anchoredPosition = new Vector2(0f, 0f);
            rectTransformOne.pivot = new Vector2(0f, 0f);

            var rectTransformTwo = textMeshTwo.rectTransform;
            rectTransformTwo.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformTwo.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightTwo);
            rectTransformTwo.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightTwo);
            rectTransformTwo.anchoredPosition = new Vector2(0, 0);
            rectTransformTwo.pivot = new Vector2(0f, 0f);

            var rectTransformThree = textMeshThree.rectTransform;
            rectTransformThree.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformThree.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightThree);
            rectTransformThree.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightThree);
            rectTransformThree.anchoredPosition = new Vector2(0, 0);
            rectTransformThree.pivot = new Vector2(0f, 0f);

            var rectTransformFour = textMeshFour.rectTransform;
            rectTransformFour.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformFour.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightFour);
            rectTransformFour.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightFour);
            rectTransformFour.anchoredPosition = new Vector2(0, 0);
            rectTransformFour.pivot = new Vector2(0f, 0f);

            var rectTransformFive = textMeshFive.rectTransform;
            rectTransformFive.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformFive.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightFive);
            rectTransformFive.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightFive);
            rectTransformFive.anchoredPosition = new Vector2(0, 0);
            rectTransformFive.pivot = new Vector2(0f, 0f);

            var rectTransformSix = textMeshSix.rectTransform;
            rectTransformSix.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformSix.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightSix);
            rectTransformSix.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightSix);
            rectTransformSix.anchoredPosition = new Vector2(0, 0);
            rectTransformSix.pivot = new Vector2(0f, 0f);

            var rectTransformSeven = textMeshSeven.rectTransform;
            rectTransformSeven.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformSeven.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightSeven);
            rectTransformSeven.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightSeven);
            rectTransformSeven.anchoredPosition = new Vector2(0, 0);
            rectTransformSeven.pivot = new Vector2(0f, 0f);

            var rectTransformEight = textMeshEight.rectTransform;
            rectTransformEight.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformEight.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightEight);
            rectTransformEight.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightEight);
            rectTransformEight.anchoredPosition = new Vector2(0, 0);
            rectTransformEight.pivot = new Vector2(0f, 0f);

            var rectTransformNine = textMeshNine.rectTransform;
            rectTransformNine.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformNine.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightNine);
            rectTransformNine.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightNine);
            rectTransformNine.anchoredPosition = new Vector2(0, 0);
            rectTransformNine.pivot = new Vector2(0f, 0f);

            var rectTransformTen = textMeshTen.rectTransform;
            rectTransformTen.sizeDelta = new Vector2(Globals.MeshWidth, Globals.MeshHeight);
            rectTransformTen.anchorMin = new Vector2(Globals.LeftMargin, Globals.HeightTen);
            rectTransformTen.anchorMax = new Vector2(Globals.LeftMargin, Globals.HeightTen);
            rectTransformTen.anchoredPosition = new Vector2(0, 0);
            rectTransformTen.pivot = new Vector2(0f, 0f);
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

                // List of all the images (progress bars)
                List<Transform> imageObjects = new List<Transform>() {
                    {gUiWindowPanel.transform.Find(imageNameOne)},   {gUiWindowPanel.transform.Find(imageNameTwo)},
                    {gUiWindowPanel.transform.Find(imageNameThree)}, {gUiWindowPanel.transform.Find(imageNameFour)},
                    {gUiWindowPanel.transform.Find(imageNameFive)},  {gUiWindowPanel.transform.Find(imageNameSix)},
                    {gUiWindowPanel.transform.Find(imageNameSeven)}, {gUiWindowPanel.transform.Find(imageNameEight)},
                    {gUiWindowPanel.transform.Find(imageNameNine)},  {gUiWindowPanel.transform.Find(imageNameTen)}
                };

                // Parse the list of all debuffs on the current target and display the first MaxDisplayableDebuffs
                for (int i = 0; (i < cleanTextList.Count && i < Globals.MaxDisplayableDebuffs); i++)
                {
                    // reset to an clean list
                    transformList[i].GetComponent<TextMeshProUGUI>().text = cleanTextList[i];
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
                // Create list that holds all the Tranforms for every TextMesh
                List<Transform> transformList = new List<Transform>() {
                    {gUiWindowPanel.transform.Find(nameOne)},   {gUiWindowPanel.transform.Find(nameTwo)},
                    {gUiWindowPanel.transform.Find(nameThree)}, {gUiWindowPanel.transform.Find(nameFour)},
                    {gUiWindowPanel.transform.Find(nameFive)},  {gUiWindowPanel.transform.Find(nameSix)},
                    {gUiWindowPanel.transform.Find(nameSeven)}, {gUiWindowPanel.transform.Find(nameEight)},
                    {gUiWindowPanel.transform.Find(nameNine)},  {gUiWindowPanel.transform.Find(nameTen)}
                };

                // List of all the images (progress bars)
                List<Transform> imageObjects = new List<Transform>() {
                    {gUiWindowPanel.transform.Find(imageNameOne)},   {gUiWindowPanel.transform.Find(imageNameTwo)},
                    {gUiWindowPanel.transform.Find(imageNameThree)}, {gUiWindowPanel.transform.Find(imageNameFour)},
                    {gUiWindowPanel.transform.Find(imageNameFive)},  {gUiWindowPanel.transform.Find(imageNameSix)},
                    {gUiWindowPanel.transform.Find(imageNameSeven)}, {gUiWindowPanel.transform.Find(imageNameEight)},
                    {gUiWindowPanel.transform.Find(imageNameNine)},  {gUiWindowPanel.transform.Find(imageNameTen)}
                };

                // Parse the list of all debuffs on the current target and display the first MaxDisplayableDebuffs
                for (int i = 0; (i < debuffList.Count && i < Globals.MaxDisplayableDebuffs) ; i++)
                {
                    DebuffData debuff = debuffList[i];

                    // Update the displayed string "DebuffName 22s"
                    transformList[i].GetComponent<TextMeshProUGUI>().text = $"{debuff.debuffName}, {debuff.debuffDurationRemaining}s, {debuff.numStacks}/{debuff.maxStacks} Stacks ({debuff.casterName})"; ;

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