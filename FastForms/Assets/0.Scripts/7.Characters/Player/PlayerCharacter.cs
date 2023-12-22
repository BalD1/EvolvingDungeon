using AYellowpaper.SerializedCollections;
using StdNounou;
using System;
using UnityEngine;
using static IComponentHolder;

public class PlayerCharacter : MonoBehaviour, IComponentHolder
{
    [field: SerializeField] public SerializedDictionary<E_Component, Component> OwnerComponents {  get; private set; }
    public event Action<ComponentChangeEventArgs> OnComponentModified;

    public ExpectedType HolderGetComponent<ExpectedType>(E_Component component) where ExpectedType : Component
        => OwnerComponents[component] as ExpectedType;

    public E_Result HolderTryGetComponent<ExpectedType>(E_Component component, out ExpectedType result) where ExpectedType : Component
    {
        result = null;
        if (!OwnerComponents.TryGetValue(component, out Component brutResult))
            return E_Result.ComponentNotFound;

        if (brutResult.GetType() != typeof(ExpectedType))
            return E_Result.TypeUnmatch;

        result = brutResult as ExpectedType;
        return E_Result.Success;
    }

    public void HolderChangeComponent<ExpectedType>(E_Component componentType, ExpectedType component) where ExpectedType : Component
    {
        if (!OwnerComponents.ContainsKey(componentType))
            OwnerComponents.Add(componentType, component);
        else
            OwnerComponents[componentType] = component;

        OnComponentModified?.Invoke(new ComponentChangeEventArgs(componentType, component));
    }
}
