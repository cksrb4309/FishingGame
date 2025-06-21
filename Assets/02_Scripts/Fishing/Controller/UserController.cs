using UnityEngine;
using TMPro;
public class UserController : MonoBehaviour
{
    public static UserController Instance { get; private set; } = null;
    [SerializeField] TMP_Text hpText;
    IFishingSystem game = null;
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
    public void SetGame(IFishingSystem game)
    {
        this.game = game;


    }
    public void ReceiveDamage(int damage)
    {
        Hp -= damage;
    }
    void Die()
    {

    }
    public void Setting()
    {
        
    }
    public void Cancel()
    {

    }
}
