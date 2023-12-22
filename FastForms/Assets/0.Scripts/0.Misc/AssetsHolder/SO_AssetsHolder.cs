using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace StdNounou
{
    public abstract class SO_AssetsHolder<T> : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<string, T> assetsDictionnary;

        public T GetAsset(string id)
        {
            if (assetsDictionnary == null || assetsDictionnary.Count == 0)
            {
                this.LogError("Dictionnary was not initialized or is empty.");
                return default(T);
            }
            if (assetsDictionnary.TryGetValue(id, out T sprite)) return sprite;
            this.LogError($"Could not fint sprite {id} in dictionnary.");
            return default(T);
        }
    } 
}