using System;
using System.Linq;
using LBF.Time;
using UnityEngine;

// Token: 0x02000942 RID: 2370
public class EconomyModule
{
	// Token: 0x1700074C RID: 1868
	// (get) Token: 0x06002D75 RID: 11637 RVA: 0x0001F5A4 File Offset: 0x0001D7A4
	public int Balance
	{
		get
		{
			return this.BalanceValue;
		}
	}

	// Token: 0x1700074D RID: 1869
	// (get) Token: 0x06002D76 RID: 11638 RVA: 0x0001F5AC File Offset: 0x0001D7AC
	// (set) Token: 0x06002D77 RID: 11639 RVA: 0x000B742C File Offset: 0x000B562C
	public int Revenue
	{
		get
		{
			return this.RunningRevenueValue;
		}
		set
		{
			int num = value - this.RunningRevenueValue;
			this.RunningRevenueValue = value;
			this.BalanceValue += num;
		}
	}

	// Token: 0x1700074E RID: 1870
	// (get) Token: 0x06002D78 RID: 11640 RVA: 0x0001F5B4 File Offset: 0x0001D7B4
	// (set) Token: 0x06002D79 RID: 11641 RVA: 0x000B7458 File Offset: 0x000B5658
	public int Capital
	{
		get
		{
			return this.RunningCapitalValue;
		}
		set
		{
			int num = value - this.RunningCapitalValue;
			this.RunningCapitalValue = value;
			this.BalanceValue -= num;
		}
	}

	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x06002D7A RID: 11642 RVA: 0x0001F5BC File Offset: 0x0001D7BC
	// (set) Token: 0x06002D7B RID: 11643 RVA: 0x000B7484 File Offset: 0x000B5684
	public int Expenses
	{
		get
		{
			return this.RunningExpensesValue;
		}
		set
		{
			int num = value - this.RunningExpensesValue;
			this.RunningExpensesValue = value;
			this.BalanceValue -= num;
		}
	}

	// Token: 0x06002D7C RID: 11644 RVA: 0x000B74B0 File Offset: 0x000B56B0
	public void Update(Clock clock)
	{
		if (!ModSettings.economyEnabled)
		{
			return;
		}
		if (GameDateMapping.GetDate(clock.Time).Hour != this.LastHour)
		{
			this.EndOfHour();
			this.LastHour = GameDateMapping.GetDate(clock.Time).Hour;
		}
		this.economyUI.balance.text = string.Format("{0:C2}", (double)this.BalanceValue / 100.0);
		this.economyUI.revenue.text = string.Format("{0:C2}", (double)this.RunningRevenueValue / 100.0);
		this.economyUI.capital.text = string.Format("{0:C2}", -(double)this.RunningCapitalValue / 100.0);
		this.economyUI.expenses.text = string.Format("{0:C2}", -(double)this.RunningExpensesValue / 100.0);
		int num = this.RunningRevenueValue - this.RunningCapitalValue - this.RunningExpensesValue;
		this.economyUI.total.text = string.Format("{0:C2}", (double)num / 100.0);
	}

	// Token: 0x06002D7D RID: 11645 RVA: 0x0001F5C4 File Offset: 0x0001D7C4
	public void OnVisitorSpawn()
	{
		if (!ModSettings.economyEnabled)
		{
			return;
		}
		this.Revenue += ConfigHelper.GetInt("ModData\\economy.cfg", "ticket_price");
	}

	// Token: 0x06002D7E RID: 11646 RVA: 0x0001F5EA File Offset: 0x0001D7EA
	public void SetStartupSettings(int balance, int hour)
	{
		this.BalanceValue = balance;
		this.LastHour = hour;
	}

	// Token: 0x06002D7F RID: 11647 RVA: 0x000B75FC File Offset: 0x000B57FC
	private void EndOfHour()
	{
		this.RunningRevenueValue = 0;
		this.RunningCapitalValue = 0;
		this.RunningExpensesValue = 0;
		this.Expenses += (int)(ConfigHelper.GetFloat("ModData\\economy.cfg", "daily_interest") * (float)this.LoanAmount);
		int num = Game.WorkService.Workers.Count((IWorker w) => w.Slot != null);
		this.Expenses += ConfigHelper.GetInt("ModData\\economy.cfg", "worker_cost") * num;
		foreach (BuildingController buildingController in Game.BuildingService.Buildings)
		{
			string id = buildingController.Descriptor.ID;
			Guid guid;
			if (!Guid.TryParse(id, out guid) && !id.Contains("Slope"))
			{
				this.Expenses += ConfigHelper.GetInt("ModData\\economy.cfg", "building_running_cost_" + id);
			}
			if (buildingController.Script is LiftScript)
			{
				(buildingController.Script as LiftScript).Age++;
			}
		}
		if (this.BalanceValue < 0)
		{
			EconomyOOMManager.Create();
		}
	}

	// Token: 0x06002D80 RID: 11648 RVA: 0x0001F5FA File Offset: 0x0001D7FA
	public void SetStartupSettings(int balance, int loanAmount, int hour)
	{
		this.BalanceValue = balance;
		this.LastHour = hour;
		this.LoanAmount = loanAmount;
	}

	// Token: 0x06002D81 RID: 11649 RVA: 0x000B774C File Offset: 0x000B594C
	public void Initialize()
	{
		if (ModSettings.economyEnabled)
		{
			this.BalanceValue = ConfigHelper.GetInt("ModData\\economy.cfg", "starting_money");
			if (EconomyModule.uiPrefab == null)
			{
				EconomyModule.uiPrefab = (GameObject)BundleHelper.LoadIfNotLoaded("ModData/Resource/ui").LoadAsset("EconomyUI");
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EconomyModule.uiPrefab);
			this.economyUI = gameObject.GetComponent<EconomyUIManager>();
		}
	}

	// Token: 0x04002234 RID: 8756
	public EconomyUIManager economyUI;

	// Token: 0x04002235 RID: 8757
	private static GameObject uiPrefab;

	// Token: 0x04002236 RID: 8758
	private int BalanceValue;

	// Token: 0x04002237 RID: 8759
	private int RunningRevenueValue;

	// Token: 0x04002238 RID: 8760
	private int RunningCapitalValue;

	// Token: 0x04002239 RID: 8761
	private int RunningExpensesValue;

	// Token: 0x0400223A RID: 8762
	private int LastHour;

	// Token: 0x0400223B RID: 8763
	public int LoanAmount;
}
