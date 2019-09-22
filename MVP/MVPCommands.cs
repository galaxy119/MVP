using System.Collections.Generic;
using System.Linq;
using Smod2.Commands;
using Smod2.API;

namespace MVP
{
	class MvpCommand : ICommandHandler
	{
		private readonly Mvp plugin;
		public MvpCommand(Mvp plugin) => this.plugin = plugin;

		public string GetCommandDescription() => "";

		public string GetUsage() =>
			"MVP Command List \n" +
			"[mvp] \n" +
			"mvp enable - Enables the MVP plugin. \n" +
			"mvp disable - Disables the MVP plugin. \n" +
			"mvp announce - Announces whoever the current MVP is. \n" +
			"mvp stats PlayerName - Gets the current kill count for the specifed player. \n" +
			"mvp help - Displays this.";

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			if (args.Length <= 0) return new string[] { GetUsage() };
			
			switch (args[0].ToLower())
			{
				case "help":
					return new string[] { GetUsage() };
				case "enable":
					plugin.Functions.Enable();
					plugin.Functions.Refresh();
					return new string[] { "The MVP plugin has been enabled." };
				case "disable":
					plugin.Enabled = false;
					return new string[] { "The MVP plugin has been disabled." };
				case "clear":
					plugin.Functions.ClearStats();
					return new string[] { "All current MVP stats have been cleared." };
				case "announce":
					if (!plugin.Enabled)
						return new string[] { "The MVP plugin must be enabled before using this command." };
					
					plugin.Functions.Announce();
					return new string[] { "The current top player has been announced." };
				case "stats":
					if (!plugin.Enabled)
						return new string[] { "The MVP plugin must be enabled before using this command." };

					if (args.Length <= 1) return new string[] { "A player name must be specified." };

					Player player;
					List<Player> players = plugin.Server.GetPlayers(args[1]);
					if (players == null || players.Count == 0) return new string[] { "Player not found."};
					player = players.OrderBy(ply => ply.Name.Length).First();
					
					plugin.Server.Map.ClearBroadcasts();
					plugin.Server.Map.Broadcast(15, player.Name + " has " + plugin.KillCounter[player.PlayerId] + " kills.", false);
					return new string[] { player.Name + "'s stats have been announced." };
				default:
					return new string[] { GetUsage() };
			}
		}
	}
}