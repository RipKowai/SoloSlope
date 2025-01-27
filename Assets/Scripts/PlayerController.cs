using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float gravityScale = 2f; 
    private float verticalSpeed = 0f;

    public LayerMask collisionLayerMask;
    private Coroutine raycastCoroutine;

    private void Start()
    {
        raycastCoroutine = StartCoroutine(RaycastCoroutine());
    }

    private IEnumerator RaycastCoroutine()
    {
        while (true)
        {
            if (gameObject.activeSelf)
            {
                if (IsCollidingWithHill())
                {
                    verticalSpeed = 0f;
                }
            }

            yield return null;
        }
    }

    private void FixedUpdate()
    {
        float moveDirection = Input.GetAxis("Horizontal");
        transform.position += new Vector3(moveDirection * moveSpeed * Time.deltaTime, 0, 0);

        verticalSpeed -= gravityScale * Time.deltaTime;
        transform.position += new Vector3(0, verticalSpeed * Time.deltaTime, 0);

        float rotationAngle = moveDirection * moveSpeed * Time.deltaTime * 360;
        transform.Rotate(Vector3.forward, rotationAngle);
    }

    private bool IsCollidingWithHill()
    {
        Debug.Log($"Player Position: {transform.position}");

        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.down;


        float rayLength = 0.5f;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, collisionLayerMask);

        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.red);

        Debug.Log($"Raycast from {rayOrigin} in direction {rayDirection} with length {rayLength}");

        return hit.collider != null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter: " + collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collision Stay:" + collision.gameObject.name);
    }
}
