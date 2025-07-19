using UnityEngine;

public class ShopNpc : Npc
{
    public override void Interact()
    {
        PlayerInventory.Instance.ShowShop();
    }
    public override void Release()
    {
        base.Release();

        PlayerInventory.Instance.HideShop();
    }
}
