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
		public static Functions singleton;
		public MVP MVP;
		public Functions(MVP plugin)
		{
			this.MVP = plugin;
			Functions.singleton = this;
		}

		public IEnumerable<float> MultiKill(Player player, float delay)
		{
			if (!MVP.multi_track[player.Name])
			{
				MVP.multi_track[player.Name] = true;
				yield return delay;
				if (MVP.multikill[player.Name] >= MVP.multi_kill_num)
				{
					MVP.Server.Map.ClearBroadcasts();
					MVP.Server.Map.Broadcast(10, player.Name + " " + MVP.multi_text, false);
				}
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
					}
				}
				MVP.multi_track[player.Name] = false;
				MVP.multikill[player.Name] = 0;
			}
		}
		public void Announce()
		{
			if (MVP.enabled)
			{
				KeyValuePair<string, double> max = new KeyValuePair<string, double>();
				foreach (var kvp in MVP.scp_kill_count)
				{
					if (kvp.Value > MVP.killCounter[kvp.Key])
					{
						MVP.killCounter[kvp.Key] += (MVP.scp_kill_count[kvp.Key] / 2);
					}
				}
				
				foreach (var kvp in MVP.killCounter)
				{
					if (kvp.Value > MVP.scp_kill_count[kvp.Key] && MVP.half_scp_kills)
					{
						MVP.killCounter[kvp.Key] += (MVP.scp_kill_count[kvp.Key] / 2);
					}
					if (kvp.Value > max.Value)
						max = kvp;
				}
				MVP.Server.Map.ClearBroadcasts();
				if (MVP.half_scp_kills)
				{
					MVP.Server.Map.Broadcast(25, "MVP: " + max.Key + " killed the most players!", false);
				}
				else
				{
					MVP.Server.Map.Broadcast(25, "MVP: " + max.Key + " killed the most players, with " + max.Value + " total kills!", false);
				}
			}
		}
		public void Enable()
		{
			MVP.enabled = true;
			MVP.Info("Refreshing player list..");
			foreach (Player player in MVP.Server.GetPlayers())
			{
				if (!MVP.killCounter.ContainsKey(player.Name))
				{
					MVP.killCounter[player.Name] = 0;
				}
				if (!MVP.multi_track.ContainsKey(player.Name))
				{
					MVP.multi_track[player.Name] = false;
				}
				if (!MVP.multikill.ContainsKey(player.Name))
				{
					MVP.multikill[player.Name] = 0;
				}
				if (!MVP.scp_kill_count.ContainsKey(player.Name))
				{
					MVP.scp_kill_count[player.Name] = 0;
				}
			}
		}
	}
}