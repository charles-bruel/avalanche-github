using UnityEngine;

public class CustomLiftDefinition : MonoBehaviour
{
    [Header("Basic Display Info")]
    public string Name;
    public string ID;
    public string Description;
    public Sprite Thumbnail;
    [Tooltip("What section of the menu and what upgrades. Other stuff too.")]
    public ExistingAssets.Lifts TemplateType;

    [Header("Technology Info")]
    public string UpgradePath;
    public string[] RequiredTechnologies;

    [Header("Economy Info")]
    public int RunningCost;
    public int ConstructionCostLinear;
    public int ConstructionCostConstant;

    [Header("Pulse Info")]
    public bool PulseEnabled;
    public float PulseSpeedMul;
    public float PulsePaddingAnte;
    public float PulsePaddingPost;

    [Header("More Info")]
    public float Attractiveness;
    public float ConstructionCost;
    public float Speed;
    public uint WorkerCount;
    public float VehicleSpacing;
    [Tooltip("Cable sag is calculated as a caternery line with a length equal to 1+(0.01*SagAmount). So a 100 value makes a cable twice as long. In general, you will probably want a sub 1 value.")]
    public float SagAmount;
    public float CableThicknessMultiplier = 1;
    [Tooltip("The x, y, and z value specify a normal offset. The w specifies cable thickness, as a ratio to the default thickness. You probably care about X (side to side) and Y (up and down) the most.")]
    public Vector4[] ExtraCables;

    [Header("Construction Constraints")]
    public ConstraintsStruct ElevationHigh;
    public ConstraintsStruct ElevationLow;
    public ConstraintsStruct SlopeHigh;
    public ConstraintsStruct SlopeLow;
    public ConstraintsStruct TowerDistance;
    public float MaxDistance;
    public float MinDistance;

    [Header("AssetNames")]
    public string EntryStationAssetName;
    public string ExitStationAssetName;
    public string VehicleAssetName;
    public string TowerAssetName;
    public string MidSectionName;
    public bool MidSectionIsMidStation = false;//Unused for now

    [Header("Advanced Vehicle Settings")]
    public string SecondaryVehicleName;
    public int PrimaryVehicleNumberPerSecondaryVehicle;
    public int GroupingAmount = 1;
    public float GroupSpacing;
}
