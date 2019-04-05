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
		version = "1.1.0",
		SmodMajor = 3,
		SmodMinor = 4,
		SmodRevision = 0
	)]

	public class MVP : Plugin
	{
		public Functions Functions { get; private set; }

		public Dictionary<string, double> killCounter = new Dictionary<string, double>();
		public Dictionary<string, int> multikill = new Dictionary<string, int>();
		public Dictionary<string, bool> multi_track = new Dictionary<string, bool>();
		public Dictionary<string, double> scp_kill_count = new Dictionary<string, double>();

		public Player mvp { get; internal set; } = null;

		public bool enabled { get; internal set; }
		public bool track_scp_kills { get; private set; }
		public bool track_scps { get; private set; }
		public bool multi_kill { get; private set; }
		public bool half_scp_kills { get; private set; }
		public bool multi_cassie { get; private set; }
		public bool count_grenades { get; private set; }

		public float multi_kill_delay { get; private set; }

		public int multi_kill_num { get; private set; }

		public string multi_text { get; private set; }

		public override void OnDisable()
		{
			this.Info(this.Details.name + " v." + this.Details.version + " has been disabled.");
		}

		public override void OnEnable()
		{
			this.Info(this.Details.name + " v." + this.Details.version + " has been enabled.");
		}

		public override void Register()
		{
			this.AddConfig(new ConfigSetting("mvp_enable", true, true, "If MVP should be enabled or not."));
			this.AddConfig(new ConfigSetting("mvp_track_scp_kills", true, true, "If SCP kills should count towards the MVP."));
			this.AddConfig(new ConfigSetting("mvp_track_scps", true, true, "If SCP's should be allowed to be MVP's."));
			this.AddConfig(new ConfigSetting("mvp_count_grenades", true, true, "If it shoudl count how many grenades the MVP threw."));
			this.AddConfig(new ConfigSetting("mvp_multikill", true, true, "If there should be an announcement for multikills."));
			this.AddConfig(new ConfigSetting("mvp_multikill_delay", 3f, true, "The amount of time kills must be registered in to qualify for a multikill."));
			this.AddConfig(new ConfigSetting("mvp_multikill_num", 3, true, "The number of kills required to get a Multikill announcement."));
			this.AddConfig(new ConfigSetting("mvp_multi_text", "is on fire!", true, "The text displayed for Multi-Kills."));
			this.AddConfig(new ConfigSetting("mvp_multi_cassie", true, true, "If Cassie should make an announcement for SCP multi-kills."));
			this.AddConfig(new ConfigSetting("mvp_scp_half_kill", false, true, "This makes SCP kills count as half kills."));

			this.AddEventHandlers(new EventsHandler(this), Priority.Normal);

			this.AddCommands(new string[] { "mvp" }, new MVPCommand(this));

			Timing.Init(this);

			Functions = new Functions(this);
		}

		public void ReloadConfig()
		{
			enabled = GetConfigBool("mvp_enable");
			count_grenades = GetConfigBool("mvp_count_grenades");
			track_scp_kills = GetConfigBool("mvp_track_scp_kills");
			track_scps = GetConfigBool("mvp_track_scps");
			multi_kill = GetConfigBool("mvp_multikill");
			multi_kill_delay = GetConfigFloat("mvp_multikill_delay");
			multi_kill_num = GetConfigInt("mvp_multikill_num");
			multi_text = GetConfigString("mvp_multi_text");
			multi_cassie = GetConfigBool("mvp_multi_cassie");
		}
	}
}