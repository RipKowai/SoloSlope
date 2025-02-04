using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public LayerMask collisionLayerMask;
    private Vector3 lastPosition;
    private Vector3 movementDirection;
    public float forceMultiplier = 10f;
    public float groundCheckDistance = 0.3f;

    private void Start()
    {
        lastPosition = transform.position;
        movementDirection = Vector3.zero;
    }

    private void FixedUpdate()
    {
        // Perform raycasting to check for collisions and slopes
        Vector3 slopeDirection = GetSlopeDirection();

        // Apply force based on slope direction
        if (slopeDirection != Vector3.zero)
        {
            movementDirection += slopeDirection * forceMultiplier * Time.fixedDeltaTime;
        }

        // Apply movement direction excluding gravity, which is handled by another script
        transform.position += new Vector3(movementDirection.x, 0, movementDirection.z) * Time.fixedDeltaTime;

        Debug.Log($"Slope Direction: {slopeDirection}, Movement Direction: {movementDirection}");

        // Check for ground collision and adjust position
        if (IsCollidingWithGround())
        {
            Debug.Log("Collision detected with ground.");
            SnapToGround();
            movementDirection.y = 0f; // Reset vertical movement
        }
    }

    public bool IsCollidingWithGround()
    {
        Vector3 rayOrigin = transform.position;
        float rayLength = groundCheckDistance;

        bool isHit = Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayLength, collisionLayerMask);

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red); // Visualize ground check ray

        return isHit;

    }

    private void SnapToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, collisionLayerMask))
        {
            float distanceToGround = hit.distance;
            if(distanceToGround > 0f)
            {
                transform.position -= new Vector3(0, distanceToGround, 0);
            }
        }
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
        rayOrigins[0] = transform.position + transform.TransformDirection(Vector3.forward * 0.5f);   // Front
        rayOrigins[1] = transform.position + transform.TransformDirection(Vector3.back * 0.5f);      // Back
        rayOrigins[2] = transform.position + transform.TransformDirection(Vector3.right * 0.5f);     // Right
        rayOrigins[3] = transform.position + transform.TransformDirection(Vector3.left * 0.5f);      // Left

        float maxDistance = 0f;
        Vector3 longestDirection = Vector3.zero;
        float[] distances = new float[4];

        for (int i = 0; i < rayOrigins.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayOrigins[i], Vector3.down, out hit, Mathf.Infinity, collisionLayerMask))
            {
                Debug.DrawRay(rayOrigins[i], Vector3.down * hit.distance, Color.green);  // Draw the ray for visualization
                distances[i] = hit.distance;
                if (hit.distance > maxDistance)
                {
                    maxDistance = hit.distance;
                    longestDirection = rayOrigins[i] - transform.position; // Direction from the origin to the ray origin
                }
            }
            else
            {
                Debug.DrawRay(rayOrigins[i], Vector3.down * 10f, Color.red);  // Draw the ray even if it doesn't hit anything
                distances[i] = 0f;
            }
        }
        bool allDistancesEqual = Mathf.Approximately(distances[0], distances[1])&&
                                 Mathf.Approximately(distances[0], distances[2])&&
                                 Mathf.Approximately(distances[0], distances[3]);
        if(allDistancesEqual)
        {
            longestDirection = Vector3.zero;
        }

        return longestDirection.normalized;
    }

}
