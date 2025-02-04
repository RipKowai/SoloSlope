using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    public LayerMask collisionLayerMask;
    public float forceMultiplier = 10f;
    public float groundCheckDistance = 0.3f;
    public float snapDistanceThreshold = 0.1f; 
    public float raycastInterval = 0.5f; 

    private Vector3 movementDirection;
    private float raycastTimer = 0f;
    private Vector3 slopeDirection; 

    private void Start()
    {
        movementDirection = Vector3.zero; 
        slopeDirection = Vector3.zero; 
    }

    private void FixedUpdate()
    {
        // Apply movement direction excluding gravity, which is handled by another script
        transform.position += new Vector3(movementDirection.x, 0, movementDirection.z) * Time.fixedDeltaTime;

        // Update the timer for raycasting
        raycastTimer += Time.fixedDeltaTime;

        if (raycastTimer >= raycastInterval)
        {
            slopeDirection = GetSlopeDirection(); // Update slope direction

            // Apply force based on slope direction
            if (slopeDirection != Vector3.zero)
            {
                movementDirection += slopeDirection * forceMultiplier * Time.fixedDeltaTime;
            }

            raycastTimer = 0f; // Reset the timer after casting rays
        }

        // Log for debugging
        Debug.Log($"Slope Direction: {slopeDirection}, Movement Direction: {movementDirection}");

        if (IsCollidingWithGround())
        {
            Debug.Log("Collision detected with ground.");
            SnapToGround();
            movementDirection.y = 0f; 
        }

        if(CheckSideCollisions())
        {
            movementDirection = Vector3.zero;
        }
    }

    public bool IsCollidingWithGround()
    {
        Vector3 rayOrigin = transform.position;
        float rayLength = groundCheckDistance;

        bool isHit = Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayLength, collisionLayerMask);

        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red); 

        return isHit;
    }

    private bool CheckSideCollisions()
    {
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        float sideCheckDistance = 0.5f;
        
        foreach (var direction in directions)
        {
            if(Physics.Raycast(transform.position, direction, sideCheckDistance, collisionLayerMask))
            {
                Debug.DrawRay(transform.position, direction * sideCheckDistance, Color.yellow);
                return true;
            }
        }
        return false;
    }

    private void SnapToGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance + snapDistanceThreshold, collisionLayerMask))
        {
            float distanceToGround = hit.distance;
            if (distanceToGround > snapDistanceThreshold)
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + snapDistanceThreshold, transform.position.z);
            }
        }
    }

    private Vector3 GetSlopeDirection()
    {
        Vector3[] rayOrigins = new Vector3[4];
        rayOrigins[0] = transform.position + transform.TransformDirection(Vector3.forward * 0.5f);  // Front
        rayOrigins[1] = transform.position + transform.TransformDirection(Vector3.back * 0.5f);     // Back
        rayOrigins[2] = transform.position + transform.TransformDirection(Vector3.right * 0.5f);    // Right
        rayOrigins[3] = transform.position + transform.TransformDirection(Vector3.left * 0.5f);     // Left

        float maxDistance = 0f;
        Vector3 longestDirection = Vector3.zero;
        float[] distances = new float[4];

        for (int i = 0; i < rayOrigins.Length; i++)
        {
            if (Physics.Raycast(rayOrigins[i], Vector3.down, out RaycastHit hit, Mathf.Infinity, collisionLayerMask))
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

        bool allDistancesEqual = Mathf.Approximately(distances[0], distances[1]) &&
                                 Mathf.Approximately(distances[0], distances[2]) &&
                                 Mathf.Approximately(distances[0], distances[3]);

        if (allDistancesEqual)
        {
            longestDirection = Vector3.zero;  // Make sure slopeDirection is zero if all distances are equal
        }

        return longestDirection.normalized;
    }
}
