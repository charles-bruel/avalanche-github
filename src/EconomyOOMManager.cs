using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000947 RID: 2375
public class EconomyOOMManager : MonoBehaviour
{
	// Token: 0x06002D8B RID: 11659 RVA: 0x000B78F4 File Offset: 0x000B5AF4
	public void OnMainMenu()
	{
		if (this.MainMenuBound)
		{
			return;
		}
		Core.SaveManager.CurrentLoadingData = null;
		Game.AudioQueriesService.InGameMix.Stop(AudioInGameMix.Id.IGM_SoloUI);
		Game.AudioQueriesService.MusicManager.InGameMusic.SetAutoPlay(false);
		Game.AudioQueriesService.MusicManager.InGameMusic.Stop();
		SceneLoaderComponent.SceneToLoad = "MainMenu";
		this.MainMenuBound = true;
		SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Additive);
	}

	// Token: 0x06002D8D RID: 11661 RVA: 0x0001F624 File Offset: 0x0001D824
	public static void Destroy()
	{
		UnityEngine.Object.DestroyImmediate(EconomyOOMManager.instance);
		EconomyOOMManager.exists = false;
	}

	// Token: 0x06002D8E RID: 11662 RVA: 0x000B7970 File Offset: 0x000B5B70
	public static void Create()
	{
		if (EconomyOOMManager.exists)
		{
			return;
		}
		EconomyOOMManager.exists = true;
		if (EconomyOOMManager.economyOOMManager == null)
		{
			EconomyOOMManager.economyOOMManager = (GameObject)BundleHelper.LoadIfNotLoaded("ModData/Resource/ui").LoadAsset("EconomyOOM");
		}
		if (EconomyOOMManager.economyOOMManager != null)
		{
			EconomyOOMManager.instance = UnityEngine.Object.Instantiate<GameObject>(EconomyOOMManager.economyOOMManager);
		}
	}

	// Token: 0x06002D90 RID: 11664 RVA: 0x0001F636 File Offset: 0x0001D836
	public void Update()
	{
		if (Game.SimulationTimeSeconds.Active)
		{
			Game.SimulationTimeSeconds.Pause();
			Game.GameTimeHours.Pause();
			Game.GameTimeUnscaled.Pause();
		}
		if (Game.EconomyModule.Balance > 0)
		{
			EconomyOOMManager.Destroy();
		}
	}

	// Token: 0x06002D91 RID: 11665 RVA: 0x0001F674 File Offset: 0x0001D874
	public void OnLoan()
	{
		EconomyLoanUIManager.Create();
		EconomyOOMManager.Destroy();
	}

	// Token: 0x04002243 RID: 8771
	private static GameObject economyOOMManager;

	// Token: 0x04002244 RID: 8772
	private static bool exists;

	// Token: 0x04002245 RID: 8773
	private bool MainMenuBound;

	// Token: 0x04002246 RID: 8774
	private static GameObject instance;
}
