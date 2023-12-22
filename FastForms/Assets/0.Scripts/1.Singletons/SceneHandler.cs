using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StdNounou
{
	public class SceneHandler : PersistentSingleton<SceneHandler>
	{
        public const string LOADING_SCENE_NAME = "LoadingScene";

        [SerializeField, ReadOnly] private string nextSceneToLoadAsync = "";
        [SerializeField, ReadOnly] private bool waitForInputToEndLoading;

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
        }

        public static void ST_TryChangeScene(string sceneName, bool waitForInput)
            => Instance.TryChangeScene(sceneName, waitForInput);

        public void TryChangeScene(string sceneName, bool waitForInput)
        {
            nextSceneToLoadAsync = sceneName;
            waitForInputToEndLoading = waitForInput;
            SceneManager.LoadScene(LOADING_SCENE_NAME);
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this.Log("Loaded scene " + scene.name);
            if (scene.name ==  LOADING_SCENE_NAME && nextSceneToLoadAsync != "")
            {
                StartCoroutine(LoadSceneAsync(nextSceneToLoadAsync));
                nextSceneToLoadAsync = "";
            }
        }

        protected override void OnSceneUnloaded(Scene scene)
        {
        }
    } 
}