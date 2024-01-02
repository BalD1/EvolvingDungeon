using UnityEditor;
using UnityEngine;

namespace StdNounou
{
	public class GameStateManager : Singleton<GameStateManager>
	{
		public enum E_GameStates
		{
			MainMenu,
			InGame,
			Pause,
		}

		public static E_GameStates CurrentState { get; private set; } = E_GameStates.MainMenu;

		public static void ST_SetGameState(E_GameStates state)
		{
			if (Instance != null)
			{
				Instance.SetGameState(state);
				return;
			}

			CustomLogger.LogError(typeof(GameStateManager), "No instance found.");
		}

		public void SetGameState(E_GameStates state)
		{
            switch (state)
            {
                case E_GameStates.MainMenu:
                    break;

                case E_GameStates.InGame:
                    break;

                case E_GameStates.Pause:
                    break;
            }

            CurrentState = state;
			this.ChangedGameState(state);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) EditorApplication.isPaused = true;
        }

        protected override void EventsSubscriber()
        {
        }

        protected override void EventsUnSubscriber()
        {
        }
    } 
}