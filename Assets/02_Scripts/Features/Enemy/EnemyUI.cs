using UnityEngine;
using UnityEngine.UI;
public class EnemyUI : MonoBehaviour
{
    public static EnemyUI Instance { get; private set; } = null;
    
    [SerializeField] Image hp_Image;

    private void ModifyHp()
    {
        hp_Image.fillAmount = EnemyData.Current.GetHpRatio();
    }
    public void Init()
    {
        EnemyData.Current.hpModifyAction += ModifyHp;
    }
    public void Release()
    {
        EnemyData.Current.hpModifyAction -= ModifyHp;
    }
    private void Awake()
    {
        Instance = this;
    }
}
