using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxMoveSpeed = 10f;
    public float accelerationTime = 2f;
    public float gravityScale = 2f;

    private float verticalSpeed = 0f;
    private float currentSpeed = 0f;

    private PlayerCollisions playerCollisions;

    private void Start()
    {
        playerCollisions = GetComponent<PlayerCollisions>();
    }

    private void Update()
    {
        // Horizontal movement
        float moveDirection = Input.GetAxis("Horizontal");

        currentSpeed = Mathf.Lerp(currentSpeed, maxMoveSpeed, Time.deltaTime);

        transform.position += new Vector3(moveDirection * currentSpeed * Time.deltaTime, 0, 0);

        // Gravity
        verticalSpeed -= gravityScale * Time.deltaTime;
        transform.position += new Vector3(0, verticalSpeed * Time.deltaTime, 0);

        // Apply rotation to the ball
        //float rotationAngle = moveDirection * moveSpeed * Time.deltaTime * 60;
        //transform.Rotate(Vector3.forward, rotationAngle);

        if (playerCollisions.IsCollidingWithGround())
        {
            verticalSpeed = 0f;
        }

        if(moveDirection == 0)
        {
            currentSpeed = 0f;
        }
    }
}
