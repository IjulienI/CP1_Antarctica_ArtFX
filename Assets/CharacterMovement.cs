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

    private float _speed = 5;
    [SerializeField] bool canUp = false;
    Vector2 _moveDirection;

    [SerializeField] private Rigidbody2D _rigidbody;


    [Header("Input Manager (don't tuch)")]
    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference interact;
    public InputActionReference midLight;
    public InputActionReference fullLight;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();

        Debug.Log(_rigidbody.velocity);
    }

    private void FixedUpdate()
    {
        if (canUp == true)
        {
            _rigidbody.velocity = new Vector2(_moveDirection.x * _speed, _moveDirection.y * _speed + 0.2f);
        }
        else
        {
            _rigidbody.velocity = new Vector2(_moveDirection.x * _speed, -_speed);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoUp")
        {
            canUp = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoUp")
        {

            canUp = false;
        }
    }
}
