using Smod2;
using Smod2.Events;
using Smod2.EventSystem;
using Smod2.EventHandlers;
using Smod2.API;
using System.Collections.Generic;
using scp4aiur;

namespace MVP
{
	internal class EventsHandler : IEventHandlerPlayerDie, IEventHandlerPlayerJoin, IEventHandlerWaitingForPlayers, IEventHandlerRoundStart, IEventHandlerRoundEnd
	{
		private readonly MVP plugin;
		public EventsHandler(MVP plugin) => this.plugin = plugin;

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			MVP.enabled = this.plugin.GetConfigBool("mvp_enable");
			MVP.count_grenades = this.plugin.GetConfigBool("mvp_count_grenades");
			MVP.track_scp_kills = this.plugin.GetConfigBool("mvp_track_scp_kills");
			MVP.track_scps = this.plugin.GetConfigBool("mvp_track_scps");
			MVP.multi_kill = this.plugin.GetConfigBool("mvp_multikill");
			MVP.multi_kill_delay = this.plugin.GetConfigFloat("mvp_multikill_delay");
			MVP.multi_kill_num = this.plugin.GetConfigInt("mvp_multikill_num");
			MVP.multi_text = this.plugin.GetConfigString("mvp_multi_text");
			MVP.multi_cassie = this.plugin.GetConfigBool("mvp_multi_cassie");
		}
		public void OnRoundStart(RoundStartEvent ev)
		{
			Functions.singleton.Refresh();
		}
		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			Functions.singleton.Refresh();
		}
		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if(!MVP.enabled) return;
			Functions.singleton.Refresh();
			if (ev.Killer != ev.Player && ev.Killer.Name != "Server")
			{
				switch (ev.Player.TeamRole.Team)
				{
					case Smod2.API.Team.SCP:
						if (MVP.track_scp_kills)
							MVP.scp_kill_count[ev.Killer.Name]++;
						break;
				}
				switch (ev.Killer.TeamRole.Team)
				{
					case Smod2.API.Team.SCP:
						if (MVP.track_scps)
							MVP.killCounter[ev.Killer.Name]++;
						break;
				}
				MVP.killCounter[ev.Killer.Name]++;
			}
			if (MVP.multi_kill)
				Timing.Run(Functions.singleton.MultiKill(ev.Killer, MVP.multi_kill_delay));
		}
		public void OnRoundEnd(RoundEndEvent ev)
		{
			Functions.singleton.Announce();
			Functions.singleton.ClearStats();
		}
	}
}