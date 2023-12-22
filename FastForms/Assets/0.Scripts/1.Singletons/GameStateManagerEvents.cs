using System;
using static StdNounou.GameStateManager;

namespace StdNounou
{
	public static class GameStateManagerEvents
	{
		public static event Action<E_GameStates> OnChangedGameState;
		public static void ChangedGameState(this GameStateManager manager, E_GameStates state)
			=> OnChangedGameState?.Invoke(state);
	} 
}