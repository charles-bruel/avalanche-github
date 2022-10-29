using System;
using UnityEngine;

// Token: 0x02000926 RID: 2342
public class CustomLiftDefinition : MonoBehaviour
{
	// Token: 0x04002195 RID: 8597
	[Header("Basic Display Info")]
	public string Name;

	// Token: 0x04002196 RID: 8598
	public string ID;

	// Token: 0x04002197 RID: 8599
	public string Description;

	// Token: 0x04002198 RID: 8600
	public Sprite Thumbnail;

	// Token: 0x04002199 RID: 8601
	[Tooltip("What section of the menu and what upgrades. Other stuff too.")]
	public ExistingAssets.Lifts TemplateType;

	// Token: 0x0400219A RID: 8602
	[Header("More Info")]
	public float Attractiveness;

	// Token: 0x0400219B RID: 8603
	public float ConstructionCost;

	// Token: 0x0400219C RID: 8604
	public float Speed;

	// Token: 0x0400219D RID: 8605
	public uint WorkerCount;

	// Token: 0x0400219E RID: 8606
	[Header("Construction Constraints")]
	public ConstraintsStruct ElevationHigh;

	// Token: 0x0400219F RID: 8607
	public ConstraintsStruct ElevationLow;

	// Token: 0x040021A0 RID: 8608
	public ConstraintsStruct SlopeHigh;

	// Token: 0x040021A1 RID: 8609
	public ConstraintsStruct SlopeLow;

	// Token: 0x040021A2 RID: 8610
	public ConstraintsStruct TowerDistance;

	// Token: 0x040021A3 RID: 8611
	[Header("AssetNames")]
	public string EntryStationAssetName;

	// Token: 0x040021A4 RID: 8612
	public string ExitStationAssetName;

	// Token: 0x040021A5 RID: 8613
	public string VehicleAssetName;

	// Token: 0x040021A6 RID: 8614
	public string TowerAssetName;

	// Token: 0x040021A7 RID: 8615
	public float VehicleSpacing;

	// Token: 0x040021A8 RID: 8616
	public float MaxDistance;

	// Token: 0x040021A9 RID: 8617
	public float MinDistance;

	// Token: 0x040021AA RID: 8618
	[Tooltip("Cable sag is calculated as a caternery line with a length equal to 1+(0.01*SagAmount). So a 100 value makes a cable twice as long. In general, you will probably want a sub 1 value.")]
	public float SagAmount;

	// Token: 0x040021AB RID: 8619
	public float CableThicknessMultiplier = 1f;

	// Token: 0x040021AC RID: 8620
	[Tooltip("The x, y, and z value specify a normal offset. The w specifies cable thickness, as a ratio to the default thickness. You probably care about X (side to side) and Y (up and down) the most.")]
	public Vector4[] ExtraCables;

	// Token: 0x040021AD RID: 8621
	[Header("Advanced Vehicle Settings")]
	public string SecondaryVehicleName;

	// Token: 0x040021AE RID: 8622
	public int PrimaryVehicleNumberPerSecondaryVehicle;

	// Token: 0x040021AF RID: 8623
	public string MidSectionName;

	// Token: 0x040021B0 RID: 8624
	public bool MidSectionIsMidStation;

	// Token: 0x040021B1 RID: 8625
	[Header("Technology Info")]
	public string UpgradePath;

	// Token: 0x040021B2 RID: 8626
	public string[] RequiredTechnologies;

	// Token: 0x040021B3 RID: 8627
	[Header("Economy Info")]
	public int RunningCost;

	// Token: 0x040021B4 RID: 8628
	public int ConstructionCostLinear;

	// Token: 0x040021B5 RID: 8629
	public int ConstructionCostConstant;

	// Token: 0x040021B6 RID: 8630
	[Header("Pulse Info")]
	public bool PulseEnabled;

	// Token: 0x040021B7 RID: 8631
	public float PulseSpeedMul;

	// Token: 0x040021B8 RID: 8632
	public int GroupingAmount = 1;

	// Token: 0x040021B9 RID: 8633
	public float GroupSpacing;

	// Token: 0x040021BA RID: 8634
	public float PulsePaddingAnte;

	// Token: 0x040021BB RID: 8635
	public float PulsePaddingPost;
}
