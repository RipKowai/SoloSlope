using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public LayerMask collisionLayerMask;
    private Vector3 lastPosition;
    private float verticalSpeed;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Perform raycasting to check for collisions
        if (IsCollidingWithGround())
        {
            Debug.Log("Collision detected with ground.");
            transform.position = lastPosition; 
        }
        lastPosition = transform.position;
    }

    public bool IsCollidingWithGround()
    {
        Debug.Log($"Player Position: {transform.position}");

        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.down;

        float rayLength = 0.5f;

        RaycastHit hit;
        bool isHit = Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength, collisionLayerMask);

        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);

        Debug.Log($"Raycast from {rayOrigin} in direction {rayDirection} with length {rayLength}");

        return isHit;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter: " + collision.gameObject.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision Stay: " + collision.gameObject.name);
    }

    public void SetVerticalSpeed(float speed)
    {
        verticalSpeed = speed;
    }
}
