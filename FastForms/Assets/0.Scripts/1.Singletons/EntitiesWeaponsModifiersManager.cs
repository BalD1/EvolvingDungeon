using System;
using System.Collections.Generic;
using UnityEngine;
using StdNounou;

public class EntitiesWeaponsModifiersManager : Singleton<EntitiesWeaponsModifiersManager>
{
    [SerializeField] private SO_EntityWeaponsModifiers[] modifiers;

    protected override void Awake()
    {
        foreach (var item in modifiers)
        {
            item.Init();
        }
        base.Awake();
    }

    protected override void EventsSubscriber()
    {
    }
    
    protected override void EventsUnSubscriber()
    {
    }
}