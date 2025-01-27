using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float gravityScale = 2f;
    private float verticalSpeed = 0f;

    private PlayerCollisions playerCollisions;

    private void Start()
    {
        playerCollisions = GetComponent<PlayerCollisions>();
    }

    private void Update()
    {
        //horizontal movement
        float moveDirection = Input.GetAxis("Horizontal");
        transform.position += new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);

        //gravity
        verticalSpeed -= gravityScale * Time.deltaTime;
        transform.position += new Vector3(0, verticalSpeed * Time.deltaTime, 0.01f);

        // Apply rotation to the ball
        float rotationAngle = moveDirection * moveSpeed * Time.deltaTime * 360;
        transform.Rotate(Vector3.forward, rotationAngle);

        if (playerCollisions.IsCollidingWithGround())
        {
            verticalSpeed = 0f;
        }
    }
}
