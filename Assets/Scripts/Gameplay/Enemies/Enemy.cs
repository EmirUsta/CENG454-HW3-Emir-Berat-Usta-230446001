using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IPoolable
{
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float attackRate = 1f;

    private float _currentHealth;
    private float _moveSpeed;
    private float _attackCooldown;
    private bool _stopped;
    private IMovementStrategy _movementStrategy;
    private Transform _coreTransform;
    private Action _returnToPool;

    public bool IsAlive => _currentHealth > 0f;

    public void Setup(IMovementStrategy strategy, Transform coreTarget, float health, float speed, Action returnCallback)
    {
        _movementStrategy = strategy;
        _coreTransform = coreTarget;
        _currentHealth = health;
        _moveSpeed = speed;
        _returnToPool = returnCallback;
        _stopped = false;
    }

    void OnEnable()
    {
        EventBus.OnCoreDestroyed += StopMoving;
    }

    void Update()
    {
        if (!IsAlive || _stopped) return;
        if (_attackCooldown > 0f) _attackCooldown -= Time.deltaTime;
        _movementStrategy?.Move(transform, _coreTransform, _moveSpeed);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!IsAlive || _stopped || _attackCooldown > 0f) return;
        if (other.CompareTag("Core") && other.TryGetComponent<IDamageable>(out var target))
        {
            target.TakeDamage(attackDamage);
            _attackCooldown = attackRate;
        }
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;
        _currentHealth -= amount;
        if (_currentHealth <= 0f) Die();
    }

    private void Die()
    {
        EventBus.EnemyDied(transform.position);
        _returnToPool?.Invoke();
    }

    private void StopMoving() => _stopped = true;

    public void OnSpawned()
    {
        _attackCooldown = 0f;
        _stopped = false;
    }

    public void OnDespawned()
    {
        _returnToPool = null;
        _movementStrategy = null;
        _coreTransform = null;
    }
}
