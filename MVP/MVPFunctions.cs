using Smod2;
using Smod2.API;
using System;
using Smod2.Events;
using System.Collections.Generic;
using System.Linq;
using Smod2.Commands;

namespace MVP
{
	public class Functions
	{
		public MVP plugin;
		public Functions(MVP plugin) => this.plugin = plugin;

		public IEnumerable<float> MultiKill(Player player, float delay)
		{
			if (!plugin.multi_track[player.Name])
			{
				plugin.multi_track[player.Name] = true;
				yield return delay;
				plugin.Info(plugin.multikill[player.Name].ToString() + " " + plugin.multi_kill_num);
				if (plugin.multikill[player.Name] >= plugin.multi_kill_num)
				{
					plugin.Server.Map.ClearBroadcasts();
					plugin.Server.Map.Broadcast(10, player.Name + " " + plugin.multi_text, false);
					if (player.TeamRole.Team == Smod2.API.Team.SCP)
					{
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
							default:
								break;
						}
					}
				}
				plugin.multi_track[player.Name] = false;
				plugin.multikill[player.Name] = 0;
			}
		}
		public void Announce()
		{
			if (plugin.enabled)
			{
				KeyValuePair<string, double> max = new KeyValuePair<string, double>();
				foreach (var kvp in plugin.scp_kill_count)
				{
					if (kvp.Value > plugin.killCounter[kvp.Key])
					{
						plugin.killCounter[kvp.Key] += plugin.scp_kill_count[kvp.Key];
					}
				}

				foreach (var kvp in plugin.killCounter)
				{
					if (kvp.Value > plugin.scp_kill_count[kvp.Key] && plugin.half_scp_kills)
					{
						plugin.killCounter[kvp.Key] += (plugin.scp_kill_count[kvp.Key] / 2);
					}
					if (kvp.Value > max.Value)
						max = kvp;
				}
				if (plugin.half_scp_kills)
				{
					plugin.Server.Map.Broadcast(25, "MVP: " + max.Key + " killed the most players!", false);
				}
				else
				{
					plugin.Server.Map.Broadcast(25, "MVP: " + max.Key + " killed the most players, with " + max.Value + " total kills!", false);
				}
			}
		}
		public IEnumerable<float> ClearStats()
		{
			yield return 5;
			plugin.killCounter.Clear();
			plugin.scp_kill_count.Clear();
			plugin.multikill.Clear();
			plugin.multi_track.Clear();
		}
		public void Enable()
		{
			plugin.enabled = true;
		}
		public void Refresh()
		{
			plugin.Debug("Refreshing player list..");
			foreach (Player player in plugin.Server.GetPlayers())
			{
				if (!plugin.killCounter.ContainsKey(player.Name))
				{
					plugin.killCounter.Add(player.Name, 0);
				}
				if (!plugin.multi_track.ContainsKey(player.Name))
				{
					plugin.multi_track.Add(player.Name, false);
				}
				if (!plugin.multikill.ContainsKey(player.Name))
				{
					plugin.multikill.Add(player.Name, 0);
				}
				if (!plugin.scp_kill_count.ContainsKey(player.Name))
				{
					plugin.scp_kill_count.Add(player.Name, 0);
				}
			}
		}
	}
}