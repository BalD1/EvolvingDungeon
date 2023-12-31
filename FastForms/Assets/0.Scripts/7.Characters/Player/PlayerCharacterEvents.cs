using System;

public static class PlayerCharacterEvents
{
	public static event Action<PlayerCharacter> OnPlayerCreated;
	public static void PlayerCreated(this PlayerCharacter player)
		=> OnPlayerCreated?.Invoke(player);

	public static event Action<PlayerCharacter> OnPlayerDeath;
	public static void PlayerDeath(this PlayerCharacter player)
		=> OnPlayerDeath?.Invoke(player);
}