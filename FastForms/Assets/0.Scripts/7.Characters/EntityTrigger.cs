using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EntityTrigger : MonoBehaviour, ITriggerable
{
    [SerializeField] private Collider2D trigger;
    [SerializeField] private GameObject ownerObj;
    public IComponentHolder ComponentHolder { get; private set; }

    private void Reset()
    {
        ownerObj = this.transform.parent.gameObject;
        trigger = this.GetComponent<Collider2D>();
    }

    private void Awake()
    {
        ComponentHolder = ownerObj.GetComponent<IComponentHolder>();
    }

    public void Activate()
    {
        trigger.enabled = true;
    }
    public void Deactivate()
    {
        trigger.enabled = false;
    }

    public IComponentHolder GetComponentHolder()
        => ComponentHolder;

    public void OnTrigger(TriggerEventArgs eventArgs)
    {
    }
}
