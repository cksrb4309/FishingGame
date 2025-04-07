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
        // TODO : �� �ı��� �켱 ��Ȱ��ȭ�� ���� (�������)
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
