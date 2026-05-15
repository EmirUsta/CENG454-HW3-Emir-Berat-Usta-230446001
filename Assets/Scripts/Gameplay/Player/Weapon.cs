using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float fireRate = 0.3f;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private float projectileLifetime = 3f;
    [SerializeField] private int poolInitialSize = 10;

    private ObjectPool<Projectile> _pool;
    private float _cooldown;

    void Awake()
    {
        var poolParent = new GameObject("ProjectilePool").transform;
        _pool = new ObjectPool<Projectile>(projectilePrefab, poolParent, poolInitialSize);
    }

    void Update()
    {
        if (_cooldown > 0f) _cooldown -= Time.deltaTime;
        if (Input.GetButton("Fire1") && _cooldown <= 0f)
            Fire();
    }

    private void Fire()
    {
        _cooldown = fireRate;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector2 direction = (mouseWorld - transform.position).normalized;

        Projectile p = _pool.Get();
        p.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        p.Setup(direction, projectileSpeed, projectileDamage, projectileLifetime, () => _pool.Return(p));
    }
}
