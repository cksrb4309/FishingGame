using UnityEngine;
using TMPro;
public class UserController : MonoBehaviour
{
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
    public void SetGame(IFishingSystem game)
    {
        this.game = game;


    }
    void Die()
    {

    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        
    }
    public void Setting()
    {
        
    }
    public void Cancel()
    {

    }
}
