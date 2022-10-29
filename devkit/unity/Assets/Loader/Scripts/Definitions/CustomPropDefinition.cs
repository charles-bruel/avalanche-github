using System;
using UnityEngine;

// Token: 0x020006FD RID: 1789
public class CustomPropDefinition : MonoBehaviour
{
	// Token: 0x04001910 RID: 6416
	[Header("Basic Display Info")]
	public string Name;

	// Token: 0x04001911 RID: 6417
	public string ID;

	// Token: 0x04001913 RID: 6419
	public Sprite Thumbnail;

	[Tooltip("Build this prop in a line. Good for fences and stuff.")]
	public bool LinearProp = false;

	[Tooltip("Should this rotate to conform to the terrainl. Good for fences, etc, bad for snowmakers, etc.")]
	public bool LinearTerrainConform;

	public float LinearSpacing;
}
