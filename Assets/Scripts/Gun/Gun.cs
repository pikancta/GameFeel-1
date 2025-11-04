using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;

    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCd = .5f;

    private Vector2 _MousePos;
    private float _LastFireTime = 0f;
    
    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void OnEnable()
    {
      OnShoot += ShootProjectile;
      OnShoot += ResetLastFireTime;
    }


    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= _LastFireTime)
        {
            OnShoot?.Invoke();
        }
    }

    private void ShootProjectile()
    {

        Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.Init(_bulletSpawnPoint.position, _MousePos);
    }

    private void ResetLastFireTime()
    {
        _LastFireTime = Time.time + _gunFireCd;
    }

    private void RotateGun()
    {
        _MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_MousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
