public class ArmorDecorator : ICoreDefense
{
    private readonly ICoreDefense _inner;
    private readonly float _reductionPercent;

    public ArmorDecorator(ICoreDefense inner, float reductionPercent)
    {
        _inner = inner;
        _reductionPercent = UnityEngine.Mathf.Clamp01(reductionPercent);
    }

    public float ApplyDefense(float incomingDamage)
    {
        float reduced = incomingDamage * (1f - _reductionPercent);
        return _inner.ApplyDefense(reduced);
    }

    public void Tick(float deltaTime) => _inner.Tick(deltaTime);
}
