using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public InputAction playerControlls;

    public float moveDirection;

    private void OnEnable()
    {
        playerControlls.Enable();
    }

    private void OnDisable()
    {
        playerControlls.Disable();
    }

    void Update()
    {
        moveDirection = playerControlls.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(moveDirection * moveSpeed, 0) * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 3.5f), transform.position.z);
    }
}
