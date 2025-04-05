using UnityEngine;

public class Block : MonoBehaviour, IDamagable
{
    [SerializeField] BlockData blockData;

    float Hp
    {
        get => hp;

        set
        {
            hp = Mathf.Clamp(value, 0f, blockData.MaxHp);

            if (hp <= 0f)
            {
                Break();
            }
        }
    }

    protected bool isAlive;

    float hp;

    protected virtual void Break()
    {
        // TODO : 블럭 파괴는 우선 비활성화로 ㄱㄱ (변경요함)
        gameObject.SetActive(false);
    }
    public void Awake()
    {
        Hp = blockData.MaxHp;

        isAlive = true;
    }

    public virtual void ReceiveDamage(float damage)
    {
        if (!isAlive) return;

        Hp -= damage;
    }
}
