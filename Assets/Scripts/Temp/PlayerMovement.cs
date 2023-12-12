using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    private float accelerationSpeed = 60f;
    private float decelerationSpeed = 80f;
    private float currentSpeed = 0f;
    private float maxSpeed = 8f;

    private float gravityScale = 3;
    private float fallingGravityScale = 5;
    public float coyoteTime = 0.1f;
    private float coyoteTimer;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.D))
        {
            if (!isGrounded)
            {
                accelerationSpeed = 50f;
                decelerationSpeed = 20f;
                maxSpeed = 6f;
            }
            else
            {
                accelerationSpeed = 60f;
                decelerationSpeed = 80f;
                maxSpeed = 8f;
            }
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, Time.deltaTime * accelerationSpeed);
            //currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (!isGrounded)
            {
                accelerationSpeed = 50f;
                decelerationSpeed = 20f;
                maxSpeed = 6f;
            }
            else
            {
                accelerationSpeed = 60f;
                decelerationSpeed = 80f;
                maxSpeed = 8f;
            }
            currentSpeed = Mathf.MoveTowards(currentSpeed, -maxSpeed, Time.deltaTime * accelerationSpeed);
            //currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, Time.deltaTime * decelerationSpeed);
        }

        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && coyoteTimer > 0f)
        {
            Jump();
        }
        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravityScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Jump()
    {
        isGrounded = false;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * 12f, ForceMode2D.Impulse);
    }

}

