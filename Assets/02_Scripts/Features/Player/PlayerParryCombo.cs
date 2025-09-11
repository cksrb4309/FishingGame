using TMPro;
using UnityEngine;

public class PlayerParryCombo : MonoBehaviour
{
    public static PlayerParryCombo Instance { get; private set; } = null;

    [SerializeField] TMP_Text comboText;

    [SerializeField] private int baseNeedCombo = 3;
    [SerializeField] private float damage;

    [SerializeField] private int comboCount = 0;
    private void Awake()
    {
        Instance = this;
    }
    int ComboCount
    {
        get => comboCount;
        set
        {
            if (value > 0)
            {
                for (int i = 0; i < (value / baseNeedCombo) - (comboCount / baseNeedCombo); i++)
                {
                    Attack();
                }
            }

            comboCount = value;

            comboText.text = value != 0 ? comboCount.ToString() : string.Empty;
        }
    }

    public void IncreaseCombo(int count = 1)
    {
        ComboCount += count;
    }
    public void DecreaseCombo(int count = 1000000)
    {
        ComboCount = Mathf.Clamp(ComboCount - count, 0, int.MaxValue);
    }
    void Attack()
    {
        Debug.Log("패링 콤보 공격!");

        EnemyData.Current.ModifyHp(-damage);
    }
}
