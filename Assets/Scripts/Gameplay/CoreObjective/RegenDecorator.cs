using System;

public class RegenDecorator : ICoreDefense
{
    private readonly ICoreDefense _inner;
    private readonly float _healPerSecond;
    private readonly Action<float> _onHeal;

    public RegenDecorator(ICoreDefense inner, float healPerSecond, Action<float> onHeal)
    {
        _inner = inner;
        _healPerSecond = healPerSecond;
        _onHeal = onHeal;
    }

    public float ApplyDefense(float incomingDamage) => _inner.ApplyDefense(incomingDamage);

    public void Tick(float deltaTime)
    {
        _onHeal?.Invoke(_healPerSecond * deltaTime);
        _inner.Tick(deltaTime);
    }
}
