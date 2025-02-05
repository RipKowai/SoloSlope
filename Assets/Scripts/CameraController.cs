using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition,smoothSpeed);

        transform.position = smoothedPosition;

        transform.rotation = Quaternion.Euler(30f, 0f, 0f);
    }
}
