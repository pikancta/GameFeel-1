using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;

    private float _moveX;
    private bool _canMove = true;

    private Rigidbody2D _rigidbody;
    private Knockback _knockback;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<Knockback>();
    }

    private void OnEnable()
    {
        _knockback.OnKnockbackStart += CanMoveFalse;
        _knockback.OnKnockbackEnd += CanMoveTrue;
    }

    private void OnDisable()
    {
        _knockback.OnKnockbackStart -= CanMoveFalse;
        _knockback.OnKnockbackEnd -= CanMoveTrue;
    }

    private void CanMoveFalse()
    {
        _canMove = true;
    }

    private void CanMoveTrue()
    {
        _canMove = false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetCurrentDirection(float currentDirection)
    {

        _moveX = currentDirection;
    }

    private void Move()
    {
        if (!_canMove) {return;}
        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidbody.linearVelocity.y);
        _rigidbody.linearVelocity = movement;
    }


}
