using System;
using UnityEngine;

// Token: 0x0200093B RID: 2363
public class CustomLiftTurnSectionDefinition : MonoBehaviour
{
	// Token: 0x04002215 RID: 8725
	[Header("Transforms Linking")]
	public Transform RightCablePoint;

	// Token: 0x04002216 RID: 8726
	public Transform LeftCablePoint;

	// Token: 0x04002217 RID: 8727
	[Tooltip("What is passed to the wheel fetcher, so for example, auto generated wheels point correctly")]
	public Transform CableAimingPoint;

	// Token: 0x04002218 RID: 8728
	[Header("Custom Scripts")]
	public CustomScriptDefinition CablePathScript;

	// Token: 0x04002219 RID: 8729
	public float MaxAngle;
}
