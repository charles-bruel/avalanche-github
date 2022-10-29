using System;
using UnityEngine;

// Token: 0x02000916 RID: 2326
public static class CustomExtensions
{
	// Token: 0x06002CE6 RID: 11494 RVA: 0x0001F1AF File Offset: 0x0001D3AF
	public static Vector2 ToHorizontal(this Vector4 vector4)
	{
		return new Vector2(vector4.x, vector4.z);
	}

	// Token: 0x06002CE7 RID: 11495 RVA: 0x0001F1C2 File Offset: 0x0001D3C2
	public static Vector3 Flatten(this Vector4 vector4)
	{
		return new Vector3(vector4.x, vector4.y, vector4.z);
	}
}
