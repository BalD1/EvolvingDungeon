using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StdNounou
{
    public class SceneChanger : MonoBehaviour
    {
        [field: Scene]
        [field: SerializeField] public string TargetScene {  get; private set; }
        [field: SerializeField] public bool WaitForInput {  get; private set; }

        public void ChangeScene()
            => ChangeScene(TargetScene);
        public void ChangeScene(string targetScene)
        {
            SceneHandler.ST_TryChangeScene(targetScene, WaitForInput);
        }
    } 
}
