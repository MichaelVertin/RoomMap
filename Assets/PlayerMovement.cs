using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.W))
            moveZ += 1f;

        if (Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.S))
            moveZ -= 1f;

        if (Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.D))
            moveX += 1f;

        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.A))
            moveX -= 1f;

        Vector3 direction = new Vector3(moveX, 0f, moveZ).normalized;

        Vector3 targetPosition = rb.position + direction * speed * Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);
    }
}