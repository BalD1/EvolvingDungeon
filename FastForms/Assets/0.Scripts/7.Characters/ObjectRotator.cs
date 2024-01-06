using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] protected GameObject ownerObj;
    protected IComponentHolder ownerComponentsHolder;
    protected ObjectMotor ownerMotor;

    [SerializeField] private float rotationOffset;
    [SerializeField] private float rotationSlerpSpeed;

    protected float lookAngle;
    protected bool isUsingGamepad = false;

    private void Awake()
    {
        ownerComponentsHolder = ownerObj.GetComponent<IComponentHolder>();
        if (ownerComponentsHolder == null) return;
        ownerComponentsHolder.HolderTryGetComponent(IComponentHolder.E_Component.Motor, out ownerMotor);
    }

    public Quaternion RotationTowardsMovements(bool lerpRotation)
    {
        Vector2 lastDirection = ownerMotor.LastDirection.normalized;
        lookAngle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
        Quaternion angle = Quaternion.AngleAxis(lookAngle + 180, Vector3.forward);

        if (!lerpRotation) return ownerObj.transform.rotation = angle;
        return ownerObj.transform.rotation = Quaternion.Slerp(this.transform.rotation, angle, Time.deltaTime * rotationSlerpSpeed);
    }

    public Quaternion RotationTowardsMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5f;

        Vector3 selfPosByCam = CameraManager.CurrentCam.WorldToScreenPoint(this.transform.position);

        mousePos.x -= selfPosByCam.x;
        mousePos.y -= selfPosByCam.y;

        return RotationTowardsPosition(mousePos);
    }

    public Quaternion RotationTowardsPosition(Vector2 position)
    {
        position -= (Vector2)this.transform.position;
        lookAngle = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
        return ownerObj.transform.rotation = Quaternion.AngleAxis(lookAngle + rotationOffset, Vector3.forward);
    }

    public Quaternion GetRotationWithoutOffset()
    {
        Vector3 result = this.transform.rotation.eulerAngles;
        result.z = result.z - rotationOffset;
        return Quaternion.Euler(result);
    }
}
