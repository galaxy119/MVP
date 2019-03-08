#MVP
======
made by Joker119
## Description
Tracks player kills and announced quickly-timed multi-kills and the player with the most kills at the end of the round.

### Features
 - Configurable Multi-kill timer
 - Configurable multi-kill message
 - Announced the MVP at the end of the round
 - Command to announce the current MVP leader for the current round
 - Command to announce the stats of a specific player


### Config Settings
Config option | Config Type | Default Value | Description
:---: | :---: | :---: | :------
mvp_enable | Bool | true | If the plugin should be enabled or not.
mvp_track_scp_kills | Bool | true | If SCP kills should count towards the total kill count for a player.
mvp_track_scps | Bool | true | If SCP players should be eligable to be an MVP.
mvp_multikill | Bool | true | If the plugin should track and announce multi-kills.
mvp_multikill_delay | Float | 3 | The amount of time a player has to qualify for a multikill to be announced.
mvp_multikill_num | Int | 3 | The number of kills required for a player's multikill to get announced.
mvp_multi_text | String | is on fire! | The text string displayed after the players name in the multi-kill announcement.

### Commands
  Command |  |  | Description
:---: | :---: | :---: | :------
**Aliases** | **mvp** |
mvp enable | | | Enabled the MVP plugin.
mvp disable | | | Disabled the MVP plugin.
mvp announce | | | Announces the current leader of the MVP list.
mvp stats PlayerName | | | Announced the number of kills for the speficied player.
mvp help | | | Displays the command list.
