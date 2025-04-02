using UnityEngine;

public class PlayerHealth : MonoBehaviour, IPlayerDamagable
{
    [SerializeField] float maxHealth;

    float Hp
    {
        get => currHealth;

        set
        {
            currHealth = Mathf.Clamp(value, 0f, maxHealth);

            if (currHealth <= 0f && isAlive)
            {
                Dead();
            }
        }
    }

    bool isAlive;

    float currHealth;

    public void ReceiveDamage(float damage)
    {
        Dead();
    }
    void Dead()
    {
        isAlive = false;
    }
    public void Awake()
    {
        Hp = maxHealth;

        isAlive = true;
    }
}
