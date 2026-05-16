using Il2Cpp;
using Il2CppServiceStack;
using Il2CppSystem.Security.Cryptography;
using Il2CppTMPro;
using MelonLoader;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace EnhancedDebuffTracking
{
    // Box on the screen with 3 labels inside it
    public class DebuffPanel : MonoBehaviour
    {
        // textmesh to display the targetName
        private static string targetName = "EDT_TargetName_EDT";

        // Names to be used for textmesh game objects, helps with calls to Find()
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
        private static string nameEleven = "EDT_NameEleven_EDT";
        private static string nameTwelve = "EDT_NameTwelve_EDT";
        private static string nameThirteen = "EDT_NameThirteen_EDT";
        private static string nameFourteen = "EDT_NameFourteen_EDT";
        private static string nameFithteen = "EDT_NameFithteen_EDT";
        private static string nameSixteen = "EDT_NameSixteen_EDT";
        private static string nameSeventeen = "EDT_NameSeventeen_EDT";          
        private static string nameEighteen = "EDT_NameEighteen_EDT";
        private static string nameNineteen = "EDT_NameNineteen_EDT";
        private static string nameTwenty = "EDT_NameTwenty_EDT";
        private static string nameTwentyOne = "EDT_NameTwentyOne_EDT";
        private static string nameTwentyTwo = "EDT_NameTwentyTwo_EDT";
        private static string nameTwentyThree = "EDT_NameTwentyThree_EDT";
        private static string nameTwentyFour = "EDT_NameTwentyFour_EDT";
        private static string nameTwentyFive = "EDT_NameTwentyFive_EDT";
        private static string nameTwentySix = "EDT_NameTwentySix_EDT";
        private static string nameTwentySeven = "EDT_NameTwentySeven_EDT";
        private static string nameTwentyEight = "EDT_NameTwentyEight_EDT";
        private static string nameTwentyNine = "EDT_NameTwentyNine_EDT";
        private static string nameThirty = "EDT_NameThirty_EDT";
        // Names to be used for time textmesh game objects, helps with calls to Find()
        private static string timeNameOne = "EDT_TimeNameOne_EDT";
        private static string timeNameTwo = "EDT_TimeNameTwo_EDT";
        private static string timeNameThree = "EDT_TimeNameThree_EDT";
        private static string timeNameFour = "EDT_TimeNameFour_EDT";
        private static string timeNameFive = "EDT_TimeNameFive_EDT";
        private static string timeNameSix = "EDT_TimeNameSix_EDT";
        private static string timeNameSeven = "EDT_TimeNameSeven_EDT";
        private static string timeNameEight = "EDT_TimeNameEight_EDT";
        private static string timeNameNine = "EDT_TimeNameNine_EDT";
        private static string timeNameTen = "EDT_TimeNameTen_EDT";
        private static string timeNameEleven = "EDT_TimeNameEleven_EDT";
        private static string timeNameTwelve = "EDT_TimeNameTwelve_EDT";
        private static string timeNameThirteen = "EDT_TimeNameThirteen_EDT";
        private static string timeNameFourteen = "EDT_TimeNameFourteen_EDT";
        private static string timeNameFithteen = "EDT_TimeNameFitheen_EDT";
        private static string timeNameSixteen = "EDT_TimeNameSixteen_EDT";
        private static string timeNameSeventeen = "EDT_TimeNameSeventeen_EDT";
        private static string timeNameEighteen = "EDT_TimeNameEighteen_EDT";
        private static string timeNameNineteen = "EDT_TimeNameNineteen_EDT";
        private static string timeNameTwenty = "EDT_TimeNameTwenty_EDT";
        private static string timeNameTwentyOne = "EDT_TimeNameTwentyOne_EDT";
        private static string timeNameTwentyTwo = "EDT_TimeNameTwentyTwo_EDT";
        private static string timeNameTwentyThree = "EDT_TimeNameTwentyThree_EDT";
        private static string timeNameTwentyFour = "EDT_TimeNameTwentyFour_EDT";
        private static string timeNameTwentyFive = "EDT_TimeNameTwentyFive_EDT";
        private static string timeNameTwentySix = "EDT_TimeNameTwentySix_EDT";
        private static string timeNameTwentySeven = "EDT_TimeNameTwentySeven_EDT";
        private static string timeNameTwentyEight = "EDT_TimeNameTwentyEight_EDT";
        private static string timeNameTwentyNine = "EDT_TimeNameTwentyNine_EDT";
        private static string timeNameThirty = "EDT_TimeNameThirty_EDT";

        // Names to be used for image game objects, helps with calls to Find()
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
        private static string imageNameEleven = "EDT_ImageNameEleven_EDT";
        private static string imageNameTwelve = "EDT_ImageNameTwelve_EDT";
        private static string imageNameThirteen = "EDT_ImageNameThirteen_EDT";
        private static string imageNameFourteen = "EDT_ImageNameFourteen_EDT";
        private static string imageNameFithteen = "EDT_ImageNameFithteen_EDT";
        private static string imageNameSixteen = "EDT_ImageNameSixteen_EDT";
        private static string imageNameSeventeen = "EDT_ImageNameSeventeen_EDT";
        private static string imageNameEighteen = "EDT_ImageNameEighteen_EDT";
        private static string imageNameNineteen = "EDT_ImageNameNineteen_EDT";
        private static string imageNameTwenty = "EDT_ImageNameTwenty_EDT";
        private static string imageNameTwentyOne = "EDT_ImageNameTwentyOne_EDT";
        private static string imageNameTwentyTwo = "EDT_ImageNameTwentyTwo_EDT";
        private static string imageNameTwentyThree = "EDT_ImageNameTwentyThree_EDT";
        private static string imageNameTwentyFour = "EDT_ImageNameTwentyFour_EDT";
        private static string imageNameTwentyFive = "EDT_ImageNameTwentyFive_EDT";
        private static string imageNameTwentySix = "EDT_ImageNameTwentySix_EDT";
        private static string imageNameTwentySeven = "EDT_ImageNameTwentySeven_EDT";
        private static string imageNameTwentyEight = "EDT_ImageNameTwentyEight_EDT";
        private static string imageNameTwentyNine = "EDT_ImageNameTwentyNine_EDT";
        private static string imageNameThirty = "EDT_ImageNameThirty_EDT";

        public static Scrollbar xScrollBar = new Scrollbar();
        public static Scrollbar yScrollBar = new Scrollbar();

        // Setup lists to aid in the accessing of transform data later on
        Transform targetNameTextMeshObject = new Transform();
        List<Transform> textMeshObjects = new List<Transform>();
        List<Transform> timeTextMeshObjects = new List<Transform>();
        List<Transform> imageObjects = new List<Transform>();

        // Holds the panel
        private static UIWindowPanel gUiWindowPanel  = null;

        private static Color getBarColours(string spellType)
        {
            Color returnColor = Color.black;
            // List the string values for all MaxDisplayableDebuffs debuffs
            switch (spellType)
            {
                case "Augmentation":
                    returnColor = Color.darkBlue;
                    break;
                case "Fortification":
                    returnColor = Color.darkGreen;
                    break;
                case "Manifestation":
                    returnColor = Color.purple;
                    break;
                case "Conjuration":
                    returnColor = Color.brown;
                    break;
                case "Evocation":
                    returnColor = Color.red;
                    break;
                case "Expulsion":
                    returnColor = Color.cadetBlue;
                    break;
                case "Restoration":
                    returnColor = Color.green;
                    break;
                case "Invocation":
                    returnColor = Color.indigo;
                    break;
                case "Illumination":
                    returnColor = Color.lavender;
                    break;
                case "Enervation":
                    returnColor = Color.limeGreen;
                    break;
                case "Corruption":
                    returnColor = Color.navyBlue;
                    break;
                case "TricksOfTheTrade":
                    returnColor = Color.oldLace;
                    break;
                case "Trapping":
                    returnColor = Color.azure;
                    break;
                case "Naturalism":
                    returnColor = Color.red;
                    break;
                case "FeignDeath":
                    returnColor = Color.orange;
                    break;
                case "Warfare":
                    returnColor = Color.olive;
                    break;
                case "None":
                    returnColor = Color.yellowGreen;
                    break;
                default:
                    returnColor = Color.black;
                    break;
            }

            return returnColor;
        }

        // Tidy up the alloated resources when we logout
        public void RemovePanel()
        {
            // On a /camp out and logging back in the static variables persist and are not garbage collected, explicitly clear them out, we will rebuild them on loading into a zone
            textMeshObjects.Clear();
            timeTextMeshObjects.Clear();
            imageObjects.Clear();

        }

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
            //RectMask2D     rectMask2D = gameObject.AddComponent<RectMask2D>(); // ADDING THIS IN CAUSES THE CLOSE BUTTON TO DISSAPEAR

            gUiWindowPanel = gameObject.AddComponent<UIWindowPanel>();

            // Block Raycasts to work around wonky click detection on the close button due other UI elements overlapping the close button image
            // I am not going to spend time making all my TextMesh's layout perfectly for this mod so block raycasts instead
            canvasGroup.blocksRaycasts = true;

            // Setup the Window Panel
            uiDraggable._windowPanel = gUiWindowPanel;
            gUiWindowPanel.CanvasGroup = canvasGroup;
            gUiWindowPanel._displayName = panelName;
            gUiWindowPanel.Resizable = true;

            // Setup the default position of the panel and its general parameters
            rectTransform.sizeDelta = panelSize;
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchoredPosition = new Vector2(-(panelSize.x / 2), panelSize.y / 2);
            rectTransform.localScale = new Vector3(1, 1, 1);
            
            // Setup the 2DRect Mask to prevent elements outide the pannel to not be rendered
//            RectTransform rectMask2DTransform = rectMask2D.rectTransform;
//            rectMask2DTransform.sizeDelta = new Vector2(5000,5000); // Allows clipping outside this range
//            rectMask2DTransform.pivot = new Vector2(0, 1);
//            rectMask2DTransform.anchoredPosition = new Vector2(-(panelSize.x / 2), panelSize.y / 2);


            string scrollRectGameObjectname = "IAMFISH";
            GameObject scrollRectGameObject = new GameObject(scrollRectGameObjectname);
            scrollRectGameObject.transform.SetParent(rectTransform);
            scrollRectGameObject.layer = Layers.UI;

            ScrollRect scrollRect = scrollRectGameObject.AddComponent<ScrollRect>();
            scrollRect.horizontalScrollbar = xScrollBar; 
            scrollRect.verticalScrollbar = yScrollBar;

            RectTransform scrollRectTransform = scrollRect.rectTransform; // Content must be placed here
            scrollRectTransform.sizeDelta = new Vector2(rectTransform.rect.height, rectTransform.rect.width);
            scrollRectTransform.pivot = new Vector2(0.065f, 1.0f);
            scrollRectTransform.anchoredPosition = new Vector2(1.0f, 0.0f);
            scrollRectTransform.anchorMax = new Vector2(1, 1); // Upper Right
            scrollRectTransform.anchorMin = new Vector2(0, 0); // Lower Left
            scrollRectTransform.localScale = rectTransform.localScale;

            if (scrollRect == null)
            {
                MelonLogger.Warning($"DisplayPanel() 7a");
            }
            // The content that can be scrolled. It should be a child of the GameObject with ScrollRect on it.
            scrollRect.content = scrollRectTransform;
            scrollRect.vertical = false;
            scrollRect.horizontal = false;
            scrollRect.elasticity = 0.0f;
            scrollRect.inertia = false;
            scrollRect.movementType = ScrollRect.MovementType.Clamped;
            scrollRect.viewport = rectTransform;


            // Adds the resize control to the Panel, hijacked from the chat box
            AddResizeControl(rectTransform, panelSize);

            // Add the MANDATORY elements to a panel, the compilor will not error if you don't do this but nothing will work
            BuildCloseButtonAndBackground(rectTransform, gameObject);

            // Add in the images that will be the progress bars
            BuildImages(scrollRectTransform);

            // Add in Text Meshs that display the data
            BuildTextMeshs(scrollRectTransform);

            // Ensure the panel is displayed immediatly
            gUiWindowPanel.Show();
        }

        private void AddResizeControl(RectTransform rectTransform, Vector2 panelSize)
        {
            // Get the resizable panel by borrowing configuration from the chat window
            var mainChatWindow = UIChatWindows.Instance.mainWindow;
            var mainChatRectHandle = mainChatWindow.GetComponentInChildren<UIResizeHandle>();

            UIResizeHandle resizeHandleCopy = Object.Instantiate(mainChatRectHandle, mainChatRectHandle.transform.position, mainChatRectHandle.transform.rotation, rectTransform);
            UIResizeHandle copyUIResizeHandle = resizeHandleCopy.GetComponent<UIResizeHandle>();
            copyUIResizeHandle.ContainerRect = rectTransform;

            // This is the resize icon at the bottom right
            var copyRect = resizeHandleCopy.GetComponent<RectTransform>();
            copyRect.pivot = new Vector2(1, 0);
            copyRect.sizeDelta = new Vector2(50, 50);
            copyRect.anchoredPosition = new Vector2(-5, 4);
         }

        // Constructs the close button and set the background
        private void BuildCloseButtonAndBackground(Transform parentPanel, GameObject gameObject)
        {
            // Source for copying button and backgrounds
            UITutorialPopup tutorialPopup = UIPanelRoots.Instance.Mid.transform.GetComponentInChildren<UITutorialPopup>();
            Transform tutorialButton = tutorialPopup.transform.GetChild(0);

            // Initialise the background for the new panel (MANDATORY)
            Image imageToCopy = tutorialPopup.GetComponent<Image>();
            var image = gameObject.AddComponent<Image>();
            image.type = Image.Type.Sliced;
            image.sprite = imageToCopy.sprite;

            // Initialise the close button of the panel (MANDATORY)
            var closeButton = GameObject.Instantiate(tutorialButton, tutorialButton.transform.position, tutorialButton.transform.rotation, gUiWindowPanel.transform);
            var closeButtonRect = closeButton.GetComponent<RectTransform>();
            closeButtonRect.sizeDelta = new Vector2(30, 30);
            closeButtonRect.anchoredPosition = new Vector2(-13.5f, -12); // Tiny size, top right corner, this ruins the box detection though
            closeButtonRect.pivot = new Vector2(0f, 0f);

            // Initialise on click behaviour of the close button
            var buttonComponent = closeButton.GetComponent<Button>();
            buttonComponent.onClick = new Button.ButtonClickedEvent();
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

        // Builder function to create a TextMesh component
        private TextMeshProUGUI BuildTextMeshComponent(GameObject gameObject)
        {
            // Add and configure the TextMeshPros for rendering the time data
            TextMeshProUGUI textMesh = gameObject.AddComponent<TextMeshProUGUI>();
            textMesh.alignment = TextAlignmentOptions.Left;
            textMesh.fontSize = Globals.FontSize;
            textMesh.color = Color.white;
            textMesh.text = "";
            textMesh.autoSizeTextContainer = false;
            textMesh.enableAutoSizing = false;
            
            return textMesh;
        }

        // Builder function to create an TextMesh
        private void BuildTextMesh(RectTransform scrollRectTransform, string name, int height, int width, float heightOffset, float widthOffset)
        {
            GameObject gameObject = new GameObject(name);
            // Set its parent to the new window panel (which is parented to Mid)
            gameObject.transform.SetParent(scrollRectTransform, false);
            ContentSizeFitter contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

            TextMeshProUGUI textMesh = BuildTextMeshComponent(gameObject);
            var rectTransformOne = textMesh.rectTransform;
            rectTransformOne.sizeDelta = new Vector2(width, height);
            rectTransformOne.anchorMin = new Vector2(widthOffset, heightOffset);
            rectTransformOne.anchorMax = new Vector2(widthOffset, heightOffset);
            rectTransformOne.anchoredPosition = new Vector2(0f, 0f);
            rectTransformOne.pivot = new Vector2(0f, 0f);
        }

        // Builder function to create an Image
        private void BuildImage(RectTransform rectTransform, string name, int height, int width, float heightOffset, float widthOffset)
        {
            GameObject gameObject = new GameObject(name);
            // Set its parent to the new window panel (which is parented to Mid)
            gameObject.transform.SetParent(rectTransform, false);
            ContentSizeFitter contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

            Image image = BuildImageComponent(gameObject);
            var transform = image.rectTransform;
            transform.sizeDelta = new Vector2(width, height);
            transform.anchorMin = new Vector2(widthOffset, heightOffset);
            transform.anchorMax = new Vector2(widthOffset, heightOffset);
            transform.anchoredPosition = new Vector2(0f, 0f);
            transform.pivot = new Vector2(0f, 0f);
        }

        // Builder function to create an image component
        private Image BuildImageComponent(GameObject gameObject)
        {
            // Make a solid colour sprite for use in the bar
            Texture2D tex = new Texture2D(1, 1);
            // NEVER set this to black, it stops the progress bar from displaying prperly for reasoons that are not obvious to me.
            tex.SetPixel(0, 0, Color.pink);
            tex.Apply();
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));

            gameObject.layer = Layers.UI;
            var image = gameObject.AddComponent<Image>();
            image.sprite = sprite;
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Horizontal;
            image.color = Color.black;
            image.fillAmount = 0.5f; // 1.0f is full 0.0f is empty
            return image;
        }

        // Builds all images (progress bars) to be display in the panel 
        private void BuildImages(RectTransform scrollRectTransform) 
        {
            // Make all the progress bars
            BuildImage(scrollRectTransform, imageNameOne,   Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightOneOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwo,   Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwoOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameThree, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThreeOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameFour,  Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFourOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameFive,  Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFiveOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameSix,   Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSixOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameSeven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSevenOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameEight, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightEightOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameNine,  Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightNineOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTen,   Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTenOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameEleven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightElevenOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwelve, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwelveOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameThirteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThirteenOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameFourteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFourteenOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameFithteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFithteenOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameSixteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSixteenOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameSeventeen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSeventeenOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameEighteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightEighteenOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameNineteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightNineteenOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwenty, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwentyOne, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyOneOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwentyTwo, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyTwoOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwentyThree, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyThreeOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwentyFour, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyFourOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwentyFive, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyFiveOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwentySix, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentySixOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwentySeven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentySevenOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwentyEight, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyEightOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameTwentyNine, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyNineOffset, Globals.RowLeftMargin);
            BuildImage(scrollRectTransform, imageNameThirty, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThirtyOffset, Globals.RowLeftMargin);

            // Save these for use later 
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameOne));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwo));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameThree));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameFour));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameFive));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameSix));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameSeven));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameEight));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameNine));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTen));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameEleven));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwelve));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameThirteen));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameFourteen));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameFithteen));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameSixteen));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameSeventeen));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameEighteen));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameNineteen));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwenty));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwentyOne));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwentyTwo));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwentyThree));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwentyFour));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwentyFive));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwentySix));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwentySeven));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwentyEight));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameTwentyNine));
            imageObjects.Add(scrollRectTransform.transform.Find(imageNameThirty));
        }

        // Builds all TextMeshes (debuff/time) to be display in the panel
        private void BuildTextMeshs(RectTransform scrollRectTransform)
        {
            // Text Mesh for Target Name
            BuildTextMesh(scrollRectTransform, targetName, Globals.NameMeshHeight, Globals.NameMeshWidth, 1f, 0.05f);

            // Make all the meshes that will sit on top of the bars
            BuildTextMesh(scrollRectTransform, nameOne, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightOneOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwo, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwoOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameThree, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThreeOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameFour, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFourOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameFive, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFiveOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameSix, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSixOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameSeven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSevenOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameEight, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightEightOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameNine, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightNineOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTenOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameEleven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightElevenOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwelve, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwelveOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameThirteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThirteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameFourteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFourteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameFithteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFithteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameSixteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSixteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameSeventeen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSeventeenOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameEighteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightEighteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameNineteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightNineteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwenty, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwentyOne, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyOneOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwentyTwo, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyTwoOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwentyThree, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyThreeOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwentyFour, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyFourOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwentyFive, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyFiveOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwentySix, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentySixOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwentySeven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentySevenOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwentyEight, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyEightOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameTwentyNine, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyNineOffset, Globals.RowLeftMargin);
            BuildTextMesh(scrollRectTransform, nameThirty, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThirtyOffset, Globals.RowLeftMargin);

            BuildTextMesh(scrollRectTransform, timeNameOne, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightOneOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwo, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwoOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameThree, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightThreeOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameFour, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightFourOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameFive, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightFiveOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameSix, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightSixOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameSeven, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightSevenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameEight, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightEightOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameNine, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightNineOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameEleven, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightElevenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwelve, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwelveOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameThirteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightThirteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameFourteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightFourteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameFithteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightFithteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameSixteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightSixteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameSeventeen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightSeventeenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameEighteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightEighteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameNineteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightNineteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwenty, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwentyOne, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyOneOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwentyTwo, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyTwoOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwentyThree, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyThreeOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwentyFour, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyFourOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwentyFive, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyFiveOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwentySix, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentySixOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwentySeven, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentySevenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwentyEight, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyEightOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameTwentyNine, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyNineOffset, Globals.TimeLeftMargin);
            BuildTextMesh(scrollRectTransform, timeNameThirty, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightThirtyOffset, Globals.TimeLeftMargin);

            // Save these for use later 
            targetNameTextMeshObject = scrollRectTransform.Find(targetName);

            textMeshObjects.Add(scrollRectTransform.Find(nameOne));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwo));
            textMeshObjects.Add(scrollRectTransform.Find(nameThree));
            textMeshObjects.Add(scrollRectTransform.Find(nameFour));
            textMeshObjects.Add(scrollRectTransform.Find(nameFive));
            textMeshObjects.Add(scrollRectTransform.Find(nameSix));
            textMeshObjects.Add(scrollRectTransform.Find(nameSeven));
            textMeshObjects.Add(scrollRectTransform.Find(nameEight));
            textMeshObjects.Add(scrollRectTransform.Find(nameNine));
            textMeshObjects.Add(scrollRectTransform.Find(nameTen));
            textMeshObjects.Add(scrollRectTransform.Find(nameEleven));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwelve));
            textMeshObjects.Add(scrollRectTransform.Find(nameThirteen));
            textMeshObjects.Add(scrollRectTransform.Find(nameFourteen));
            textMeshObjects.Add(scrollRectTransform.Find(nameFithteen));
            textMeshObjects.Add(scrollRectTransform.Find(nameSixteen));
            textMeshObjects.Add(scrollRectTransform.Find(nameSeventeen));
            textMeshObjects.Add(scrollRectTransform.Find(nameEighteen));
            textMeshObjects.Add(scrollRectTransform.Find(nameNineteen));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwenty));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwentyOne));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwentyTwo));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwentyThree));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwentyFour));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwentyFive));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwentySix));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwentySeven));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwentyEight));
            textMeshObjects.Add(scrollRectTransform.Find(nameTwentyNine));
            textMeshObjects.Add(scrollRectTransform.Find(nameThirty));

            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameOne));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwo));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameThree));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameFour));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameFive));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameSix));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameSeven));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameEight));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameNine));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTen));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameEleven));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwelve));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameThirteen));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameFourteen));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameFithteen));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameSixteen));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameSeventeen));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameEighteen));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameNineteen));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwenty));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwentyOne));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwentyTwo));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwentyThree));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwentyFour));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwentyFive));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwentySix));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwentySeven));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwentyEight));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameTwentyNine));
            timeTextMeshObjects.Add(scrollRectTransform.Find(timeNameThirty));
        }


        // Update the text displayed in the Debuff Box
        public void ResetDebuffPanel()
        { 
            // Try and stop unwanted access to the panel to prevent exceptions
            if (gUiWindowPanel != null && gUiWindowPanel.isActiveAndEnabled && gUiWindowPanel.IsVisible)
            {
                // Parse the list of all debuffs on the current target and display the first MaxDisplayableDebuffs
                for (int i = 0; i < Globals.MaxDisplayableDebuffs; i++)
                {
                    // Reset to an clean list
                    targetNameTextMeshObject.GetComponent<TextMeshProUGUI>().text = "";
                    textMeshObjects[i].GetComponent<TextMeshProUGUI>().text = "";
                    timeTextMeshObjects[i].GetComponent<TextMeshProUGUI>().text = "";
                    // Now update the progress bar colour and time
                    Image image = imageObjects[i].transform.GetComponent<Image>();
                    // Set colour to black on reset
                    image.color = Color.black;
                    image.fillAmount = 0.5f;
                }
            }
        }

        //Update the text displayed in the Debuff Box
        public void UpdateDebuffPanel(EntityData entityData)
        {
            // Try and stop unwanted access to the panel to prevent exceptions
            if (gUiWindowPanel != null && gUiWindowPanel.isActiveAndEnabled && gUiWindowPanel.IsVisible)
            {
                // Parse the list of all debuffs on the current target and display the first MaxDisplayableDebuffs
                for (int i = 0; (i < entityData.debuffData.Count && i < Globals.MaxDisplayableDebuffs); i++)
                {
                    DebuffData debuff = entityData.debuffData[i];

                    targetNameTextMeshObject.GetComponent<TextMeshProUGUI>().text = $" <b>Target:</b> {debuff.targetName.ToUpperSafe()}, {debuff.targetClass}, {debuff.targetKind},\n {entityData.traits}";
                    // Update the target information, leave the leading space in
                    textMeshObjects[i].GetComponent<TextMeshProUGUI>().text = $" {debuff.debuffName} ({debuff.numStacks}/{debuff.maxStacks}), ({debuff.casterName})";
                    if (debuff.debuffDurationRemaining < 60)
                    {
                        timeTextMeshObjects[i].GetComponent<TextMeshProUGUI>().text = $"{debuff.debuffDurationRemaining}s";
                        // Display the remaining time in seconds
                        timeTextMeshObjects[i].GetComponent<TextMeshProUGUI>().text = $"{debuff.debuffDurationRemaining}s ({debuff.consolidatedEncounterUptimePercent.ToString("0")}%)";
                    }
                    else
                    {
                        timeTextMeshObjects[i].GetComponent<TextMeshProUGUI>().text = $"{Math.Floor((decimal)debuff.debuffDurationRemaining/60)}m{debuff.debuffDurationRemaining%60}s";
                        // Display the remaining time in minutes and seconds
                        timeTextMeshObjects[i].GetComponent<TextMeshProUGUI>().text = $"{Math.Floor((decimal)debuff.debuffDurationRemaining/60)}m{Math.Floor((decimal)debuff.debuffDurationRemaining) % 60}s, ({debuff.consolidatedEncounterUptimePercent.ToString("0")}%)";
                    }
                    
                    // Now update the progress bar colour and time
                    Image image = imageObjects[i].transform.GetComponent<Image>();

                    // Set colour based on the spell type
                    image.color = getBarColours(debuff.spellType.ToString());
                    // Set the fill amount  1.0f is full, 0.0f is empty
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
            // Display the panel if the gloabl is set to allow it
            if (Globals.ShowDebuffPanel == true)
            {
                gUiWindowPanel.Show();
            }
        }
    }
}