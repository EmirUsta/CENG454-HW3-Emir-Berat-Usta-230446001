using UnityEngine;

public class ZigzagStrategy : IMovementStrategy
{
    private readonly float _frequency;
    private readonly float _amplitude;
    private float _time;

    public ZigzagStrategy(float frequency = 2f, float amplitude = 1.5f)
    {
        _frequency = frequency;
        _amplitude = amplitude;
    }

    public void Move(Transform self, Transform target, float speed)
    {
        if (target == null) return;
        _time += Time.deltaTime;
        Vector2 toTarget = (target.position - self.position).normalized;
        Vector2 perpendicular = new Vector2(-toTarget.y, toTarget.x);
        Vector2 direction = toTarget + perpendicular * Mathf.Sin(_time * _frequency) * _amplitude;
        self.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
    }
}
