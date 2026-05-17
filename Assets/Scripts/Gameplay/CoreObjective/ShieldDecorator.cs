public class ShieldDecorator : ICoreDefense
{
    private readonly ICoreDefense _inner;
    private float _shieldHealth;

    public ShieldDecorator(ICoreDefense inner, float shieldCapacity)
    {
        _inner = inner;
        _shieldHealth = shieldCapacity;
    }

    public float ApplyDefense(float incomingDamage)
    {
        if (_shieldHealth <= 0f)
            return _inner.ApplyDefense(incomingDamage);

        float absorbed = UnityEngine.Mathf.Min(_shieldHealth, incomingDamage);
        _shieldHealth -= absorbed;
        EventBus.CoreShieldChanged(_shieldHealth);
        float remaining = incomingDamage - absorbed;
        return _inner.ApplyDefense(remaining);
    }

    public void Tick(float deltaTime) => _inner.Tick(deltaTime);
}
