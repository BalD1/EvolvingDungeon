using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New ScenesHandler", menuName = "Scriptable/Scenes/Handler")]
public class SO_ScenesHandler : ScriptableObject
{
    [System.Serializable]
    public struct S_SceneWithWeight
    {
        [field: SerializeField, Scene] public string FirstSceneToLoad { get; private set; }
        [field: SerializeField] public float Weight { get; private set; }
    }

    [field: SerializeField] public S_SceneWithWeight[] SceneWithWeights { get; private set; }
}