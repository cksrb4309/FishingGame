using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float damage;
    protected float speed;
    protected float range;

    protected Vector2 targetPos;

    public virtual void Setting(float damage, float range, float speed, float angle, Vector2 targetPos)
    {
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        this.targetPos = targetPos;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        gameObject.SetActive(true);

        OnSettingComplete();
    }
    protected virtual void OnSettingComplete() { }
}
