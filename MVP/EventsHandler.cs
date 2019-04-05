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
			plugin.ReloadConfig();
		}
		public void OnRoundStart(RoundStartEvent ev)
		{
			plugin.Functions.Refresh();
		}
		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			plugin.Functions.Refresh();
		}
		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (!plugin.enabled) return;
			plugin.Functions.Refresh();
			plugin.Debug(ev.Player.Name + ev.Killer.Name);
			if (ev.Killer.SteamId == ev.Player.SteamId || ev.Killer.Name == "Server" || ev.Player.Name == "Sever" || ev.Killer.Name == "" || ev.Player.Name == "") return;
			if (ev.DamageTypeVar == DamageType.POCKET || ev.DamageTypeVar == DamageType.WALL || ev.DamageTypeVar == DamageType.CONTAIN ||
				ev.DamageTypeVar == DamageType.NUKE || ev.DamageTypeVar == DamageType.LURE || ev.DamageTypeVar == DamageType.DECONT || ev.DamageTypeVar == DamageType.FALLDOWN) return;

			if (ev.Player.TeamRole.Team == Smod2.API.Team.SCP && plugin.track_scp_kills)
				plugin.scp_kill_count[ev.Killer.Name]++;
			if (ev.Killer.TeamRole.Team == Smod2.API.Team.SCP && plugin.track_scps)
				plugin.killCounter[ev.Killer.Name]++;
			else if (ev.Killer.TeamRole.Team != Smod2.API.Team.SCP)
				plugin.killCounter[ev.Killer.Name]++;

			if (plugin.multi_kill)
			{
				plugin.multikill[ev.Player.Name]++;
				Timing.Run(plugin.Functions.MultiKill(ev.Killer, plugin.multi_kill_delay));
			}
		}
		public void OnRoundEnd(RoundEndEvent ev)
		{
			plugin.Functions.Announce();
			Timing.Run(plugin.Functions.ClearStats());
		}
	}
}