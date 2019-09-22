using Smod2.Events;
using Smod2.EventHandlers;
using Smod2.API;
using MEC;

namespace MVP
{
	internal class EventsHandler : IEventHandlerPlayerDie, IEventHandlerPlayerJoin, IEventHandlerWaitingForPlayers,
		IEventHandlerRoundStart, IEventHandlerRoundEnd, IEventHandlerThrowGrenade
	{
		private readonly Mvp plugin;
		public EventsHandler(Mvp plugin) => this.plugin = plugin;

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
			if (!plugin.Enabled) return;
			plugin.Functions.Refresh();
			plugin.Debug(ev.Player.Name + ev.Killer.Name);
			if (ev.Killer.SteamId == ev.Player.SteamId || ev.Killer.Name == "Server" || ev.Player.Name == "Sever" || ev.Killer.Name == "" || ev.Player.Name == "") return;
			if (ev.DamageTypeVar == DamageType.POCKET || ev.DamageTypeVar == DamageType.WALL || ev.DamageTypeVar == DamageType.CONTAIN ||
				ev.DamageTypeVar == DamageType.NUKE || ev.DamageTypeVar == DamageType.LURE || ev.DamageTypeVar == DamageType.DECONT || ev.DamageTypeVar == DamageType.FALLDOWN) return;

			if (ev.Player.TeamRole.Team == Smod2.API.Team.SCP && plugin.TrackScpKills)
				plugin.ScpKillCount[ev.Killer.PlayerId]++;
			if (ev.Killer.TeamRole.Team == Smod2.API.Team.SCP && plugin.TrackScps)
				plugin.KillCounter[ev.Killer.PlayerId]++;
			else if (ev.Killer.TeamRole.Team != Smod2.API.Team.SCP)
				plugin.KillCounter[ev.Killer.PlayerId]++;

			if (!plugin.MultiKill) return;
			
			plugin.Multikill[ev.Killer.PlayerId]++;
			Timing.RunCoroutine(plugin.Functions.MultiKill(ev.Killer, plugin.MultiKillDelay));
		}
		
		public void OnRoundEnd(RoundEndEvent ev)
		{
			plugin.Functions.Announce();
			Timing.RunCoroutine(plugin.Functions.ClearStats());
		}

		public void OnThrowGrenade(PlayerThrowGrenadeEvent ev)
		{
			plugin.GrenadeCount[ev.Player.PlayerId]++;
		}
	}
}