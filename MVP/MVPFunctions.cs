using Smod2.API;
using System.Collections.Generic;
using System.Linq;
using MEC;

namespace MVP
{
	public class Functions
	{
		private readonly Mvp plugin;
		public Functions(Mvp plugin) => this.plugin = plugin;

		public IEnumerator<float> MultiKill(Player player, float delay)
		{
			if (plugin.MultiTrack[player.PlayerId]) yield break;
			
			plugin.MultiTrack[player.PlayerId] = true;
			yield return Timing.WaitForSeconds(delay);
			
			plugin.Info(plugin.Multikill[player.PlayerId] + " " + plugin.MultiKillNum);
			
			if (plugin.Multikill[player.PlayerId] >= plugin.MultiKillNum)
			{
				plugin.Server.Map.ClearBroadcasts();
				plugin.Server.Map.Broadcast(10, player.Name + " " + plugin.MultiText, false);
				if (player.TeamRole.Team == Smod2.API.Team.SCP)
					switch (player.TeamRole.Role)
					{
						case Role.SCP_049:
						{
							PlayerManager.localPlayer.GetComponent<MTFRespawn>().CallRpcPlayCustomAnnouncement("SCP 0 4 9 is on fire", false);
							break;
						}
						case Role.SCP_096:
						{
							PlayerManager.localPlayer.GetComponent<MTFRespawn>().CallRpcPlayCustomAnnouncement("SCP 0 9 6 is on fire", false);
							break;
						}
						case Role.SCP_106:
						{
							PlayerManager.localPlayer.GetComponent<MTFRespawn>().CallRpcPlayCustomAnnouncement("SCP 1 0 6 is on fire", false);
							break;
						}
						case Role.SCP_173:
						{
							PlayerManager.localPlayer.GetComponent<MTFRespawn>().CallRpcPlayCustomAnnouncement("SCP 1 7 3 is on fire", false);
							break;
						}
						case Role.SCP_939_53:
						case Role.SCP_939_89:
						{
							PlayerManager.localPlayer.GetComponent<MTFRespawn>().CallRpcPlayCustomAnnouncement("SCP 9 3 9 is on fire", false);
							break;
						}
					}
			}
			
			plugin.MultiTrack[player.PlayerId] = false;
			plugin.Multikill[player.PlayerId] = 0;
		}
		
		public void Announce()
		{
			if (!plugin.Enabled) return;
			
			KeyValuePair<int, double> max = new KeyValuePair<int, double>();
			foreach (KeyValuePair<int, double> kvp in plugin.ScpKillCount)
				if (kvp.Value > plugin.KillCounter[kvp.Key]) plugin.KillCounter[kvp.Key] += plugin.ScpKillCount[kvp.Key];

			foreach (KeyValuePair<int, double> kvp in plugin.KillCounter)
			{
				if (kvp.Value > plugin.ScpKillCount[kvp.Key] && plugin.HalfScpKills) plugin.KillCounter[kvp.Key] += plugin.ScpKillCount[kvp.Key] / 2;
				
				if (kvp.Value > max.Value)
					max = kvp;
			}

			Player mvp = null;

			foreach (Player player in plugin.Server.GetPlayers().Where(ply => ply.PlayerId == max.Key)) 
				mvp = player;

			if (mvp == null) return;

			if (plugin.HalfScpKills)
			{
				if (plugin.CountGrenades)
					plugin.Server.Map.Broadcast(25,
						"MVP: " + mvp.Name + " killed the most players! \n" + "They threw " +
						plugin.GrenadeCount[mvp.PlayerId] + " grenades.", false);
				else
					plugin.Server.Map.Broadcast(25, "MVP: " + mvp.Name + " killed the most players!", false);
			}
			else
			{
				if (plugin.CountGrenades)
					plugin.Server.Map.Broadcast(25,
						"MVP: " + mvp.Name + " killed the most players, with " + max.Value + " total kills! \n" +
						"They threw " + plugin.GrenadeCount[mvp.PlayerId] + " grenades.", false);
				else
					plugin.Server.Map.Broadcast(25,
						"MVP: " + mvp.Name + " killed the most players, with " + max.Value + " total kills!", false);
			}
		}
		
		public IEnumerator<float> ClearStats()
		{
			yield return Timing.WaitForSeconds(5);
			
			plugin.KillCounter.Clear();
			plugin.ScpKillCount.Clear();
			plugin.Multikill.Clear();
			plugin.MultiTrack.Clear();
			plugin.GrenadeCount.Clear();
		}
		
		public void Enable()
		{
			plugin.Enabled = true;
		}
		
		public void Refresh()
		{
			plugin.Debug("Refreshing player list..");
			foreach (Player player in plugin.Server.GetPlayers())
			{
				if (!plugin.KillCounter.ContainsKey(player.PlayerId)) 
					plugin.KillCounter.Add(player.PlayerId, 0);
				if (!plugin.MultiTrack.ContainsKey(player.PlayerId)) 
					plugin.MultiTrack.Add(player.PlayerId, false);
				if (!plugin.Multikill.ContainsKey(player.PlayerId)) 
					plugin.Multikill.Add(player.PlayerId, 0);
				if (!plugin.ScpKillCount.ContainsKey(player.PlayerId)) 
					plugin.ScpKillCount.Add(player.PlayerId, 0);
				if (!plugin.GrenadeCount.ContainsKey(player.PlayerId))
					plugin.GrenadeCount.Add(player.PlayerId, 0);
			}
		}
	}
}