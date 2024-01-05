using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StdNounou
{
    public class SceneChanger : MonoBehaviour
    {
        [field: Scene]
        [field: SerializeField] public string[] TargetScenes {  get; private set; }
        [field: SerializeField] public bool WaitForInput {  get; private set; }
        [field: SerializeField] public bool UnloadSubscenesBeforeLoad { get; private set; } = true;

        public void ChangeScene()
        {
            if (TargetScenes.Length == 0)
            {
                this.LogError("Scenes Array was empty.");
                return;
            }

            if (TargetScenes.Length == 1)
            {
                ChangeScene(TargetScenes[0]);
                return;
            }

            ChangeScenesMultiples(TargetScenes);
        }

        public void ChangeScene(string targetScene)
        {
            SceneHandler.ST_TryChangeScene(targetScene, WaitForInput);
        }

        public void ChangeScenesMultiples(string[] targetScenes)
        {
            SceneHandler.ST_TryChangeScenesMultiples(targetScenes, WaitForInput);
        }

        public void LoadTargetScenesAsSubscene()
        {
            if (UnloadSubscenesBeforeLoad)
            {
                SceneHandler.Instance.OnEndedUnloading += LoadSceneOnUnloadEnded;
                SceneHandler.Instance.StartUnloadSubscenes();
                return;
            }

            Queue<string> scenesToLoad = new Queue<string>(TargetScenes);
            SceneHandler.Instance.StartLoadSceneMultiplesAsync(scenesToLoad, LoadSceneMode.Additive);
        }

        public void LoadSceneOnUnloadEnded()
        {
            SceneHandler.Instance.OnEndedUnloading -= LoadSceneOnUnloadEnded;
            Queue<string> scenesToLoad = new Queue<string>(TargetScenes);
            SceneHandler.Instance.StartLoadSceneMultiplesAsync(scenesToLoad, LoadSceneMode.Additive);
        }
    } 
}
