using UnityEngine;

public class ShopNpc : Npc
{
    public override void Interact()
    {
        ShopUI.Instance.ShowShopUI();
    }
}
