using System;
using UnityEngine;

// Token: 0x0200092C RID: 2348
public class CustomPropDefinition : MonoBehaviour
{
	// Token: 0x040021E3 RID: 8675
	[Header("Basic Display Info")]
	public string Name;

	// Token: 0x040021E4 RID: 8676
	public string ID;

	// Token: 0x040021E5 RID: 8677
	public Sprite Thumbnail;

	// Token: 0x040021E6 RID: 8678
	public bool LinearProp;

	// Token: 0x040021E7 RID: 8679
	public bool LinearTerrainConform;

	// Token: 0x040021E8 RID: 8680
	public float LinearSpacing = 5f;
}
