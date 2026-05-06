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

    public static int MaxDisplayableDebuffs = 10;

    // Panel / TextMesh Constants
    public static float RowLeftMargin = 0.05f;
    public static int PanelHeight = 300;
    public static int PanelWidth = 400;
    public static int MeshWidth = (PanelWidth - 75); // Give space for the time remaining textmesh
    public static float TimeLeftMargin = 0.875f; // The Time mesh must start after the name text mesh ends
    public static int TimeMeshWidth = 500;
    public static int MeshHeight = 20;
    public static int TimeMeshHeight = MeshHeight;
    public static int FontSize = 14;

    // Progress Bar Display Co-ordinates
    public static float TopMargin         = 0.15f;
    public static float InterBarOffset    = 0.09f;
    public static float HeightOneOffset   = 1f - TopMargin;
    public static float HeightTwoOffset   = HeightOneOffset - InterBarOffset;
    public static float HeightThreeOffset = HeightTwoOffset - InterBarOffset;
    public static float HeightFourOffset  = HeightThreeOffset - InterBarOffset;
    public static float HeightFiveOffset  = HeightFourOffset - InterBarOffset;
    public static float HeightSixOffset   = HeightFiveOffset - InterBarOffset;
    public static float HeightSevenOffset = HeightSixOffset - InterBarOffset; 
    public static float HeightEightOffset = HeightSevenOffset - InterBarOffset; 
    public static float HeightNineOffset  = HeightEightOffset - InterBarOffset;
    public static float HeightTenOffset   = HeightNineOffset - InterBarOffset;
}

