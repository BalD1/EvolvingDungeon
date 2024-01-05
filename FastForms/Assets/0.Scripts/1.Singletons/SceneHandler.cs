using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StdNounou
{
	public class SceneHandler : PersistentSingleton<SceneHandler>
	{
        public const string LOADING_SCENE_NAME = "LoadingScene";
        public const string MAINSCENE_NAME = "MainScene";
        public const string MAINMENU_NAME = "MainMenu";

        [SerializeField, ReadOnly] private string[] nextScenesToLoadAsync;
        [SerializeField, ReadOnly] private bool waitForInputToEndLoading;

        private List<Scene> currentlyLoadedScenes;

        public event Action OnEndedUnloading;

        private bool isLoading;
        private bool isUnloading;

        protected override void Awake()
        {
            base.Awake();
            currentlyLoadedScenes = new List<Scene>();
        }

        public void StartLoadSceneMultiplesAsync(Queue<string> scenesNames, LoadSceneMode firstSceneLoadMode)
            => StartCoroutine(LoadSceneMultiplesAsync(scenesNames, firstSceneLoadMode));
        private IEnumerator LoadSceneMultiplesAsync(Queue<string> scenesNames, LoadSceneMode firstSceneLoadMode)
        {
            yield return new WaitForSeconds(0.1f);
            AsyncOperation[] operations = new AsyncOperation[scenesNames.Count];
            int idx = 0;
            LoadSceneMode loadSceneMode = firstSceneLoadMode;

            while(scenesNames.Count > 0)
            {
                if (operations[idx] != null)
                {
                    while (operations[idx].progress < .9f) yield return null;
                    idx++;
                }

                AsyncOperation op = SceneManager.LoadSceneAsync(scenesNames.Dequeue(), loadSceneMode);
                op.allowSceneActivation = false;
                operations[idx] = op;
                this.StartedLoadSceneAsync(op);
                loadSceneMode = LoadSceneMode.Additive;
            }

            this.LoadingCompleted();

            if (waitForInputToEndLoading)
            {
                while (!Input.anyKeyDown) yield return null;
            }

            foreach (var item in operations) item.allowSceneActivation = true;
            nextScenesToLoadAsync = null;
            isLoading = false;
        }
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            yield return new WaitForSeconds(0.1f);
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;
            this.StartedLoadSceneAsync(op);

            while(!op.isDone)
            {
                if (op.progress >= .9f)
                    op.allowSceneActivation = (!waitForInputToEndLoading) || (waitForInputToEndLoading && Input.anyKeyDown);

                yield return null;
            }
            isLoading = false;
        }

        public static void ST_TryChangeScene(string sceneName, bool waitForInput)
            => Instance.TryChangeScene(new string[1] {sceneName}, waitForInput);
        public static void ST_TryChangeScenesMultiples(string[] scenes, bool waitForInput)
            => Instance.TryChangeScene(scenes, waitForInput);

        public void TryChangeScene(string[] scenesNames, bool waitForInput)
        {
            nextScenesToLoadAsync = scenesNames;
            waitForInputToEndLoading = waitForInput;
            SceneManager.LoadScene(LOADING_SCENE_NAME);
        }

        public void StartUnloadSubscenes()
            => StartCoroutine(UnloadSubscenes());
        private IEnumerator UnloadSubscenes()
        {
            if (isUnloading || isLoading) yield return null;
            isUnloading = true;

            foreach (var item in currentlyLoadedScenes)
            {
                if (!item.isSubScene) continue;
                AsyncOperation op = SceneManager.UnloadSceneAsync(item);

                while (!op.isDone) yield return null;
            }

            isUnloading = false;
            OnEndedUnloading?.Invoke();
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this.Log("Loaded scene " + scene.name);
            currentlyLoadedScenes.Add(scene);

            if (scene.name ==  LOADING_SCENE_NAME && !isLoading)
            {
                isLoading = true;
                if (nextScenesToLoadAsync.Length == 1)
                {
                    StartCoroutine(LoadSceneAsync(nextScenesToLoadAsync[0]));
                    nextScenesToLoadAsync = null;
                    return;
                }

                Queue<string> scenesToLoad = new Queue<string>(nextScenesToLoadAsync);
                StartLoadSceneMultiplesAsync(scenesToLoad, LoadSceneMode.Single);
            }
        }

        protected override void OnSceneUnloaded(Scene scene)
        {
            this.Log("Unloaded scene " + scene.name);
            currentlyLoadedScenes.Remove(scene);
        }
    } 
}