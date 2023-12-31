using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Don't Touch")]
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PhysicsMaterial2D playerMaterial;
    [SerializeField] private Light2D _fire;
    [SerializeField] private Image coldStep1, coldStep2, coldStep3, coldStep4;
    [SerializeField] private GameObject glassHit1, glassHit2, glassHit3;
    [SerializeField] private bool hasNumpadCanvas;
    [SerializeField] bool running = false;

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

    [Header("Flame")]
    [SerializeField] private float coldness = 10f;
    float actualCold = 0f;
    [SerializeField] Animator flameAnim;
    private Animator anim;

    [Header("Sound")]
    [SerializeField] private AudioClip[] clip;
    [SerializeField] private AudioClip clipLanding;
    [SerializeField] private AudioSource audioSource;

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
    public int stateOfFire = 1;
    bool progressiveFire = false;

    bool onMucus = false;
    float timeInMucus = 0;
    float timeOutMucus = 0;
    float timeIddle = 0;
    bool stopTriggerDance = false;
    

    [Header("Input Manager (don't touch)")]
    public InputActionReference interact;
    public InputActionReference upLight;
    public InputActionReference downLight;
    [SerializeField] private Image numpadImg;

    public static PlayerMovement instance;

    private void Start()
    {
        _fire.color = new Color(96, 96, 96, 0.005f);
        GUI.enabled = false;
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();

        if (instance == null)
        {
            instance = this;
        }

        if (running)
        {
            anim.SetBool("isRunning", true);
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
            flameAnim.SetBool("State1", true);
            if (actualCold > 0f)
            {
                actualCold -= Time.deltaTime * 4;
            }
        }
        else if (stateOfFire == 2)
        {
            flameAnim.SetBool("State1", false);
            actualCold += Time.deltaTime;
        }
        else
        {
            flameAnim.SetBool("State1", false);
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
                        if (coldStep4 != null)
                        {
                        coldStep4.color = new Color(255, 255, 255, (actualCold - 3f * coldness) / coldness);
                        }

                        if (stateOfFire != 1)
                        {
                            _fire.color = new Color(Mathf.SmoothStep(_fire.color.r, 128, 0.05f), Mathf.SmoothStep(_fire.color.g, 113, 0.05f), Mathf.SmoothStep(_fire.color.b, 116, 0.05f), 0.005f);
                        }
                        
                    }

                }
                else
                {
                    if (coldStep3 != null)
                    {
                        coldStep3.color = new Color(255, 255, 255, (actualCold - 2f * coldness) / coldness);
                    }

                    if (stateOfFire != 1)
                    {
                        _fire.color = new Color(Mathf.SmoothStep(_fire.color.r, 173, 0.05f), Mathf.SmoothStep(_fire.color.g, 138, 0.05f), Mathf.SmoothStep(_fire.color.b, 123, 0.05f), 0.005f);
                    }
                }

            }
            else
            {
                if (coldStep2 != null)
                {
                    coldStep2.color = new Color(255, 255, 255, (actualCold - coldness) / coldness);
                }

                if (stateOfFire != 1)
                {
                    _fire.color = new Color(Mathf.SmoothStep(_fire.color.r, 255, 0.05f), Mathf.SmoothStep(_fire.color.g, 183, 0.05f), Mathf.SmoothStep(_fire.color.b, 136, 0.05f), 0.005f);
                }
            }

        }
        else
        {
            if(coldStep1 != null)
            {
                 coldStep1.color = new Color(255, 255, 255, actualCold / coldness);
            }

            if(stateOfFire != 1)
            {
                _fire.color = new Color(255, 183, 136, 0.005f);
            }
        }
    }

    private void FixedUpdate()
    {
        anim.SetFloat("yVelocity", _rb.velocity.y);
        anim.SetFloat("xVelocity", _rb.velocity.x);

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
            timeIddle = 0;
            stopTriggerDance = false;
            anim.SetBool("isWalking", true);
            if (!isGrounded)
            {
                maxSpeed = maxSpeedInAir;
                accelerationFactor = accelerationFactorInAir;
                brakeFactor = brakeFactorInAir;
            }
            else
            {
                if (running == true)
                {
                    maxSpeed = maxSpeedOnGround + 2;
                }
                else
                {
                    maxSpeed = maxSpeedOnGround;
                }
                
                accelerationFactor = accelerationFactorOnGround;
                brakeFactor = brakeFactorOnGround;
            }
            speedX = Mathf.MoveTowards(speedX, maxSpeed * horizontal, Time.deltaTime * accelerationFactor);
        }
        else
        {

            anim.SetBool("isWalking", false);
            speedX = Mathf.MoveTowards(speedX, 0f, Time.deltaTime * brakeFactor);

            if (timeIddle <= 20)
            {
                timeIddle += Time.deltaTime;
            }
            else if (timeIddle > 20 && stopTriggerDance == false)
            {
                anim.SetTrigger("Dance");
                stopTriggerDance = true;
            }
        }

        if (ladderInteraction == true)
        {
            anim.SetBool("isClimb", true);
            _rb.velocity = new Vector2(0, speedY);
        }
        else
        {
            anim.SetBool("isClimb", false);
            _rb.velocity = new Vector2(speedX, _rb.velocity.y);
        }


        if (ladderInteraction == true)
        {

            _rb.gravityScale = 0;
            if (vertical != 0)
            {
                anim.SetBool("isClimbing", true);
                speedY = maxSpeed * vertical;
            }
            else 
            {
                anim.SetBool("isClimbing", false);
                speedY = 0;
            }

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


                if (_fire.pointLightOuterRadius <= 2)
                {
                    if (_fire.color == new Color (96,95,111,0.005f))
                    progressiveFire = false;
                    _fire.color = new Color(Mathf.SmoothStep(_fire.color.r, 96, 0.1f), Mathf.SmoothStep(_fire.color.g, 96, 0.1f), Mathf.SmoothStep(_fire.color.b, 96, 0.1f), 0.005f);
                }
                else
                {
                    _fire.pointLightOuterRadius = Mathf.SmoothStep(_fire.pointLightOuterRadius, 1.9f, 0.2f);
                    _fire.color = new Color(Mathf.SmoothStep(_fire.color.r, 96, 0.1f), Mathf.SmoothStep(_fire.color.g, 96, 0.1f), Mathf.SmoothStep(_fire.color.b, 96, 0.1f), 0.005f);
                }
            }


        }

        if (onMucus == true && glassHit1 != null && glassHit2 != null && glassHit3 != null)
            {

            timeInMucus += Time.deltaTime;
            if(timeInMucus > 0.1f && timeInMucus <= 3f/2)
            {
                glassHit1.SetActive(true);
                timeOutMucus = 10f / 2;
            }
            else if (timeInMucus > 3f / 2 && timeInMucus <= 6f / 2)
            {
                glassHit2.SetActive(true);
                timeOutMucus = 15f / 2;
            }
            else if (timeInMucus > 6f / 2 && timeInMucus <= 9f / 2)
            {
                glassHit3.SetActive(true);
                timeOutMucus = 20f / 2;
            }
            else if (timeInMucus > 9f / 2)
            {
                SceneManager.LoadScene("MucusGameOver");
            }
        }
        else if (glassHit1 != null && glassHit2 != null && glassHit3 != null)
        {
            if (timeOutMucus < 0)
            {
                timeOutMucus = 0;
            }
            else if(timeOutMucus > 0)
            {
                timeOutMucus -= Time.deltaTime;
            }

            if (timeOutMucus > 14.5f / 2 && timeOutMucus < 15f / 2)
            {
                glassHit3.SetActive(false);
            }
            else if (timeOutMucus > 9.5f / 2 && timeOutMucus < 10f / 2)
            {
                glassHit2.SetActive(false);
            }
            else if (timeOutMucus > 4.5f / 2 && timeOutMucus < 5f / 2)
            {
                glassHit1.SetActive(false);
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
            anim.SetBool("isJumping", true);
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);
            _rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }

        if (context.canceled && _rb.velocity.y > 0f)
        {
            anim.SetBool("isJumping", false);
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
        }
    }

    public void UpLight(InputAction.CallbackContext light)
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
    public void DownLight(InputAction.CallbackContext light)
    {
        if (light.performed)
        {
            /*if (stateOfFire == 1)
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
            {*/
                stateOfFire = 1;
                progressiveFire = true;
            //}
        }
    }

    public void PauseGame(InputAction.CallbackContext pause)
    {
        if ((pause.performed && numpadImg != null && !numpadImg.IsActive() && canFlip)||(pause.performed && !hasNumpadCanvas && canFlip))
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

        if (collision.gameObject.tag == "Mucus")
        {
            if(glassHit1 != null && glassHit2 != null && glassHit3 != null)
            {
                if (glassHit1.activeSelf == false)
                {
                    timeInMucus = 0f;
                }
                else if (glassHit2.activeSelf == false)
                {
                    timeInMucus = 2.9f;
                }
                else if (glassHit3.activeSelf == false)
                {
                    timeInMucus = 6.9f;
                }
                else
                {
                    SceneManager.LoadScene("MucusGameOver");
                }

            }


            onMucus = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            timeIddle = 0;
            stopTriggerDance = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            anim.SetBool("isGrounded", true);
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
            anim.SetBool("isGrounded", false);
            isGrounded = false;
            launchFallAcceleration = true;
        }
        if (collision.gameObject.tag == "Mucus")
        {
            onMucus = false;
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
    public void PlaySound()
    {
        int x = UnityEngine.Random.Range(0, 5);
        audioSource.clip = clip[x];
        audioSource.Play();
    }
}
