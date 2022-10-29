using System;

// Token: 0x02000922 RID: 2338
public class ExistingAssets
{
	// Token: 0x04002179 RID: 8569
	public static int CHAIRLIFT_MASK = 16;

	// Token: 0x0400217A RID: 8570
	public static int DETACHABLE_MASK = 32;

	// Token: 0x0400217B RID: 8571
	public static int GONDOLA_MASK = 64;

	// Token: 0x0400217C RID: 8572
	public static int TOW_MASK = 128;

	// Token: 0x0400217D RID: 8573
	public static int TOW_A_MASK = 256;

	// Token: 0x0400217E RID: 8574
	public static int LIFT_CAPACITY_MASK = 15;

	// Token: 0x02000923 RID: 2339
	public enum Lifts
	{
		// Token: 0x04002180 RID: 8576
		CHAIRLIFT_2 = 18,
		// Token: 0x04002181 RID: 8577
		CHAIRLIFT_4 = 20,
		// Token: 0x04002182 RID: 8578
		DETACHABLE_6 = 38,
		// Token: 0x04002183 RID: 8579
		GONDOLA_6 = 70,
		// Token: 0x04002184 RID: 8580
		GONDOLA_8 = 72,
		// Token: 0x04002185 RID: 8581
		GONDOLA_10 = 74,
		// Token: 0x04002186 RID: 8582
		GONDOLA_12 = 76,
		// Token: 0x04002187 RID: 8583
		TOW_1 = 129,
		// Token: 0x04002188 RID: 8584
		TOW_1A = 257,
		// Token: 0x04002189 RID: 8585
		TOW_2 = 130,
		// Token: 0x0400218A RID: 8586
		DETACHABLE_4 = 36
	}
}
