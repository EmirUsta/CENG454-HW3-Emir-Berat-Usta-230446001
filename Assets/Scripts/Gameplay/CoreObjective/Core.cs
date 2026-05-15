using UnityEngine;

public class Core : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float shieldCapacity = 30f;
    [SerializeField] private float armorReduction = 0.2f;
    [SerializeField] private float regenPerSecond = 1f;

    private float _currentHealth;
    private ICoreDefense _defense;

    public bool IsAlive => _currentHealth > 0f;
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => maxHealth;

    void Awake()
    {
        _currentHealth = maxHealth;
        _defense = new RegenDecorator(
            new ArmorDecorator(
                new ShieldDecorator(
                    new NullCoreDefense(), shieldCapacity),
                armorReduction),
            regenPerSecond, Heal);
    }

    void Update()
    {
        if (!IsAlive) return;
        _defense.Tick(Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;
        float finalDamage = _defense.ApplyDefense(amount);
        _currentHealth = Mathf.Max(_currentHealth - finalDamage, 0f);

        if (IsAlive)
            EventBus.CoreDamaged(_currentHealth, maxHealth);
        else
            EventBus.CoreDestroyed();
    }

    private void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
    }
}
