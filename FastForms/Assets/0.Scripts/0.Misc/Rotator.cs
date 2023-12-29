using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotationOffset = 90;
    private Camera mainCam;
    private float lookAngle;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    public Quaternion RoateTowardsMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5f;

        Vector3 selfPosByCam = mainCam.WorldToScreenPoint(this.transform.position);

        mousePos.x -= selfPosByCam.x;
        mousePos.y -= selfPosByCam.y;

        lookAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(lookAngle + rotationOffset, Vector3.forward);
    }
}
