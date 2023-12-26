using System;

public static class PlayerCharacterEvents
{
	public static event Action<PlayerCharacter> OnPlayerCreated;
	public static void PlayerCreated(this PlayerCharacter player)
		=> OnPlayerCreated?.Invoke(player);
}