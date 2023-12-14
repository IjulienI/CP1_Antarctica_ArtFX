using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovementEvann : MonoBehaviour
{
    [Header("Don't Touch")]
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PhysicsMaterial2D playerMaterial;
    [SerializeField] private Light2D _fire;

    [Header("Ground Control")]
    [SerializeField] private float accelerationFactorOnGround = 2f;
    [SerializeField] private float brakeFactorOnGround = 1f;
    [SerializeField] private float maxSpeedOnGround = 8f;

    [Header("Air Control")]
    [SerializeField] private float accelerationFactorInAir = 1f;
    [SerializeField] private float brakeFactorInAir = 0.5f;
    [SerializeField] private float maxSpeedInAir = 4f;

    [Header("Jump")]
    [SerializeField] private float jumpingPower = 5f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float normalGravityScale = 1f;
    [SerializeField] private float fallingGravityScale = 3f;

    private float coyoteTimer;
    private float horizontal;
    private float vertical;
    private float speedX;
    private float speedY;
    private float accelerationFactor = 2f;
    private float brakeFactor = 1f;
    private float maxSpeed = 8f;

    private bool isFacingRight = true;
    private bool isGrounded;

    bool _ladderInteraction = false;
    bool _onTriggerLadder = false;
    bool _canDown = false;
    bool canJump = true;
    float _fall = 1;
    bool _lunchFallAcceleration = false;

    [Header("Input Manager (don't touch)")]
    public InputActionReference interact;
    public InputActionReference upLight;
    public InputActionReference downLight;

    public static PlayerMovementEvann instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Update()
    {
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            _lunchFallAcceleration = false;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
        if (_onTriggerLadder == false)
        {
            _ladderInteraction = false;
            rb.gravityScale = fallingGravityScale;
            speedY = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (_lunchFallAcceleration == true)
        {
            _fall = Mathf.SmoothStep(_fall, 1, 0.2f);
            if (_fall == 0)
            {
                _lunchFallAcceleration = false;
            }
        }

        horizontal = Mathf.RoundToInt(horizontal);
        vertical = Mathf.RoundToInt(vertical);
        if (horizontal != 0)
        {
            if (!isGrounded)
            {
                maxSpeed = maxSpeedInAir;
                accelerationFactor = accelerationFactorInAir;
                brakeFactor = brakeFactorInAir;
            }
            else
            {
                maxSpeed = maxSpeedOnGround;
                accelerationFactor = accelerationFactorOnGround;
                brakeFactor = brakeFactorOnGround;
            }
            speedX = Mathf.MoveTowards(speedX, maxSpeed * horizontal, Time.deltaTime * accelerationFactor);
        }
        else
        {
           speedX = Mathf.MoveTowards(speedX, 0f, Time.deltaTime * brakeFactor);
        }

        if (_ladderInteraction == true && isGrounded == false)
        {

            rb.velocity = new Vector2(0, speedY);
        }
        else
        {
            rb.velocity = new Vector2(speedX, rb.velocity.y);
        }


        if (_ladderInteraction == true && isGrounded == true)
        {
            rb.gravityScale = 0;
            if (vertical != 0)
            {
                speedY = maxSpeed * vertical;
            }
            else { speedY = 0; }

            if (vertical < 0 && _canDown == true)
            {
                _collider.isTrigger = true;
            }
            else
            {
                _collider.isTrigger = false;
            }

            rb.velocity = new Vector2(speedX, speedY);
        }
        else if (_ladderInteraction == true)
        {

            rb.gravityScale = 0;
            if (vertical != 0)
            {
                speedY = maxSpeed * vertical;
            }
            else { speedY = 0; }

            if (vertical < 0 && _canDown == true)
            {
                _collider.isTrigger = true;
            }
            else
            {
                _collider.isTrigger = false;
            }
        }
        else if (vertical < 0 && _canDown == true && isGrounded ==true)
        {
            _collider.isTrigger = true;
            _lunchFallAcceleration = true;
            Invoke("GoDown", 0.2f);
        }
        else
        {
            
            if (rb.velocity.y >= 0)
            {
                rb.gravityScale = normalGravityScale;
            }
            else if (rb.velocity.y < 0)
            {
                _lunchFallAcceleration = true;
                rb.gravityScale = fallingGravityScale * _fall;
            }
        }
    }
    private void GoDown()
    {
        _collider.isTrigger = false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && coyoteTimer > 0f && canJump)
        {   
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void OnEnable()
    {
        interact.action.started += Interaction;
        upLight.action.started += UpLight;
        downLight.action.started += DownLight;
    }
    private void Interaction(InputAction.CallbackContext obj)
    {
        if (_onTriggerLadder)
        {
            _ladderInteraction = !_ladderInteraction;
            //if (_ladderInteraction == true)
            //{
            //    _collider.isTrigger = true;
            //}
        }
    }

    private void UpLight(InputAction.CallbackContext light)
    {
        if (_fire.pointLightOuterRadius == 2)
        {
            _fire.pointLightOuterRadius = 5;
        }
        else if (_fire.pointLightOuterRadius == 5)
        {
            _fire.pointLightOuterRadius = 10;
        }
        else
        {
            _fire.pointLightOuterRadius = 2;
        }
    }
    private void DownLight(InputAction.CallbackContext light)
    {
        if (_fire.pointLightOuterRadius == 10)
        {
            _fire.pointLightOuterRadius = 5;
        }
        else if (_fire.pointLightOuterRadius == 5)
        {
            _fire.pointLightOuterRadius = 2;
        }
        else
        {
            _fire.pointLightOuterRadius = 10;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoUp")
        {
            _onTriggerLadder = true;
        }
        if (collision.gameObject.tag == "FlyingPlatform")
        {
            _canDown = true;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            _fall = 0;
            _lunchFallAcceleration = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoUp")
        {
            _onTriggerLadder = false;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
            _lunchFallAcceleration = true;
        }
        if (collision.gameObject.tag == "FlyingPlatform")
        {
            _canDown = false;

        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }

    public void SetCanJump(bool x)
    {
        canJump = x;
    }

}
