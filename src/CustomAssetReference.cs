using System;
using UnityEngine;

// Token: 0x0200092D RID: 2349
public class CustomAssetRefrence : MonoBehaviour
{
	// Token: 0x040021E9 RID: 8681
	public CustomLiftDefinition CustomLiftDefinition;

	// Token: 0x040021EA RID: 8682
	public CustomLiftEntryStationDefinition CustomLiftEntryStationDefinition;

	// Token: 0x040021EB RID: 8683
	public CustomLiftExitStationDefinition CustomLiftExitStationDefinition;

	// Token: 0x040021EC RID: 8684
	public CustomLiftTowerDefinition CustomLiftTowerDefinition;

	// Token: 0x040021ED RID: 8685
	public CustomLiftVehicleDefinition CustomLiftVehicleDefinition;

	// Token: 0x040021EE RID: 8686
	public CustomAssetRefrence.Type AssetType;

	// Token: 0x040021EF RID: 8687
	public LiftChairComponent SecondaryLiftVehicleDefinition;

	// Token: 0x040021F0 RID: 8688
	public CustomLiftTurnSectionDefinition CustomLiftTurnSectionDefinition;

	// Token: 0x0200092E RID: 2350
	public enum Type
	{
		// Token: 0x040021F2 RID: 8690
		LIFT,
		// Token: 0x040021F3 RID: 8691
		LIFT_ENTRY,
		// Token: 0x040021F4 RID: 8692
		LIFT_EXIT,
		// Token: 0x040021F5 RID: 8693
		LIFT_TOWER,
		// Token: 0x040021F6 RID: 8694
		LIFT_VEHICLE,
		// Token: 0x040021F7 RID: 8695
		LIFT_TURN
	}
}
