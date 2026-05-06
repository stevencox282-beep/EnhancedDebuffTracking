using Il2Cpp;
using Il2CppServiceStack;
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

        // List the string values for all MaxDisplayableDebuffs debuffs
        private static List<Color> barColours = new List<Color>()
        {
            { Color.darkBlue}, { Color.darkGreen}, { Color.purple }, { Color.brown }, { Color.red },
            { Color.darkBlue}, { Color.darkGreen}, { Color.purple }, { Color.brown }, { Color.red },
        };

        // Setup lists to aid in the accessing of transform data later on
        Transform targetNameTextMeshObject = new Transform();
        List<Transform> textMeshObjects = new List<Transform>();
        List<Transform> timeTextMeshObjects = new List<Transform>();
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

            // Add in the images that will be the progress bars
            BuildImages();

            // Add in Text Meshs that display the data
            BuildTextMeshs();

            // Ensure the panel is not displayed immediatly, this will trigger on target selection
            gUiWindowPanel.Hide();
        }

        // Tidy up the alloated resources when we logout
        public void RemovePanel()
        {
            // On a /camp out and logging back in the static variables persist and are not garbage collected, explicitly clear them out, we will rebuild them on loading into a zone
            textMeshObjects.Clear();
            timeTextMeshObjects.Clear();
            imageObjects.Clear();
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
            
            return textMesh;
        }

        // Builder function to create an TextMesh
        private void BuildTextMesh(string name, float height, float width, float heightOffset, float widthOffset)
        {
            GameObject gameObject = new GameObject(name);
            // Set its parent to the new window panel (which is parented to Mid)
            gameObject.transform.SetParent(gUiWindowPanel.transform, false);

            TextMeshProUGUI textMesh = BuildTextMeshComponent(gameObject);
            var rectTransformOne = textMesh.rectTransform;
            rectTransformOne.sizeDelta = new Vector2(width, height);
            rectTransformOne.anchorMin = new Vector2(widthOffset, heightOffset);
            rectTransformOne.anchorMax = new Vector2(widthOffset, heightOffset);
            rectTransformOne.anchoredPosition = new Vector2(0f, 0f);
            rectTransformOne.pivot = new Vector2(0f, 0f);
                    }

        // Builder function to create an Image
        private void BuildImage(string name, float height, float width, float heightOffset, float widthOffset)
        {
            GameObject gameObject = new GameObject(name);
            // Set its parent to the new window panel (which is parented to Mid)
            gameObject.transform.SetParent(gUiWindowPanel.transform, false);
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
        public void UpdateDebuffPanel(List<DebuffData> debuffList)
        {
            // Try and stop unwanted access to the panel to prevent exceptions
            if (gUiWindowPanel != null && gUiWindowPanel.isActiveAndEnabled && gUiWindowPanel.IsVisible)
            {
                // Parse the list of all debuffs on the current target and display the first MaxDisplayableDebuffs
                for (int i = 0; (i < debuffList.Count && i < Globals.MaxDisplayableDebuffs); i++)
                {
                    DebuffData debuff = debuffList[i];

                    // Set the target name, not the most optimal as it is set multiple times but lets live with it
                    targetNameTextMeshObject.GetComponent<TextMeshProUGUI>().text = $" <b>Target:</b> {debuff.targetName.ToUpperSafe()}, {debuff.targetClass}, {debuff.targetKind}";
                    // Update the target information, leave the leading space in
                    textMeshObjects[i].GetComponent<TextMeshProUGUI>().text = $" {debuff.debuffName} ({debuff.numStacks}/{debuff.maxStacks} Stacks), ({debuff.casterName})";
                    if (debuff.debuffDurationRemaining < 60)
                    {
                        timeTextMeshObjects[i].GetComponent<TextMeshProUGUI>().text = $"{debuff.debuffDurationRemaining}s";
                        // Display the remaining time in seconds
                        timeTextMeshObjects[i].GetComponent<TextMeshProUGUI>().text = $"{debuff.debuffDurationRemaining}s ({debuff.uptimePercent.ToString("0")}%)";
                    }
                    else
                    {
                        timeTextMeshObjects[i].GetComponent<TextMeshProUGUI>().text = $"{Math.Floor((decimal)debuff.debuffDurationRemaining/60)}m{debuff.debuffDurationRemaining%60}s";
                        // Display the remaining time in minutes and seconds
                        timeTextMeshObjects[i].GetComponent<TextMeshProUGUI>().text = $"{Math.Floor((decimal)debuff.debuffDurationRemaining/60)}m{Math.Floor((decimal)debuff.debuffDurationRemaining) % 60}s, ({debuff.uptimePercent.ToString("0")}%)";
                    }
                    
                    // Now update the progress bar colour and time
                    Image image = imageObjects[i].transform.GetComponent<Image>();

                    // Set colour based on the debuff type
                    image.color = barColours[i];
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