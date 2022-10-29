using System;
using TMPro;
using UnityEngine;

// Token: 0x02000948 RID: 2376
public class EconomyLoanUIManager : MonoBehaviour
{
	// Token: 0x06002D92 RID: 11666 RVA: 0x000B79D4 File Offset: 0x000B5BD4
	public void OnBorrow()
	{
		int @int = ConfigHelper.GetInt("ModData\\economy.cfg", "loan_step");
		Game.EconomyModule.LoanAmount += @int;
		Game.EconomyModule.Capital -= @int;
		this.UpdateText();
	}

	// Token: 0x06002D93 RID: 11667 RVA: 0x000B7A1C File Offset: 0x000B5C1C
	public void OnRepay()
	{
		int @int = ConfigHelper.GetInt("ModData\\economy.cfg", "loan_step");
		if (Game.EconomyModule.Balance > @int)
		{
			Game.EconomyModule.LoanAmount -= @int;
			Game.EconomyModule.Capital += @int;
			this.UpdateText();
		}
	}

	// Token: 0x06002D95 RID: 11669 RVA: 0x0001F680 File Offset: 0x0001D880
	public static void Destroy()
	{
		UnityEngine.Object.Destroy(EconomyLoanUIManager.instance);
		EconomyLoanUIManager.exists = false;
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x000B7A70 File Offset: 0x000B5C70
	public static void Create()
	{
		if (EconomyLoanUIManager.exists)
		{
			return;
		}
		EconomyLoanUIManager.exists = true;
		if (EconomyLoanUIManager.economyLoanUIManager == null)
		{
			EconomyLoanUIManager.economyLoanUIManager = (GameObject)BundleHelper.LoadIfNotLoaded("ModData/Resource/ui").LoadAsset("EconomyLoan");
		}
		if (EconomyLoanUIManager.economyLoanUIManager != null)
		{
			EconomyLoanUIManager.instance = UnityEngine.Object.Instantiate<GameObject>(EconomyLoanUIManager.economyLoanUIManager);
		}
	}

	// Token: 0x06002D97 RID: 11671 RVA: 0x0001F692 File Offset: 0x0001D892
	public static void Toggle()
	{
		if (EconomyLoanUIManager.exists)
		{
			EconomyLoanUIManager.Destroy();
			return;
		}
		EconomyLoanUIManager.Create();
	}

	// Token: 0x06002D98 RID: 11672 RVA: 0x000B7AD4 File Offset: 0x000B5CD4
	private void Start()
	{
		int @int = ConfigHelper.GetInt("ModData\\economy.cfg", "loan_step");
		string str = string.Format("{0:C2}", (double)@int / 100.0);
		this.repay.text = "Repay " + str;
		this.borrow.text = "Borrow " + str;
		this.UpdateText();
	}

	// Token: 0x06002D99 RID: 11673 RVA: 0x000B7B40 File Offset: 0x000B5D40
	private void UpdateText()
	{
		this.balance.text = string.Format("{0:C2}", (double)Game.EconomyModule.LoanAmount / 100.0);
		this.interest.text = string.Format("{0:C2}", (double)Game.EconomyModule.LoanAmount * (double)ConfigHelper.GetFloat("ModData\\economy.cfg", "daily_interest") / 100.0);
	}

	// Token: 0x06002D9A RID: 11674 RVA: 0x0001F6A6 File Offset: 0x0001D8A6
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			EconomyLoanUIManager.Destroy();
		}
	}

	// Token: 0x04002247 RID: 8775
	public TextMeshProUGUI balance;

	// Token: 0x04002248 RID: 8776
	public TextMeshProUGUI interest;

	// Token: 0x04002249 RID: 8777
	private static GameObject economyLoanUIManager;

	// Token: 0x0400224A RID: 8778
	private static bool exists;

	// Token: 0x0400224B RID: 8779
	private static GameObject instance;

	// Token: 0x0400224C RID: 8780
	public TextMeshProUGUI borrow;

	// Token: 0x0400224D RID: 8781
	public TextMeshProUGUI repay;
}
