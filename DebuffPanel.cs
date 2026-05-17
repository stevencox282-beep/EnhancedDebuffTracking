using Il2Cpp;
using Il2CppServiceStack;
using Il2CppSystem.Data;
using Il2CppSystem.Security.Cryptography;
using Il2CppTMPro;
using MelonLoader;
using Unity.Collections;
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
        private static string baseTargetName = "EDT_TargetName_EDT_";
        private static string baseTextName = "EDT_TextName_EDT_";
        private static string baseTimeTextName = "EDT_TimeTextName_EDT_";
        private static string baseImageName = "EDT_ImageName_EDT_";

        public static Scrollbar xScrollBar = new Scrollbar();
        public static Scrollbar yScrollBar = new Scrollbar();

        // Setup lists to aid in the accessing of transform data later on
        List<Transform> targetNameTextMeshObject = new List<Transform>();
        List<Transform> textMeshObjects = new List<Transform>();
        List<Transform> timeTextMeshObjects = new List<Transform>();
        List<Transform> imageObjects = new List<Transform>();
        UITutorialPopup gTutorialPopup = new UITutorialPopup();
        
        // Holds the panel
        private static UIWindowPanel gUiWindowPanel  = null;

        

        // Tidy up the alloated resources when we logout
        public void ClearPanelLists()
        {
            // Static variables can persist and not be garbage collected on zoning, logout or panel reloading so explicitly clear them out, we will rebuild them on loading into a zone
            targetNameTextMeshObject.Clear();
            textMeshObjects.Clear();
            timeTextMeshObjects.Clear();
            imageObjects.Clear();

        }

        public void PreserveRequiredTransforms()
        {
            gTutorialPopup = UIPanelRoots.Instance.Mid.transform.GetComponentInChildren<UITutorialPopup>();
        }

        // Displays a panel with to contain the data we want
        public void DisplayPanel(string panelName, Transform parentPanel)
        {
            // This allows us to remake the panel with any number of rows we want without having left over transforms corrupting the display
            if (gUiWindowPanel != null)
            {
                Destroy(gUiWindowPanel.gameObject);
                Destroy(gUiWindowPanel);
            }

            // Setup the general panel parameters
            GameObject gameObject = new GameObject(panelName);
            // Add the panel to the Mid, this ensures we get rendered
            gameObject.transform.SetParent(parentPanel);
            gameObject.layer = Layers.UI;

            // Add the necessary component for a panel
            CanvasRenderer canvasRenderer = gameObject.AddComponent<CanvasRenderer>();
            CanvasGroup canvasGroup = gameObject.AddComponent<CanvasGroup>();
            UIDraggable uiDraggable = gameObject.AddComponent<UIDraggable>();
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();

            gUiWindowPanel = gameObject.AddComponent<UIWindowPanel>();

            // Block Raycasts to work around wonky click detection on the close button due other UI elements overlapping the close button image
            // I am not going to spend time making all my TextMesh's layout perfectly for this mod so block raycasts instead
            canvasGroup.blocksRaycasts = true;

            // Setup the Window Panel
            uiDraggable._windowPanel = gUiWindowPanel;
            gUiWindowPanel.CanvasGroup = canvasGroup;
            gUiWindowPanel._displayName = panelName;

            // Add the MANDATORY elements to a panel, the compilor will not error if you don't do this but nothing will work
            BuildCloseButtonAndBackground(rectTransform, gameObject);

            // Set the panel size based on the number of rows we have to draw
            SetPanelSize(panelName);

            // Add in the row data
            AddPanelRows(panelName);

            // Ensure the panel is displayed immediatly
            gUiWindowPanel.Show();
        }

        // Sets the size of the panel based on the number of rows to add
        public void SetPanelSize(string panelName)
        {
            // Get the RectTransform to add the rows too
            GameObject gameObject = gUiWindowPanel.gameObject;
            RectTransform rectTransform = gameObject.transform.GetComponent<RectTransform>();

            // We need to know how much space each row takes up, then multiply that by the number of rows.
            // The space we need per row 
            int heightPerRow = Globals.NameMeshHeight;
            int totalHeightNeeded = (heightPerRow + 6) * Globals.NumDisplayableDebuffs;
            // We can not change the width, just the height
            Vector2 panelSize = new Vector2(Globals.DefaultPanelWidth, totalHeightNeeded);
            rectTransform.pivot = new Vector2(0, 1);
            rectTransform.anchoredPosition = new Vector2(-(panelSize.x / 2), panelSize.y / 2);
            rectTransform.localScale = new Vector3(1, 1, 1);
            rectTransform.sizeDelta = panelSize;
        }

        // Adds all the images and text meshes to the panel
        public void AddPanelRows(string panelName)
        {
            // Get the RectTransform to add the rows too
            GameObject gameObject = gUiWindowPanel.gameObject;
            RectTransform rectTransform = gameObject.transform.GetComponent<RectTransform>();

            // Add in the images that will be the progress bars
            BuildImages(rectTransform);

            // Add in Text Meshs that display the data
            BuildTextMeshs(rectTransform);
        }

        // Constructs the close button and set the background
        private void BuildCloseButtonAndBackground(Transform parentPanel, GameObject gameObject)
        {
            // Source for copying button and backgrounds            
            Transform tutorialButton = gTutorialPopup.transform.GetChild(0);

            // Initialise the background for the new panel (MANDATORY)
            Image imageToCopy = gTutorialPopup.GetComponent<Image>();
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
        private void BuildTextMesh(RectTransform rectTransform, string name, int height, int width, float heightOffset, float widthOffset)
        {
            GameObject gameObject = new GameObject(name);
            // Set its parent to the new window panel (which is parented to Mid)
            gameObject.transform.SetParent(rectTransform, false);
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
            image.color = Color.pink;
            image.fillAmount = 0.5f; // 1.0f is full 0.0f is empty
            return image;
        }

        // Builds all images (progress bars) to be display in the panel 
        private void BuildImages(RectTransform rectTransform) 
        {
            float heightOffset = 0.0f;
            float interBarOffset = 0.0f;
            DebuffPanelUtils.GetOffsetsForPanel(ref heightOffset, ref interBarOffset);

            // Make all the progress bars
            for (int i = 0 ; i < Globals.NumDisplayableDebuffs; i++)
            {
                string imageName = $"{baseImageName} + {i}";
                BuildImage(rectTransform, imageName, Globals.NameMeshHeight, Globals.NameMeshWidth, heightOffset, Globals.RowLeftMargin);
                imageObjects.Add(rectTransform.transform.Find(imageName));
                heightOffset = heightOffset - interBarOffset;
            }
        }

        // Builds all TextMeshes (debuff/time) to be display in the panel
        private void BuildTextMeshs(RectTransform rectTransform)
        {
            // Text Mesh for Target Name
            BuildTextMesh(rectTransform, baseTargetName, Globals.NameMeshHeight, Globals.NameMeshWidth, 1.0f, 0.0f);
            targetNameTextMeshObject.Add(rectTransform.Find(baseTargetName));

            // Build the meshes
            float heightOffset = 0.0f;
            float interBarOffset = 0.0f;
            DebuffPanelUtils.GetOffsetsForPanel(ref heightOffset, ref interBarOffset);

            for (int i = 0; i < Globals.NumDisplayableDebuffs; i++)
            {
                string textName = $"{baseTextName} + {i}";
                string timeTextName = $"{baseTimeTextName} + {i}";
                BuildTextMesh(rectTransform, textName, Globals.NameMeshHeight, Globals.NameMeshWidth, heightOffset, Globals.RowLeftMargin);
                BuildTextMesh(rectTransform, timeTextName, Globals.TimeMeshHeight, Globals.TimeMeshWidth, heightOffset, Globals.TimeLeftMargin);
                textMeshObjects.Add(rectTransform.Find(textName));
                timeTextMeshObjects.Add(rectTransform.Find(timeTextName));
                heightOffset = heightOffset - interBarOffset;
            }
        }


        // Update the text displayed in the Debuff Box
        public void ClearPanel()
        { 
            // Try and stop unwanted access to the panel to prevent exceptions
            if (gUiWindowPanel != null && gUiWindowPanel.isActiveAndEnabled && gUiWindowPanel.IsVisible)
            {
                targetNameTextMeshObject[0].GetComponent<TextMeshProUGUI>().text = "";
                // Parse the list of all debuffs on the current target and display the first MaxDisplayableDebuffs
                for (int i = 0; i < Globals.NumDisplayableDebuffs; i++)
                {
                    // Reset to an clean list
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
        public void UpdatePanel(EntityData entityData)
        {
            // Try and stop unwanted access to the panel to prevent exceptions
            if (gUiWindowPanel != null && gUiWindowPanel.isActiveAndEnabled && gUiWindowPanel.IsVisible && entityData.monsterNetworkId != null & entityData.monsterNetworkId != "")
            {
                // Parse the list of all debuffs on the current target and display the first MaxDisplayableDebuffs
                for (int i = 0; (i < entityData.debuffData.Count && i < Globals.NumDisplayableDebuffs); i++)
                {
                    DebuffData debuff = entityData.debuffData[i];

                    targetNameTextMeshObject[0].GetComponent<TextMeshProUGUI>().text = $" <b>Target:</b> {debuff.targetName.ToUpperSafe()}, {debuff.targetClass}, {debuff.targetKind},\n {entityData.traits}";
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
                    image.color = DebuffPanelUtils.getBarColours(debuff.spellType.ToString());
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