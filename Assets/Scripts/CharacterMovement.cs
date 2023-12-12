using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Move Settings")]
    

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float accelerationFactor = 0.1f;
    [SerializeField] private float brakeFactor = 0.1f;
    [SerializeField] private float stopMargin = 0.05f;
    [SerializeField] float gravityForce = 5f;

    private LayerMask layerGround;
    private float _speedX;
    private float _speedY;
    public float _fall = 1;
    bool _canUp = false;
    bool _lunchFallAcceleration = true;
    Vector2 _moveDirection;

    [SerializeField] private Rigidbody2D _rigidbody;


    [Header("Input Manager (don't tuch)")]
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
    }

    private void FixedUpdate()
    {
        if (_lunchFallAcceleration)
        {
                _fall = Mathf.SmoothStep(_fall, 1, 0.2f);
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

        if (_canUp == true)
        {
            _rigidbody.velocity = new Vector2(_speedX, _speedY);
        }
        else
        {
            _rigidbody.velocity = new Vector2(_speedX, - gravityForce * _fall);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoUp")
        {
            _canUp = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoUp")
        {
            _canUp = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerGround)
        {
            _fall = 0;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerGround && _canUp == false)
        {
            _lunchFallAcceleration = true;
        }
    }
}
