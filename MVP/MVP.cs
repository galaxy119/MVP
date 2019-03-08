using System.Collections.Generic;
using Smod2;
using Smod2.Attributes;
using Smod2.API;
using Smod2.Config;
using Smod2.Events;
using scp4aiur;

namespace MVP
{
	[PluginDetails(
		author = "Joker119",
		name = "Most Valuable player",
		description = "Announces who the best player in the round was.",
		id = "joker.mvp",
		version = "1.0.0",
		SmodMajor = 3,
		SmodMinor = 3,
		SmodRevision = 0
	)]

	public class MVP : Plugin
	{
		internal static MVP singleton;
		public static Dictionary<string, int> killCounter = new Dictionary<string, int>();
		public static Dictionary<string, int> multikill = new Dictionary<string, int>();
		public static Dictionary<string, bool> multi_track = new Dictionary<string, bool>();
		public static Player mvp;
		public static bool
			enabled,
			track_scp_kills,
			track_scps,
			multi_kill,
			count_grenades;
		public static float
			multi_kill_delay;
		public static int
			multi_kill_num;
		public static string
			multi_text;
		public override void OnDisable()
		{
			this.Info(this.Details.name + " v." + this.Details.version + " has been disabled.");
		}
		public override void OnEnable()
		{
			singleton = this;
			this.Info(this.Details.name + " v." + this.Details.version + " has been enabled.");
		}
		public override void Register()
		{
			this.AddEventHandlers(new EventsHandler(this), Priority.Normal);
			this.AddConfig(new ConfigSetting("mvp_enable", true, SettingType.BOOL, true, "If MVP should be enabled or not."));
			this.AddConfig(new ConfigSetting("mvp_track_scp_kills", true, SettingType.BOOL, true, "If SCP kills should count towards the MVP."));
			this.AddConfig(new ConfigSetting("mvp_track_scps", true, SettingType.BOOL, true, "If SCP's should be allowed to be MVP's."));
			this.AddConfig(new ConfigSetting("mvp_count_grenades", true, SettingType.BOOL, true, "If it shoudl count how many grenades the MVP threw."));
			this.AddConfig(new ConfigSetting("mvp_multikill", true, SettingType.BOOL, true, "If there should be an announcement for multikills."));
			this.AddConfig(new ConfigSetting("mvp_multikill_delay", 3f, SettingType.FLOAT, true, "The amount of time kills must be registered in to qualify for a multikill."));
			this.AddConfig(new ConfigSetting("mvp_multikill_num", 3, SettingType.NUMERIC, true, "The number of kills required to get a Multikill announcement."));
			this.AddConfig(new ConfigSetting("mvp_multi_text", "is on fire!", SettingType.STRING, true, "The text displayed for Multi-Kills."));
			this.AddCommands(new string[] { "mvp" }, new MVPCommand());
			Timing.Init(this);
			new Functions(this);
		}
	}
}