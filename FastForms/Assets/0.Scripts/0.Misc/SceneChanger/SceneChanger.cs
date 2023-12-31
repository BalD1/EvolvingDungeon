using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StdNounou
{
    public class SceneChanger : MonoBehaviour
    {
        [field: Scene]
        [field: SerializeField] public string[] TargetScenes {  get; private set; }
        [field: SerializeField] public bool WaitForInput {  get; private set; }

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
    } 
}
