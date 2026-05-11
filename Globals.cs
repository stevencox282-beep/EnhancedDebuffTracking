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

    public static int MaxDisplayableDebuffs = 20;
    public static int MsToSeconds = 1000;

    // Panel / TextMesh Constants
    public static float RowLeftMargin = 0.05f;
    public static int PanelHeight = 600;
    public static int PanelWidth = 400;
    public static int NameMeshWidth = (PanelWidth - 120); // Give space for the time textmesh
    public static int NameMeshHeight = 20;
    public static float TimeLeftMargin = 0.75f; // The Time mesh must start after the name text mesh ends and the progress bars end
    public static int   TimeMeshWidth = 500;
    public static int   TimeMeshHeight = NameMeshHeight;
    public static int FontSize = 12;

    // Progress Bar Display Co-ordinates
    public static float TopMargin         = 0.05f;
    public static float InterBarOffset    = 0.04f;
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
    public static float HeightElevenOffset = HeightTenOffset - InterBarOffset;
    public static float HeightTwelveOffset = HeightElevenOffset - InterBarOffset;
    public static float HeightThirteenOffset = HeightTwelveOffset - InterBarOffset;
    public static float HeightFourteenOffset = HeightThirteenOffset - InterBarOffset;
    public static float HeightFithteenOffset = HeightFourteenOffset - InterBarOffset;
    public static float HeightSixteenOffset = HeightFithteenOffset - InterBarOffset;
    public static float HeightSeventeenOffset = HeightSixteenOffset - InterBarOffset;
    public static float HeightEighteenOffset = HeightSeventeenOffset - InterBarOffset;
    public static float HeightNineteenOffset = HeightEighteenOffset - InterBarOffset;
    public static float HeightTwentyOffset = HeightNineteenOffset - InterBarOffset;

}

