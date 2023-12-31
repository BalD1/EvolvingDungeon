using System;
using UnityEngine;

namespace StdNounou
{
	public static class SceneHandlerEvents
	{
		public static event Action<AsyncOperation> OnStartedLoadSceneAsync;
		public static void StartedLoadSceneAsync(this SceneHandler handler, AsyncOperation operation)
			=> OnStartedLoadSceneAsync?.Invoke(operation);

		public static event Action OnLoadingCompleted;
		public static void LoadingCompleted(this SceneHandler handler)
			=> OnLoadingCompleted?.Invoke();
	} 
}