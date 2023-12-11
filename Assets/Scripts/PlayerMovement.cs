using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private bool accelerate;
    private float speed = 0;
    private float maxSpeed = 8;
    private bool goRight;
    private float jumpHeight = 450f;
    private bool isFalling;
    private float tempHeigh;
    private float baseGravity;
    private float fallGravity;
    Rigidbody2D rb;

    private void Awake()
    {        
        rb = GetComponent<Rigidbody2D>();
        baseGravity = rb.gravityScale;
        fallGravity = baseGravity * 2;
    }

    private void Update()
    {
        Walk();
        Jump();
    }

    private void Walk()
    {
        if (Input.GetKey(KeyCode.D))
        {
            if (!goRight)
            {
                if (speed == 0)
                {
                    accelerate = true;
                    goRight = true;
                }
                else
                {
                    accelerate = false;
                }
            }
            else
            {
                accelerate = true;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (goRight)
            {
                if (speed == 0)
                {
                    accelerate = true;
                    goRight = false;
                }
                else
                {
                    accelerate = false;
                }
            }
            else
            {
                accelerate = true;
            }
        }
        else
        {
            accelerate = false;
        }
        Velocity();
    }
    private void Jump()
    {        
        if(Input.GetKeyDown(KeyCode.Space) && !isFalling)
        {
            isFalling = true;
            rb.AddForce(transform.up * jumpHeight);
            tempHeigh = transform.position.y;
        }

        if(isFalling)
        {
            if(tempHeigh -0.01f < transform.position.y)
            {
                tempHeigh = transform.position.y;
            }
            else
            {
                rb.gravityScale = fallGravity;
            }
        }
    }
    private void Velocity()
    {
        if (goRight)
        {
            transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
        }

        if (accelerate)
        {
            if (speed <= maxSpeed)
            {
                if(!isFalling)
                {
                    speed += 70f * Time.deltaTime;
                }
                else
                {
                    speed += 40f * Time.deltaTime;
                }
            }
        }
        else
        {
            if (speed >= 0)
            {
                if(!isFalling)
                {
                    speed -= 60f * Time.deltaTime;
                }
                else
                {
                    speed -= 30f * Time.deltaTime;
                }
            }
            if (speed < 0)
            {
                speed = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isFalling = false;
            rb.gravityScale = baseGravity;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isFalling = true;
        }
    }
}
