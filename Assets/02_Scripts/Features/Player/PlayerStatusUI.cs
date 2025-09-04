using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField] TMP_Text hp_Text;
    [SerializeField] TMP_Text sp_Text;
    [SerializeField] Image hp_Image;
    [SerializeField] Image sp_Image;

    private void Start()
    {
        ModifyHp();
        ModifySp();
    }
    public void ModifyHp()
    {
        hp_Text.text = PlayerData.Data.GetMaxHp().ToString("F0") + " / " + PlayerData.Data.GetHp().ToString("F0");
        hp_Image.fillAmount = PlayerData.Data.GetHpRatio();
    }
    public void ModifySp()
    {
        sp_Text.text = PlayerData.Data.GetMaxSp().ToString("F0") + " / " + PlayerData.Data.GetSp().ToString("F0");
        sp_Image.fillAmount = PlayerData.Data.GetSpRatio();
    }
    private void OnEnable()
    {
        PlayerData.Data.hpModifyAction += ModifyHp;
        PlayerData.Data.spModifyAction += ModifySp;
    }
    private void OnDisable()
    {
        PlayerData.Data.hpModifyAction -= ModifyHp;
        PlayerData.Data.spModifyAction -= ModifySp;
    }
}
