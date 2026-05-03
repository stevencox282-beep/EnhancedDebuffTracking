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

    public static int MaxDisplayableDebuffs = 10;

    // Panel / TextMesh Constants
    public static float LeftMargin = 0.05f;
    public static int PanelHeight = 300;
    public static int PanelWidth = 400;
    public static int MeshWidth = (PanelWidth-50);
    public static int MeshHeight = 20;
    public static int FontSize = 14;

    // Progress Bar Display Co-ordinates
    public static float TopMargin = 0.15f;
    public static float InterBarOffset = 0.09f;
    public static float HeightOne   = 1f - TopMargin;
    public static float HeightTwo   = HeightOne - InterBarOffset;
    public static float HeightThree = HeightTwo - InterBarOffset;
    public static float HeightFour  = HeightThree - InterBarOffset;
    public static float HeightFive  = HeightFour - InterBarOffset;
    public static float HeightSix   = HeightFive - InterBarOffset;
    public static float HeightSeven = HeightSix - InterBarOffset; 
    public static float HeightEight = HeightSeven - InterBarOffset; 
    public static float HeightNine  = HeightEight - InterBarOffset;
    public static float HeightTen   = HeightNine - InterBarOffset;
}

