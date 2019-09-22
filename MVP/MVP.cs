using System.Collections.Generic;
using Smod2;
using Smod2.Attributes;
using Smod2.Config;

namespace MVP
{
	[PluginDetails(
		author = "Joker119",
		name = "Most Valuable player",
		description = "Announces who the best player in the round was.",
		id = "joker.mvp",
		version = "1.1.8",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 0
	)]

	public class Mvp : Plugin
	{
		public Functions Functions { get; private set; }

		public Dictionary<int, double> KillCounter = new Dictionary<int, double>();
		public Dictionary<int, int> Multikill = new Dictionary<int, int>();
		public Dictionary<int, bool> MultiTrack = new Dictionary<int, bool>();
		public Dictionary<int, double> ScpKillCount = new Dictionary<int, double>();
		public Dictionary<int, int> GrenadeCount = new Dictionary<int, int>();

		public bool Enabled { get; internal set; }
		public bool TrackScpKills { get; private set; }
		public bool TrackScps { get; private set; }
		public bool MultiKill { get; private set; }
		public bool HalfScpKills { get; private set; }
		public bool MultiCassie { get; private set; }
		public bool CountGrenades { get; private set; }

		public float MultiKillDelay { get; private set; }

		public int MultiKillNum { get; private set; }

		public string MultiText { get; private set; }

		public override void OnDisable()
		{
			Info(Details.name + " v." + Details.version + " has been disabled.");
		}

		public override void OnEnable()
		{
			Info(Details.name + " v." + Details.version + " has been enabled.");
		}

		public override void Register()
		{
			AddConfig(new ConfigSetting("mvp_enable", true, true, "If MVP should be enabled or not."));
			AddConfig(new ConfigSetting("mvp_track_scp_kills", true, true, "If SCP kills should count towards the MVP."));
			AddConfig(new ConfigSetting("mvp_track_scps", true, true, "If SCP's should be allowed to be MVP's."));
			AddConfig(new ConfigSetting("mvp_count_grenades", true, true, "If it shoudl count how many grenades the MVP threw."));
			AddConfig(new ConfigSetting("mvp_multikill", true, true, "If there should be an announcement for multikills."));
			AddConfig(new ConfigSetting("mvp_multikill_delay", 3f, true, "The amount of time kills must be registered in to qualify for a multikill."));
			AddConfig(new ConfigSetting("mvp_multikill_num", 3, true, "The number of kills required to get a Multikill announcement."));
			AddConfig(new ConfigSetting("mvp_multi_text", "is on fire!", true, "The text displayed for Multi-Kills."));
			AddConfig(new ConfigSetting("mvp_multi_cassie", true, true, "If Cassie should make an announcement for SCP multi-kills."));
			AddConfig(new ConfigSetting("mvp_scp_half_kill", false, true, "This makes SCP kills count as half kills."));

			AddEventHandlers(new EventsHandler(this));

			AddCommands(new string[] { "mvp" }, new MvpCommand(this));

			Functions = new Functions(this);
		}

		public void ReloadConfig()
		{
			Enabled = GetConfigBool("mvp_enable");
			CountGrenades = GetConfigBool("mvp_count_grenades");
			TrackScpKills = GetConfigBool("mvp_track_scp_kills");
			TrackScps = GetConfigBool("mvp_track_scps");
			MultiKill = GetConfigBool("mvp_multikill");
			MultiKillDelay = GetConfigFloat("mvp_multikill_delay");
			MultiKillNum = GetConfigInt("mvp_multikill_num");
			MultiText = GetConfigString("mvp_multi_text");
			MultiCassie = GetConfigBool("mvp_multi_cassie");
			HalfScpKills = GetConfigBool("mvp_scp_half_kills");
		}
	}
}