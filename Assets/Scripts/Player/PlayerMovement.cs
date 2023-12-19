using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Don't Touch")]
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PhysicsMaterial2D playerMaterial;
    [SerializeField] private Light2D _fire;
    [SerializeField] private Image coldStep1;
    [SerializeField] private Image coldStep2;
    [SerializeField] private Image coldStep3;
    [SerializeField] private Image coldStep4;

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

    [Header("Cold")]
    [SerializeField] private float coldness = 10f;
    public float actualCold = 0f;

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
    private bool canFlip = true;

    bool ladderInteraction = false;
    float ladderPosition;
    bool onTriggerLadder = false;
    bool canDown = false;
    bool canJump = true;
    float fall = 1;
    bool launchFallAcceleration = false;
    int stateOfFire = 1;
    bool progressiveFire = false;

    [Header("Input Manager (don't touch)")]
    public InputActionReference interact;
    public InputActionReference upLight;
    public InputActionReference downLight;
    [SerializeField] private Image numpadImg;

    public static PlayerMovement instance;

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
            launchFallAcceleration = false;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
        if (onTriggerLadder == false)
        {
            ladderInteraction = false;
            _rb.gravityScale = fallingGravityScale;
            speedY = 0f;
        }

        if (stateOfFire == 1)
        {
            if (actualCold > 0f)
            {
                actualCold -= Time.deltaTime * 2;
            }
        }
        else if (stateOfFire == 2)
        {
            actualCold += Time.deltaTime;
        }
        else
        {
            actualCold += Time.deltaTime *3;
        }
        if(actualCold > coldness)
        {
            if (actualCold > coldness * 2f)
            {
                if (actualCold > coldness * 3f)
                {
                    if (actualCold > coldness * 4f)
                    {
                        stateOfFire = 1;
                        progressiveFire = true;
                    }
                    else
                    {
                        coldStep4.color = new Color(255, 255, 255, (actualCold - 3f * coldness) / coldness);
                    }

                }
                else
                {
                    coldStep3.color = new Color(255, 255, 255, (actualCold - 2f * coldness) / coldness);
                }

            }
            else
            {
                coldStep2.color = new Color(255, 255, 255, (actualCold - coldness) / coldness);
            }

        }
        else
        {
            coldStep1.color = new Color(255, 255, 255, actualCold / coldness);
        }
    }

    private void FixedUpdate()
    {
        if (launchFallAcceleration == true)
        {
            fall = Mathf.SmoothStep(fall, 1, 0.2f);
            if (fall == 0)
            {
                launchFallAcceleration = false;
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

        if (ladderInteraction == true && isGrounded == false)
        {

            _rb.velocity = new Vector2(0, speedY);
        }
        else
        {
            _rb.velocity = new Vector2(speedX, _rb.velocity.y);
        }


        if (ladderInteraction == true && isGrounded == true)
        {
            _rb.gravityScale = 0;
            if (vertical != 0)
            {
                speedY = maxSpeed * vertical;
            }
            else { speedY = 0; }

            if (vertical < 0 && canDown == true)
            {
                _collider.isTrigger = true;
            }
            else
            {
                _collider.isTrigger = false;
            }

            _rb.velocity = new Vector2(speedX, speedY);
        }
        else if (ladderInteraction == true)
        {

            _rb.gravityScale = 0;
            if (vertical != 0)
            {
                speedY = maxSpeed * vertical;
            }
            else { speedY = 0; }

            if (vertical < 0 && canDown == true)
            {
                _collider.isTrigger = true;
            }
            else
            {
                _collider.isTrigger = false;
            }
        }
        else if (vertical < 0 && canDown == true && isGrounded ==true)
        {
            _collider.isTrigger = true;
            launchFallAcceleration = true;
            Invoke("GoDown", 0.25f);
        }
        else
        {
            
            if (_rb.velocity.y >= 0)
            {
                _rb.gravityScale = normalGravityScale;
            }
            else if (_rb.velocity.y < 0)
            {
                launchFallAcceleration = true;
                _rb.gravityScale = fallingGravityScale * fall;
            }
        }
        if (progressiveFire == true)
        {
            if (stateOfFire == 2)
            {
                _fire.pointLightOuterRadius = Mathf.SmoothStep(_fire.pointLightOuterRadius, 5.1f, 0.2f);
                if (_fire.pointLightOuterRadius <= 5.2 && _fire.pointLightOuterRadius >= 5)
                {
                    progressiveFire = false;
                }
            }
            else if (stateOfFire == 3)
            {
                _fire.pointLightOuterRadius = Mathf.SmoothStep(_fire.pointLightOuterRadius, 10.1f, 0.2f);
                if (_fire.pointLightOuterRadius >= 10)
                {
                    progressiveFire = false;
                }
            }
            else
            {
                _fire.pointLightOuterRadius = Mathf.SmoothStep(_fire.pointLightOuterRadius, 1.9f, 0.2f);
                if (_fire.pointLightOuterRadius <= 2)
                {
                    progressiveFire = false;
                }
            }
        }
    }
    //private void GoDown()
    //{
    //    _collider.isTrigger = false;
    //}

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && coyoteTimer > 0f && canJump && canFlip)
        {   
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);
            _rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
        }

        if (context.canceled && _rb.velocity.y > 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
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
        if (onTriggerLadder)
        {
            ladderInteraction = !ladderInteraction;
            transform.position = new Vector3(ladderPosition, transform.position.y, transform.position.z);
            //if (_ladderInteraction == true)
            //{
            //    _collider.isTrigger = true;
            //}
        }
    }

    public void UpLight(InputAction.CallbackContext light)
    {
        if (light.performed)
        {
            if (stateOfFire == 3)
            {
                stateOfFire = 2;
                progressiveFire = true;
            }
            else if (stateOfFire == 2)
            {
                stateOfFire = 1;
                progressiveFire = true;
            }
            else
            {
                stateOfFire = 3;
                progressiveFire = true;
            }
        }
    }
    public void DownLight(InputAction.CallbackContext light)
    {
        if (light.performed)
        {
            if (stateOfFire == 1)
            {
                stateOfFire = 2;
                progressiveFire = true;
            }
            else if (stateOfFire == 2)
            {
                stateOfFire = 3;
                progressiveFire = true;
            }
            else
            {
                stateOfFire = 1;
                progressiveFire = true;
            }
        }
    }

    public void PauseGame(InputAction.CallbackContext pause)
    {
        if (pause.performed && numpadImg != null && !numpadImg.IsActive() && canFlip)
        {
            canFlip = false;
            MenuManager.instance.PauseGame();
        }
        else if (pause.performed && !canFlip)
        {
            canFlip = true;
            MenuManager.instance.UnPauseGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoUp")
        {
            onTriggerLadder = true;
            ladderPosition = collision.transform.position.x;
        }
        //if (collision.gameObject.tag == "FlyingPlatform")
        //{
        //    _canDown = true;
        //}
        
        if (collision.gameObject.tag == "Untagged")
        {
            _collider.isTrigger = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            fall = 0;
            launchFallAcceleration = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoUp")
        {
            onTriggerLadder = false;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
            launchFallAcceleration = true;
        }
        //if (collision.gameObject.tag == "FlyingPlatform")
        //{
        //    print("exit");
        //    _canDown = false;

        //}
    }

    private void Flip()
    {
        if (canFlip)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
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
    public void SetCanFlip()
    {
        canFlip = true;
    }
}
