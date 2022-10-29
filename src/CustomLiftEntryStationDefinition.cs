using System;
using UnityEngine;

// Token: 0x02000927 RID: 2343
public class CustomLiftEntryStationDefinition : MonoBehaviour
{
	// Token: 0x040021BC RID: 8636
	public Transform RightCablePoint;

	// Token: 0x040021BD RID: 8637
	public Transform LeftCablePoint;

	// Token: 0x040021BE RID: 8638
	public Transform ClosedItems;

	// Token: 0x040021BF RID: 8639
	public Transform QueuePosition;

	// Token: 0x040021C0 RID: 8640
	public Transform SFXEmitter1;

	// Token: 0x040021C1 RID: 8641
	public Transform SFXEmitter2;

	// Token: 0x040021C2 RID: 8642
	public Transform NavNodeTransform;

	// Token: 0x040021C3 RID: 8643
	public CustomScriptDefinition CablePathScript;

	// Token: 0x040021C4 RID: 8644
	public Transform CableAimingPoint;

	// Token: 0x040021C5 RID: 8645
	public AreaDefinition[] collisions;

	// Token: 0x040021C6 RID: 8646
	public AreaDefinition navArea;
}
