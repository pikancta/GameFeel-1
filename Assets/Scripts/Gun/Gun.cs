using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;

    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCd = .5f;

    private ObjectPool<Bullet> _bulletPool;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _MousePos;
    private float _LastFireTime = 0f;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CreateBulletPool(); 
    }

    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void OnEnable()
    {
      OnShoot += ShootProjectile;
      OnShoot += ResetLastFireTime;
      OnShoot += FireAnimation;
    }


    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= FireAnimation;
    }

    public void ReleaseBulletFromPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(() => { return Instantiate(_bulletPrefab); }, bullet => { bullet.gameObject.SetActive(true); }, bullet => { bullet.gameObject.SetActive(false); }, Bullet => { Destroy(Bullet); }, false, 20, 40);
    }

    private Bullet CreatenewBullet()
    {
        return Instantiate(_bulletPrefab);
    }

    private void ActiveBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
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
        FireAnimation();
        Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.Init(this, _bulletSpawnPoint.position, _MousePos);
    }

    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH,0,.5f);
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
