using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public static ShopUI Instance { get; private set; } = null;

    [SerializeField] GameObject shopUIObj;

    public void ShowShopUI()
    {
        if (shopUIObj.activeSelf) return;

        shopUIObj.SetActive(true);

    }
}
