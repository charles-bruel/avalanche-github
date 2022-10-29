using System;
using TMPro;
using UnityEngine;

// Token: 0x02000944 RID: 2372
public class EconomyUIManager : MonoBehaviour
{
	// Token: 0x06002D86 RID: 11654 RVA: 0x0001F61D File Offset: 0x0001D81D
	public void OnLoan()
	{
		EconomyLoanUIManager.Toggle();
	}

	// Token: 0x0400223E RID: 8766
	public TextMeshProUGUI revenue;

	// Token: 0x0400223F RID: 8767
	public TextMeshProUGUI capital;

	// Token: 0x04002240 RID: 8768
	public TextMeshProUGUI expenses;

	// Token: 0x04002241 RID: 8769
	public TextMeshProUGUI total;

	// Token: 0x04002242 RID: 8770
	public TextMeshProUGUI balance;
}
