using StdNounou;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    [SerializeField] private GameObject ownerObj;
    private IComponentHolder ownerComponentsHolder;
    private PlayerMotor ownerMotor;

    [SerializeField] private float rotationOffset;
    [SerializeField] private float rotationSlerpSpeed;

    private float lookAngle;
    private bool isUsingGamepad = false;

    private void Awake()
    {
        ownerComponentsHolder = ownerObj.GetComponent<IComponentHolder>();
        ownerComponentsHolder.HolderTryGetComponent(IComponentHolder.E_Component.Motor, out ownerMotor);
    }

    private void Update()
    {
        SetAim();
    }

    private void SetAim()
    {
        ownerObj.transform.rotation = isUsingGamepad ?
                                      RotationTowardsMovements(true) :
                                      RotationTowardsMouse();
    }

    public Quaternion RotationTowardsMovements(bool lerpRotation)
    {
        Vector2 lastDirection = ownerMotor.LastDirection.normalized;
        lookAngle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
        Quaternion angle = Quaternion.AngleAxis(lookAngle + 180, Vector3.forward);

        if (!lerpRotation) return angle;
        return Quaternion.Slerp(this.transform.rotation, angle, Time.deltaTime * rotationSlerpSpeed);
    }

    public Quaternion RotationTowardsMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5f;

        Vector3 selfPosByCam = Camera.main.WorldToScreenPoint(ownerObj.transform.position);

        mousePos.x -= selfPosByCam.x;
        mousePos.y -= selfPosByCam.y;

        lookAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(lookAngle + rotationOffset, Vector3.forward);
    }
}
