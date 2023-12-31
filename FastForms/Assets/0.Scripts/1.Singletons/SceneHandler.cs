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

        private bool isLoading;

        public IEnumerator LoadSceneMultiplesAsync(Queue<string> scenesNames)
        {
            yield return new WaitForSeconds(0.1f);
            AsyncOperation[] operations = new AsyncOperation[scenesNames.Count];
            int idx = 0;
            LoadSceneMode loadSceneMode = LoadSceneMode.Single;

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
        public IEnumerator LoadSceneAsync(string sceneName)
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

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this.Log("Loaded scene " + scene.name);
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
                StartCoroutine(LoadSceneMultiplesAsync(scenesToLoad));
            }
        }

        protected override void OnSceneUnloaded(Scene scene)
        {
        }
    } 
}