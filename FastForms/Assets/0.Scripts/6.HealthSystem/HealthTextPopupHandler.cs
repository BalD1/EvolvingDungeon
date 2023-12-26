using StdNounou;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class HealthTextPopupHandler : MonoBehaviourEventsHandler
{
    [SerializeField] private HealthSystem targetSystem;
    [SerializeField] private Vector3 successiveOffset = new Vector3(0,1,0);

    [SerializeField] private SO_TextPopupData damagedPopupData;

    private List<TextPopup> textPopupList;

    private void Reset()
    {
        targetSystem = this.GetComponent<HealthSystem>();
        damagedPopupData = ResourcesObjectLoader.GetTextPopupDataHolder().GetAsset("Damages") as SO_TextPopupData;
        EditorUtility.SetDirty(this);
    }

    protected override void EventsSubscriber()
    {
        targetSystem.OnTookDamages += OnTargetTookDamages;
    }

    protected override void EventsUnSubscriber()
    {
        if (targetSystem != null)
            targetSystem.OnTookDamages -= OnTargetTookDamages;
    }

    protected override void Awake()
    {
        base.Awake();
        textPopupList = new List<TextPopup>();
    }

    private void OnTargetTookDamages(IDamageable.DamagesData damageData)
    {
        TextPopup current = TextPopup.Create(damageData.Damages.ToString(), targetSystem.GetHealthPopupPosition(), damagedPopupData);
        current.OnEnd += OnTextEnded;
        for (int i = 0; i < textPopupList.Count; i++)
        {
            textPopupList[i].AddToTargetPosition(successiveOffset);
            textPopupList[i].SetListIndex(textPopupList.Count - i);
        }
        textPopupList.Add(current);
    }

    private void OnTextEnded(TextPopup text)
    {
        text.OnEnd -= OnTextEnded;
        textPopupList.Remove(text);
    }
}
