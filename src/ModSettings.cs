using System;

// Token: 0x02000941 RID: 2369
public class ModSettings
{
	// Token: 0x04002231 RID: 8753
	public static bool economyEnabled = ConfigHelper.GetInt("ModData\\basic.cfg", "enable_economy") != 0;

	// Token: 0x04002232 RID: 8754
	public static bool sandboxEnabled = ConfigHelper.GetInt("ModData\\basic.cfg", "enable_sandbox") != 0;

	// Token: 0x04002233 RID: 8755
	public static bool autoBuildEnabled = ConfigHelper.GetInt("ModData\\basic.cfg", "enable_auto_build") != 0;
}
