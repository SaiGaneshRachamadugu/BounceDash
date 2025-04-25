using UnityEngine;

public class VerticalCameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    private float highestY;

    void Start()
    {
        if (target != null)
            highestY = target.position.y;
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (target.position.y > highestY)
        {
            highestY = target.position.y;
        }

        Vector3 targetPos = new Vector3(0, highestY, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
