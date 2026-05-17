using Il2Cpp;
using UnityEngine;

namespace EnhancedDebuffTracking;

public static class Globals
{
    public static bool PlayerIsLoaded = false;
    public static bool ShowDebuffPanel = true;
    public static EntityPlayerGameObject? LocalPlayer = null;
    public static bool HasSetUpUI = false;
    public static EntityNpcGameObject? TrackedOffensiveEntity = null;
    public static float MinimumTrackingDistance = 3f;
    public static Dictionary<uint, EntityPlayerGameObject> PlayersInRange = new Dictionary<uint, EntityPlayerGameObject>();
    public static Dictionary<uint, Vector3> PlayersInRangeLastPosition = new Dictionary<uint, Vector3>();
    public static Dictionary<uint, EntityNpcGameObject> MonstersInRange = new Dictionary<uint, EntityNpcGameObject>();
    public static Dictionary<uint, Vector3> MonstersInRangeLastPosition = new Dictionary<uint, Vector3>();

    public static string SetNumberOfRowsCommand = "setdebuffrows";
    public static int NumDisplayableDebuffs = 10;
    public static int MsToSeconds = 1000;
    public static Vector2 maxPanelSize = new Vector2(1000,1000);

    // Panel / TextMesh Constants
    public static float RowLeftMargin = 0.05f;
    public static int DefaultPanelHeight = 540; // y-axis
    public static int DefaultPanelWidth = 300; // x-axis

    public static int NameMeshWidth = 250;
    public static int NameMeshHeight = 12;
    public static float TimeLeftMargin = 0.75f; // The Time mesh must start after the name text mesh ends and the progress bars end
    public static int   TimeMeshHeight = NameMeshHeight;
    public static int TimeMeshWidth = 75;
    public static int FontSize = 10;

    // Progress Bar Display Co-ordinates
    public static float TopMargin         = 0.04f;
    public static float InterBarOffset    = 0.028f;
}

