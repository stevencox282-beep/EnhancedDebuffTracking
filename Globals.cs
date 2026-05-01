using Il2Cpp;
using UnityEngine;

namespace EnhancedDebuffTracking;

public static class Globals
{
    public static bool PlayerIsLoaded = false;
    public static EntityPlayerGameObject? LocalPlayer = null;
    public static bool HasSetUpUI = false;
    public static EntityNpcGameObject? TrackedOffensiveEntity = null;
    public static float MinimumTrackingDistance = 3f;
    public static Dictionary<uint, EntityPlayerGameObject> PlayersInRange = new Dictionary<uint, EntityPlayerGameObject>();
    public static Dictionary<uint, Vector3> PlayersInRangeLastPosition = new Dictionary<uint, Vector3>();
    public static Dictionary<uint, EntityNpcGameObject> MonstersInRange = new Dictionary<uint, EntityNpcGameObject>();
    public static Dictionary<uint, Vector3> MonstersInRangeLastPosition = new Dictionary<uint, Vector3>();

    public static int NumHoursInADay = 24;
    // 24 hours in game is 1 hour in real life.  1 hour in game is 1/24th of a real hour.
    public static double ConvertInGameTimeToRealTime = 0.0416666666666667;
    public static int MinutesInHour = 60;

    // Outer box constants
    public static int OuterBoxWidth = 450;
    public static int OuterBoxHeight = 30;
    public static int OuterBoxLeftOffset = (OuterBoxWidth / 2); // Previously was 200

    // Inner Label constants
    public static int LabelHeightOffset = 15;
    public static int LabelOffset = 150;
    public static int FontSize = 16;

    // Panel / TextMesh Constants
    public static int PanelMeshHeight = 50;
    public static int PanelWidth = 500;
    public static int MeshWidth = 150;
}

