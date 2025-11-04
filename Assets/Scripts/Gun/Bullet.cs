using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;

    private Vector2 _fireDirection;

    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        _rigidBody.linearVelocity = _fireDirection * _moveSpeed;
    }

    public void Init(Vector2 _BulletSpawnPos, Vector2 _MousePos)
    {
        _fireDirection = (_MousePos  - _BulletSpawnPos).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damageAmount);
        Destroy(this.gameObject);
    }
}