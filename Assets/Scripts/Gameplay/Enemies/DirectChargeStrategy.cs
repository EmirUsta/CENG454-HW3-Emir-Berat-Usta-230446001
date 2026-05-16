using UnityEngine;

public class DirectChargeStrategy : IMovementStrategy
{
    public void Move(Transform self, Transform target, float speed)
    {
        if (target == null) return;
        self.position = Vector2.MoveTowards(self.position, target.position, speed * Time.deltaTime);
    }
}
