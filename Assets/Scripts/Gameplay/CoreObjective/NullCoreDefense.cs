public class NullCoreDefense : ICoreDefense
{
    public float ApplyDefense(float incomingDamage) => incomingDamage;
    public void Tick(float deltaTime) { }
}
