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
			plugin.Functions = this;
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
				MVP.multi_track[player.Name] = false;
				MVP.multikill[player.Name] = 0;
			}
		}
		public void Announce()
		{
			if (MVP.enabled)
			{
				KeyValuePair<string, int> max = new KeyValuePair<string, int>();
				foreach (var kvp in MVP.killCounter)
				{
					if (kvp.Value > max.Value)
						max = kvp;
				}
				MVP.Server.Map.ClearBroadcasts();
				MVP.Server.Map.Broadcast(25, "MVP: " + max.Key + " killed the most players, with " + max.Value + " total kills!", false);
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
			}
		}
	}
}