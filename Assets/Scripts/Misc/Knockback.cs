using System;
using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public Action OnKnockbackStart;
    public Action OnKnockbackEnd;

    [SerializeField] private float _knockbackTime = .2f;

    private Rigidbody2D _rigidbody;
    private Vector3 _hitDirection;
    private float _knockbackThrust;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Vector3 hitDirection, float knockbackThrust)
    {
        _hitDirection = hitDirection;
        _knockbackThrust = knockbackThrust;

        OnKnockbackStart?.Invoke();
    }

    private void ApplyKnockbackForce()
    {
        Vector3 difference = (transform.position - _hitDirection).normalized * _knockbackThrust * _rigidbody.mass;
        _rigidbody.AddForce(difference, ForceMode2D.Impulse);
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(_knockbackTime);
    }

    private void OnEnable()
    {
        OnKnockbackStart += ApplyKnockbackForce;
        OnKnockbackEnd += StopKnockRoutine;
    }

    private void OnDisable()
    {
        OnKnockbackStart -= ApplyKnockbackForce;
        OnKnockbackEnd -= StopKnockRoutine;
    }

    private void StopKnockRoutine()
    {
        _rigidbody.linearVelocity = Vector2.zero;
    }

}
