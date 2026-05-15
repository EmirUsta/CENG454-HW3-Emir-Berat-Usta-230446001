using System;
using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    private float _speed;
    private float _damage;
    private float _timer;
    private Vector2 _direction;
    private Action _returnToPool;

    public void Setup(Vector2 direction, float speed, float damage, float lifetime, Action returnCallback)
    {
        _direction = direction.normalized;
        _speed = speed;
        _damage = damage;
        _timer = lifetime;
        _returnToPool = returnCallback;
    }

    void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            _returnToPool?.Invoke();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(_damage);
            _returnToPool?.Invoke();
        }
    }

    public void OnSpawned() { }

    public void OnDespawned()
    {
        _returnToPool = null;
    }
}
