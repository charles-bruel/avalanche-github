using System;
using UnityEngine;

// Token: 0x02000928 RID: 2344
public class CustomLiftExitStationDefinition : MonoBehaviour
{
	// Token: 0x040021C7 RID: 8647
	public Transform RightCablePoint;

	// Token: 0x040021C8 RID: 8648
	public Transform LeftCablePoint;

	// Token: 0x040021C9 RID: 8649
	public Transform NavNodeTransform;

	// Token: 0x040021CA RID: 8650
	public CustomScriptDefinition CablePathScript;

	// Token: 0x040021CB RID: 8651
	public Transform SFXEmitter;

	// Token: 0x040021CC RID: 8652
	public Transform CableAimingPoint;

	// Token: 0x040021CD RID: 8653
	public AreaDefinition navArea;

	// Token: 0x040021CE RID: 8654
	public AreaDefinition[] collisions;
}
