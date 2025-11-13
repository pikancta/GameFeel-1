using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public static Action OnJump;

    [SerializeField] private Transform _feetTransform;
    [SerializeField] private Vector2 _groundCheck;
    [SerializeField] private LayerMask _groundlayer;
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = .2f;
    [SerializeField] private float _coyoteTime = .5f;

    private PlayerInput _playerInput;
    private PlayerInput.FrameInput _frameInput;

    private bool _doubleJumpAvaliable;
    private float _TimeInAir, _coyoteTimer;
    private Movement _movement;
    private Rigidbody2D _rigidBody;

    public void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        OnJump += ApplyJumpForce;
    }

    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;
    }

    private void Update()
    {
        Movement();
        GatherInput();
        CoyoteTimer();
        HandleJump();
        HandleSpriteFlip();
    }

    private void FixedUpdate()
    {
        ExtraGravity();
    }

    private bool CheckGrounded()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(_feetTransform.position, _groundCheck, 0f, _groundlayer);
        return isGrounded;
    }

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    private void GatherInput()
    {
        _frameInput = _playerInput.frameInput;
    }

    private void Movement() 
    {
        _movement.SetCurrentDirection(_frameInput.Move.x);
    }

    private void HandleJump()
    {

        if (!_frameInput.Jump)
        {
            return;
        }


        if (CheckGrounded())
        {
            OnJump?.Invoke();
        }
        else if(_coyoteTimer > 0f)
        {
            OnJump?.Invoke();
        }
        else if (_doubleJumpAvaliable)
        {
            _doubleJumpAvaliable = false;
            OnJump?.Invoke();
        }

        
    }

    private void ApplyJumpForce()
    {
        _rigidBody.linearVelocity = Vector2.zero;
        _TimeInAir = 0f;
        _coyoteTimer = 0f;
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }

    private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_feetTransform.position, _groundCheck);
    }

    private void GravityDelay()
    {
        if(!CheckGrounded())
        {
            _TimeInAir += Time.deltaTime;
        }
        else
        {
            _TimeInAir = 0f;
        }


    }

    private void ExtraGravity()
    {
        if(_TimeInAir > _gravityDelay)
        {
            _rigidBody.AddForce(new Vector2(0f, -_extraGravity * Time.deltaTime));
        }
    }

    private void CoyoteTimer()
    {
        if (CheckGrounded())
        {
            _coyoteTimer = _coyoteTime;
            _doubleJumpAvaliable = true;
        }
        else
        {
            _coyoteTime -= Time.deltaTime;
        }
    }






}
