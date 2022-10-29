using UnityEngine;

public class LiftAspectReplacement : MonoBehaviour
{
    [Header("ToReplace")]
    public ExistingAssets.Lifts LiftType;

    [Header("AssetNames")]
    [Tooltip("Can be left blank, in which case the default asset will be used")]
    public string EntryStationAssetName;
    [Tooltip("Can be left blank, in which case the default asset will be used")]
    public string ExitStationAssetName;
    [Tooltip("Can be left blank, in which case the default asset will be used")]
    public string VehicleAssetName;
    [Tooltip("Can be left blank, in which case the default asset will be used")]
    public string TowerAssetName;
    [Tooltip("Can be left blank, in which case the default asset will be used")]
    public string MidStationAssetName;

    [Header("Technology Info")]
    [Tooltip("Can be left blank, in which case the default lift will be used")]
    public string UpgradePath;

    [Header("Other")]
    [Tooltip("Can be left blank, in which case the default thumbnail will be used")]
    public Sprite thumbnail;

    [Header("Settings to Adjust")]
    public float NewVehicleSpeed;
    public float NewVehicleSpacing;
    public float NewMinLength;
    public float NewMaxLength;
}
