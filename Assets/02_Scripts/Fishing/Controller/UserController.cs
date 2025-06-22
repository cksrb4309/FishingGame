using UnityEngine;
using TMPro;
public class UserController : MonoBehaviour
{
    public static UserController Instance { get; private set; } = null;
    public Vector2 Position => transform.position;
    [SerializeField] TMP_Text hpText;
    [SerializeField] SpriteRenderer myRenderer;
    bool isAlive = false;
    int Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;

            hpText.text = hp > 0 ? hp.ToString() : "0";

            if (hp <= 0) Die();
        }
    }
    int hp;
    private void Awake()
    {
        Instance = this;
    }
    public void ReceiveDamage(int damage)
    {
        Hp -= damage;

        Debug.Log("데미지 : " + damage.ToString() + " / 현재 체력 : " + Hp.ToString());
    }
    void Die()
    {
        if (!isAlive) return;

        isAlive = false;

        Debug.Log("User Die");

        Fishing_Game.fishing_Game.CancelFishing();
    }
    public void Setting()
    {
        Hp = FishingData.MiniGame_4_Data.MaxHp;

        isAlive = true;

        myRenderer.color = Color.white;
    }
    public void Cancel()
    {
        myRenderer.color = new Color(0, 0, 0, 0);
    }
}
