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
			foreach (Player player in plugin.Server.GetPlayers())
			{
				MVP.killCounter[player.Name] = 0;
				MVP.multi_track[player.Name] = false;
				MVP.multikill[player.Name] = 0;
				MVP.scp_kill_count[player.Name] = 0;
			}
		}
		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			MVP.killCounter[ev.Player.Name] = 0;
			MVP.multi_track[ev.Player.Name] = false;
			MVP.multikill[ev.Player.Name] = 0;
			MVP.scp_kill_count[ev.Player.Name] = 0;
		}
		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (MVP.enabled && ev.Killer != null)
			{
				if (MVP.track_scps && ev.Killer.TeamRole.Team == Smod2.API.Team.SCP)
				{
					MVP.killCounter[ev.Killer.Name]++;
					MVP.multikill[ev.Killer.Name]++;
				}
				else if (MVP.track_scp_kills && ev.Player.TeamRole.Team == Smod2.API.Team.SCP)
				{
					if (MVP.half_scp_kills)
					{
						MVP.scp_kill_count[ev.Killer.Name]++;
					}
					else
					{
						MVP.killCounter[ev.Killer.Name]++;
					}
					MVP.multikill[ev.Killer.Name]++;
				}
				else if (ev.Player.TeamRole.Team != Smod2.API.Team.SCP && ev.Killer.TeamRole.Team != Smod2.API.Team.SCP)
				{
					MVP.killCounter[ev.Killer.Name]++;
					MVP.multikill[ev.Killer.Name]++;
				}
				if (MVP.multi_kill)
				{
					Timing.Run(Functions.singleton.MultiKill(ev.Killer, MVP.multi_kill_delay));
				}
			}
		}
		public void OnRoundEnd(RoundEndEvent ev)
		{
			Functions.singleton.Announce();
		}
	}
}