using System;
using UnityEngine;

// Token: 0x02000929 RID: 2345
public class CustomLiftTowerDefinition : MonoBehaviour
{
	// Token: 0x040021CF RID: 8655
	[Header("Transforms Linking")]
	public Transform RightCablePoint;

	// Token: 0x040021D0 RID: 8656
	public Transform LeftCablePoint;

	// Token: 0x040021D1 RID: 8657
	[Header("Custom Scripts")]
	public CustomScriptDefinition CablePathScript;

	// Token: 0x040021D2 RID: 8658
	[Tooltip("What is passed to the wheel fetcher, so for example, auto generated wheels point correctly")]
	public Transform CableAimingPoint;

	// Token: 0x040021D3 RID: 8659
	public CustomScriptDefinition TowerPlacementScript;

	// Token: 0x040021D4 RID: 8660
	public GameObject Padding;
}
