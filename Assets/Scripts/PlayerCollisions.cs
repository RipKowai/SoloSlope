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

        float[] heights = new float[4];

        for (int i = 0; i < rayOrigins.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayOrigins[i], Vector3.down, out hit, 1f, collisionLayerMask))
            {
                Debug.DrawRay(rayOrigins[i], Vector3.down * hit.distance, Color.green);
                heights[i] = hit.point.y;
            }
            else
            {
                heights[i] = transform.position.y;  // Default to the player's position if no hit
            }
        }

        // Calculate height differences
        float heightFront = (heights[0] + heights[1]) / 2;
        float heightBack = (heights[2] + heights[3]) / 2;
        float heightRight = (heights[0] + heights[2]) / 2;
        float heightLeft = (heights[1] + heights[3]) / 2;

        Vector3 forwardSlope = new Vector3(0, heightBack - heightFront, 1).normalized;
        Vector3 rightSlope = new Vector3(1, heightLeft - heightRight, 0).normalized;

        Vector3 slopeDirection = (forwardSlope + rightSlope).normalized;

        Debug.Log($"Height Front: {heightFront}, Height Back: {heightBack}");
        Debug.Log($"Height Right: {heightRight}, Height Left: {heightLeft}");
        Debug.Log($"Calculated Slope Direction: {slopeDirection}");

        return slopeDirection;
    }

}
