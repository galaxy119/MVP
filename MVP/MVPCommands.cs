using Smod2.Commands;
using Smod2.API;

namespace MVP
{
	class MVPCommand : ICommandHandler
	{
		public string GetCommandDescription()
		{
			return "";
		}
		public string GetUsage()
		{
			return "MVP Command List \n"+
			"[mvp] \n"+
			"mvp enable - Enables the MVP plugin. \n"+
			"mvp disable - Disables the MVP plugin. \n"+
			"mvp announce - Announces whoever the current MVP is. \n"+
			"mvp stats PlayerName - Gets the current kill count for the specifed player. \n"+
			"mvp help - Displays this.";
		}
		public string[] OnCall(ICommandSender sender, string[] args)
		{
			if (args.Length > 0)
			{
				switch (args[0].ToLower())
				{
					case "help":
						return new string[] {GetUsage()};
					case "enable":
						Functions.singleton.Enable();
						Functions.singleton.Refresh();
						return new string[] { "The MVP plugin has been enabled." };
					case "disable":
						MVP.enabled = false;
						return new string[] { "The MVP plugin has been disabled." };
					case "clear": 
						Functions.singleton.ClearStats();
						return new string[] { "All current MVP stats have been cleared." };
					case "announce":
						if (MVP.enabled)
						{
							Functions.singleton.Announce();
							return new string[] { "The current top player has been announced." };
						}
						return new string[] { "The MVP plugin must be enabled before using this command." };
					case "stats":
						if (MVP.enabled)
						{
							if (args.Length > 1)
							{
								Player player = GetPlayerFromString.GetPlayer(args[1]);
								if (player == null)
								{
									return new string[] { "Couldn't get player: " + args[1]};
								}
								MVP.singleton.Server.Map.ClearBroadcasts();
								MVP.singleton.Server.Map.Broadcast(15, player.Name + " has " + MVP.killCounter[player.Name] + " kills.", false);
								return new string[] { player.Name + "'s stats have been announced." };
							}
							return new string[] {"A player name must be specified."};
						}
						return new string[] {"The MVP plugin must be enabled before using this command."};
					default:
						return new string[] {GetUsage()};
				}
			}
			return new string[] {GetUsage()};
		}
	}
}