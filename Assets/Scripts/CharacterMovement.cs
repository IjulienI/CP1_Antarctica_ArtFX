using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    
    

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float accelerationFactor = 0.1f;
    [SerializeField] private float brakeFactor = 0.1f;
    [SerializeField] private float stopMargin = 0.05f;
    [SerializeField] float gravityForce = 5f;
    [SerializeField] private float jumpStrength = 8f;

    private LayerMask layerGround;
    private float _speedX;
    private float _speedY;
    public float _fall = 1;
    bool _canUp = false;
    bool _canDown = false;
    [SerializeField] bool _isGrounded = false;
    //bool _lunchJump = false;
    bool _lunchFallAcceleration = false;
    bool _ladderInteraction = false;
    bool _onTriggerLadder = false;
    Vector2 _moveDirection;
    private float coyoteTime = 0.1f;
    private float coyoteTimer;


    [SerializeField] private Rigidbody2D _rigidbody;


    [Header("Input Manager (don't tuch)")]
    [SerializeField] private BoxCollider2D _collider;
    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference interact;
    public InputActionReference midLight;
    public InputActionReference fullLight;

    private void Awake()
    {
        layerGround = LayerMask.NameToLayer("Ground");
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();

        //Debug.Log(_rigidbody.velocity);

        if (_ladderInteraction == false && _canUp == true)
        {
            _fall = 0.1f;
            _lunchFallAcceleration = true;
            _canUp = false;
        }

        if (_isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (_lunchFallAcceleration == true)
        {
            _fall = Mathf.SmoothStep(_fall, 1, 0.1f); 
            if (_fall == 0)
            {
                _lunchFallAcceleration = false;
            }
        }
        if (_moveDirection.x != 0)
        {
            _speedX = Mathf.SmoothStep(_rigidbody.velocity.x, maxSpeed * _moveDirection.x, accelerationFactor);
        }
        else if (Mathf.Abs(_speedX) > stopMargin)
        {
            _speedX = Mathf.SmoothStep(_rigidbody.velocity.x, 0, brakeFactor);
        }
        else
        {
            _speedX = 0;
        }
        if (_moveDirection.y != 0)
        {
            _speedY = maxSpeed * _moveDirection.y ;
        }
        else
        {
            _speedY = 0;
        }

        if (_ladderInteraction == true && _isGrounded)
        {
            _rigidbody.velocity = new Vector2(_speedX, _speedY);
        }
        else if (_ladderInteraction == true)
        {
            _rigidbody.velocity = new Vector2(0, _speedY);
        }
        else if (_moveDirection.y < 0 && _canDown == true)
        {
            StartCoroutine(GoDown());
        }
        else
        {
            _rigidbody.velocity = new Vector2(_speedX, - gravityForce * _fall);
        }



    }

    IEnumerator GoDown()
    {
        _collider.isTrigger = true;
        _lunchFallAcceleration = true;
        yield return new WaitForSeconds(0.5f);
        _collider.isTrigger = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoUp")
        {
            _onTriggerLadder = true;
        }

        if (collision.gameObject.tag == "GoUp" && _ladderInteraction == true)
        {
            _canUp = true;
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
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerGround)
        {
            _fall = 0;
            _lunchFallAcceleration = false;
            _isGrounded = true;
        }
        if (collision.gameObject.tag == "FlyingPlatform")
        {
            _canDown = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerGround && _canUp == false)
        {
            _lunchFallAcceleration = true;
            _isGrounded = true;
        }
        if (collision.gameObject.tag == "FlyingPlatform")
        {
            _canDown = false;
        }
    }

    private void OnEnable()
    {
        interact.action.started += Interaction;
        jump.action.started += Jumping;
    }

    private void Interaction(InputAction.CallbackContext obj)
    {
        if (_onTriggerLadder)
        {
            _ladderInteraction = !_ladderInteraction;
        }
    }
    private void Jumping(InputAction.CallbackContext obj)
    {
        if (coyoteTime > 0f)
        {
        LunchJump();
        }

    }

    private void LunchJump()
    {
            _isGrounded = true;
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
            _rigidbody.AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
    }
}
