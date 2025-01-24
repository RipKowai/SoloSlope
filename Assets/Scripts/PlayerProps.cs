using UnityEngine;

public class PlayerProps : MonoBehaviour
{
    public float speed = 100.0f;
    private Rigidbody rb;

    public ScoreManager scoreManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ResetPosition();

        AddStartingForce();
    }

    public void ResetPosition()
    {
        rb.position = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }

    public void AddStartingForce()
    {
        float x = Random.value < 0.5f ? -1.0f : 1.0f;
        float y = Random.value < 0.5f ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f);

        Vector2 direction = new Vector2(x, y);
        rb.AddForce(direction * this.speed);
    }

    public void AddForce(Vector2 force)
    {
        rb.AddForce(force);
    }
}
