using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public LayerMask collisionLayerMask;
    private Vector3 lastPosition;
    private Vector3 movementDirection;
    public float forceMultiplier = 10f;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Perform raycasting to check for collisions and slopes
        Vector3 slopeDirection = GetSlopeDirection();

        // Apply force based on slope direction
        movementDirection += slopeDirection * forceMultiplier * Time.fixedDeltaTime;
        transform.position += movementDirection * Time.fixedDeltaTime;

        Debug.Log($"Slope Direction: {slopeDirection}, Movement Direction: {movementDirection}");

        if (IsCollidingWithGround())
        {
            Debug.Log("Collision detected with ground.");
            movementDirection = Vector3.zero;  // Stop movement when colliding with the ground
            transform.position = lastPosition;
        }
        else
        {
            lastPosition = transform.position;
        }

    }

    public bool IsCollidingWithGround()
    {
        Debug.Log($"Player Position: {transform.position}");

        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.down;

        float rayLength = 0.5f;

        UnityEngine.RaycastHit hit;
        bool isHit = UnityEngine.Physics.Raycast(rayOrigin, rayDirection, out hit, rayLength, collisionLayerMask);

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

    private Vector3 GetSlopeDirection()
    {
        Vector3[] rayOrigins = new Vector3[4];
        rayOrigins[0] = transform.position + new Vector3(0.5f, 0, 0);   // Front Right
        rayOrigins[1] = transform.position + new Vector3(-0.5f, 0, 0);  // Front Left
        rayOrigins[2] = transform.position + new Vector3(0, 0, 0.5f);   // Back Right
        rayOrigins[3] = transform.position + new Vector3(0, 0, -0.5f);  // Back Left

        Vector3 heightDifferences = Vector3.zero;

        for (int i = 0; i < rayOrigins.Length; i++)
        {
            UnityEngine.RaycastHit hit;
            if (UnityEngine.Physics.Raycast(rayOrigins[i], Vector3.down, out hit, 1f, collisionLayerMask))
            {
                Debug.DrawRay(rayOrigins[i], Vector3.down * hit.distance, Color.green);

                switch (i)
                {
                    case 0:
                        heightDifferences.x -= hit.point.y;  // Front Right
                        heightDifferences.z -= hit.point.y;
                        break;
                    case 1:
                        heightDifferences.x += hit.point.y;  // Front Left
                        heightDifferences.z -= hit.point.y;
                        break;
                    case 2:
                        heightDifferences.x -= hit.point.y;  // Back Right
                        heightDifferences.z += hit.point.y;
                        break;
                    case 3:
                        heightDifferences.x += hit.point.y;  // Back Left
                        heightDifferences.z += hit.point.y;
                        break;
                }
            }
        }

        Vector3 slopeDirection = new Vector3(heightDifferences.x, 0, heightDifferences.z).normalized;
        Debug.Log($"Height Differences: {heightDifferences}, Calculated Slope Direction: {slopeDirection}");

        return slopeDirection;
    }
}
