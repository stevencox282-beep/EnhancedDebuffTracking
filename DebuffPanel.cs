using Il2Cpp;
using Il2CppServiceStack;
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
            RectMask2D         rectMask2D = gameObject.AddComponent<RectMask2D>();
            ScrollRect         scrollRect = gameObject.AddComponent<ScrollRect>();

            gUiWindowPanel = gameObject.AddComponent<UIWindowPanel>();
            

            // Block Raycasts to work around wonky click detection on the close button due other UI elements overlapping the close button image
            // I am not going to spend time making all my TextMesh's layout perfectly for this mod so block raycasts instead
            canvasGroup.blocksRaycasts = true;

            // Setup the Window Panel
            uiDraggable._windowPanel = gUiWindowPanel;
            gUiWindowPanel.CanvasGroup = canvasGroup;
            gUiWindowPanel._displayName = panelName;
            gUiWindowPanel.Resizable = true;


            // The content that can be scrolled. It should be a child of the GameObject with ScrollRect on it.
            scrollRect.content = rectTransform;
            scrollRect.vertical = true;
            scrollRect.horizontal = false;
            
            // Setup the default position of the panel and its general parameters
            rectTransform.sizeDelta = panelSize;
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchoredPosition = new Vector2(-(panelSize.x / 2), panelSize.y / 2);
            rectTransform.localScale = new Vector3(1, 1, 1);
            
            // Setup the 2DRect Mask to prevent elements outide the pannel to not be rendered
            RectTransform rectMask2DTransform = rectMask2D.rectTransform;
            rectMask2DTransform.sizeDelta = new Vector2(200,200); // Allows clipping outside this range
            rectMask2DTransform.pivot = new Vector2(0, 1);
            rectMask2DTransform.anchoredPosition = new Vector2(-(panelSize.x / 2), panelSize.y / 2);

            // TODO - Is this actually necessary?
            //ContentSizeFitter contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
            //contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            //ontentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

            // Adds the resize control to the Panel, hijacked from the chat box
            AddResizeControl(rectTransform, panelSize);

            // Add the MANDATORY elements to a panel, the compilor will not error if you don't do this but nothing will work
            BuildCloseButtonAndBackground(parentPanel, gameObject);

            // Add in the images that will be the progress bars
            BuildImages();

            // Add in Text Meshs that display the data
            BuildTextMeshs();

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
            copyUIResizeHandle.MaxSize = panelSize;
            copyUIResizeHandle.MinSize = new Vector2(0, 0);

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
        private void BuildTextMesh(string name, int height, int width, float heightOffset, float widthOffset)
        {
            GameObject gameObject = new GameObject(name);
            // Set its parent to the new window panel (which is parented to Mid)
            gameObject.transform.SetParent(gUiWindowPanel.transform, false);
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
        private void BuildImage(string name, int height, int width, float heightOffset, float widthOffset)
        {
            GameObject gameObject = new GameObject(name);
            // Set its parent to the new window panel (which is parented to Mid)
            gameObject.transform.SetParent(gUiWindowPanel.transform, false);
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
        private void BuildImages() 
        {
            // Make all the progress bars
            BuildImage(imageNameOne,   Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightOneOffset, Globals.RowLeftMargin);
            BuildImage(imageNameTwo,   Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwoOffset, Globals.RowLeftMargin);
            BuildImage(imageNameThree, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThreeOffset, Globals.RowLeftMargin);
            BuildImage(imageNameFour,  Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFourOffset, Globals.RowLeftMargin);
            BuildImage(imageNameFive,  Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFiveOffset, Globals.RowLeftMargin);
            BuildImage(imageNameSix,   Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSixOffset, Globals.RowLeftMargin);
            BuildImage(imageNameSeven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSevenOffset, Globals.RowLeftMargin);
            BuildImage(imageNameEight, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightEightOffset, Globals.RowLeftMargin);
            BuildImage(imageNameNine,  Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightNineOffset, Globals.RowLeftMargin);
            BuildImage(imageNameTen,   Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTenOffset, Globals.RowLeftMargin);
            BuildImage(imageNameEleven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightElevenOffset, Globals.RowLeftMargin);
            BuildImage(imageNameTwelve, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwelveOffset, Globals.RowLeftMargin);
            BuildImage(imageNameThirteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThirteenOffset, Globals.RowLeftMargin);
            BuildImage(imageNameFourteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFourteenOffset, Globals.RowLeftMargin);
            BuildImage(imageNameFithteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFithteenOffset, Globals.RowLeftMargin);
            BuildImage(imageNameSixteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSixteenOffset, Globals.RowLeftMargin);
            BuildImage(imageNameSeventeen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSeventeenOffset, Globals.RowLeftMargin);
            BuildImage(imageNameEighteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightEighteenOffset, Globals.RowLeftMargin);
            BuildImage(imageNameNineteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightNineteenOffset, Globals.RowLeftMargin);
            BuildImage(imageNameTwenty, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyOffset, Globals.RowLeftMargin);
//            BuildImage(imageNameTwentyOne, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyOneOffset, Globals.RowLeftMargin);
//            BuildImage(imageNameTwentyTwo, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyTwoOffset, Globals.RowLeftMargin);
//            BuildImage(imageNameTwentyThree, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyThreeOffset, Globals.RowLeftMargin);
//            BuildImage(imageNameTwentyFour, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyFourOffset, Globals.RowLeftMargin);
//            BuildImage(imageNameTwentyFive, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyFiveOffset, Globals.RowLeftMargin);
//            BuildImage(imageNameTwentySix, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentySixOffset, Globals.RowLeftMargin);
//            BuildImage(imageNameTwentySeven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentySevenOffset, Globals.RowLeftMargin);
//            BuildImage(imageNameTwentyEight, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyEightOffset, Globals.RowLeftMargin);
//            BuildImage(imageNameTwentyNine, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyNineOffset, Globals.RowLeftMargin);
//            BuildImage(imageNameThirty, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThirtyOffset, Globals.RowLeftMargin);

            // Save these for use later 
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
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameEleven));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwelve));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameThirteen));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameFourteen));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameFithteen));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameSixteen));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameSeventeen));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameEighteen));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameNineteen));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwenty));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwentyOne));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwentyTwo));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwentyThree));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwentyFour));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwentyFive));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwentySix));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwentySeven));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwentyEight));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameTwentyNine));
            imageObjects.Add(gUiWindowPanel.transform.Find(imageNameThirty));
        }

        // Builds all TextMeshes (debuff/time) to be display in the panel
        private void BuildTextMeshs()
        {
            // Text Mesh for Target Name
            BuildTextMesh(targetName, Globals.NameMeshHeight, Globals.NameMeshWidth, 1f, 0f);

            // Make all the meshes that will sit on top of the bars
            BuildTextMesh(nameOne, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightOneOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameTwo, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwoOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameThree, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThreeOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameFour, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFourOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameFive, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFiveOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameSix, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSixOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameSeven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSevenOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameEight, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightEightOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameNine, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightNineOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameTen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTenOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameEleven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightElevenOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameTwelve, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwelveOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameThirteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThirteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameFourteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFourteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameFithteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightFithteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameSixteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSixteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameSeventeen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightSeventeenOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameEighteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightEighteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameNineteen, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightNineteenOffset, Globals.RowLeftMargin);
            BuildTextMesh(nameTwenty, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyOffset, Globals.RowLeftMargin);
//            BuildTextMesh(nameTwentyOne, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyOneOffset, Globals.RowLeftMargin);
//            BuildTextMesh(nameTwentyTwo, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyTwoOffset, Globals.RowLeftMargin);
//            BuildTextMesh(nameTwentyThree, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyThreeOffset, Globals.RowLeftMargin);
//            BuildTextMesh(nameTwentyFour, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyFourOffset, Globals.RowLeftMargin);
//            BuildTextMesh(nameTwentyFive, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyFiveOffset, Globals.RowLeftMargin);
//            BuildTextMesh(nameTwentySix, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentySixOffset, Globals.RowLeftMargin);
//            BuildTextMesh(nameTwentySeven, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentySevenOffset, Globals.RowLeftMargin);
//            BuildTextMesh(nameTwentyEight, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyEightOffset, Globals.RowLeftMargin);
//            BuildTextMesh(nameTwentyNine, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightTwentyNineOffset, Globals.RowLeftMargin);
//            BuildTextMesh(nameThirty, Globals.NameMeshHeight, Globals.NameMeshWidth, Globals.HeightThirtyOffset, Globals.RowLeftMargin);

            BuildTextMesh(timeNameOne, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightOneOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameTwo, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwoOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameThree, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightThreeOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameFour, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightFourOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameFive, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightFiveOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameSix, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightSixOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameSeven, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightSevenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameEight, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightEightOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameNine, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightNineOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameTen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameEleven, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightElevenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameTwelve, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwelveOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameThirteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightThirteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameFourteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightFourteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameFithteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightFithteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameSixteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightSixteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameSeventeen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightSeventeenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameEighteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightEighteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameNineteen, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightNineteenOffset, Globals.TimeLeftMargin);
            BuildTextMesh(timeNameTwenty, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyOffset, Globals.TimeLeftMargin);
//            BuildTextMesh(timeNameTwentyOne, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyOneOffset, Globals.TimeLeftMargin);
//            BuildTextMesh(timeNameTwentyTwo, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyTwoOffset, Globals.TimeLeftMargin);
//            BuildTextMesh(timeNameTwentyThree, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyThreeOffset, Globals.TimeLeftMargin);
//            BuildTextMesh(timeNameTwentyFour, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyFourOffset, Globals.TimeLeftMargin);
//            BuildTextMesh(timeNameTwentyFive, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyFiveOffset, Globals.TimeLeftMargin);
//            BuildTextMesh(timeNameTwentySix, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentySixOffset, Globals.TimeLeftMargin);
//            BuildTextMesh(timeNameTwentySeven, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentySevenOffset, Globals.TimeLeftMargin);
//            BuildTextMesh(timeNameTwentyEight, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyEightOffset, Globals.TimeLeftMargin);
//            BuildTextMesh(timeNameTwentyNine, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightTwentyNineOffset, Globals.TimeLeftMargin);
//            BuildTextMesh(timeNameThirty, Globals.TimeMeshHeight, Globals.TimeMeshWidth, Globals.HeightThirtyOffset, Globals.TimeLeftMargin);

            // Save these for use later 
            targetNameTextMeshObject = gUiWindowPanel.transform.Find(targetName);

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
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameEleven));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameTwelve));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameThirteen));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameFourteen));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameFithteen));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameSixteen));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameSeventeen));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameEighteen));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameNineteen));
            textMeshObjects.Add(gUiWindowPanel.transform.Find(nameTwenty));

            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameOne));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameTwo));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameThree));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameFour));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameFive));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameSix));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameSeven));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameEight));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameNine));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameTen));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameEleven));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameTwelve));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameThirteen));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameFourteen));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameFithteen));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameSixteen));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameSeventeen));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameEighteen));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameNineteen));
            timeTextMeshObjects.Add(gUiWindowPanel.transform.Find(timeNameTwenty));
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